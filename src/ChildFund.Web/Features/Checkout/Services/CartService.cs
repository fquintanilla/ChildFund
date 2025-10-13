using ChildFund.Web.Features.Checkout.ViewModels;
using ChildFund.Web.Features.NamedCarts;
using ChildFund.Web.Infrastructure.Commerce.Extensions;
using ChildFund.Web.Infrastructure.Commerce.Markets;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Security;
using Mediachase.Commerce.Customers;
using Mediachase.Commerce.Inventory;
using Mediachase.Commerce.Security;
using ReferenceConverter = Mediachase.Commerce.Catalog.ReferenceConverter;

namespace ChildFund.Web.Features.Checkout.Services
{
    public class CartService(
        IOrderGroupFactory orderGroupFactory,
        IPlacedPriceProcessor placedPriceProcessor,
        IInventoryProcessor inventoryProcessor,
        ILineItemValidator lineItemValidator,
        IOrderRepository orderRepository,
        IPromotionEngine promotionEngine,
        ICurrentMarket currentMarket,
        ReferenceConverter referenceConverter,
        IContentLoader contentLoader,
        IRelationRepository relationRepository,
        IWarehouseRepository warehouseRepository,
        ILineItemCalculator lineItemCalculator,
        ICurrencyService currencyService)
        : ICartService
    {
        private readonly string VariantOptionCodesProperty = "VariantOptionCodes";
        private readonly CustomerContext _customerContext = CustomerContext.Current;

        public string DefaultCartName => "Default" + SiteDefinition.Current.StartPage.ID;

        public Dictionary<ILineItem, List<ValidationIssue>> ChangeCartItem(ICart cart, int shipmentId, string code, decimal quantity, string size, string newSize)
        {
            _ = new Dictionary<ILineItem, List<ValidationIssue>>();
            Dictionary<ILineItem, List<ValidationIssue>> validationIssues;
            if (quantity > 0)
            {
                validationIssues = ChangeQuantity(cart, shipmentId, code, quantity);
            }
            else
            {
                validationIssues = RemoveLineItem(cart, shipmentId, code);
            }

            return validationIssues;
        }

        public AddToCartResult AddToCart(ICart cart, RequestParamsToCart requestParams)
        {
            var contentLink = referenceConverter.GetContentLink(requestParams.Code);
            var entryContent = contentLoader.Get<EntryContentBase>(contentLink);
            return AddToCart(cart, entryContent, requestParams.Quantity, requestParams.Store, requestParams.SelectedStore, requestParams.DynamicCodes);
        }

        public AddToCartResult AddToCart(ICart cart, EntryContentBase entryContent, decimal quantity, string deliveryMethod, string warehouseCode, List<string> dynamicVariantOptionCodes)
        {
            var result = new AddToCartResult();
            var contact = PrincipalInfo.CurrentPrincipal.GetCustomerContact();

            if (contact?.OwnerId != null)
            {
                var org = cart.GetString("OwnerOrg");
                if (string.IsNullOrEmpty(org))
                {
                    cart.Properties["OwnerOrg"] = contact.OwnerId.Value.ToString().ToLower();
                }
            }

            IWarehouse warehouse = null;

            if (deliveryMethod.Equals("instore") && !string.IsNullOrEmpty(warehouseCode))
            {
                warehouse = warehouseRepository.Get(warehouseCode);
            }

            if (entryContent is BundleContent)
            {
                foreach (var relation in relationRepository.GetChildren<BundleEntry>(entryContent.ContentLink))
                {
                    var entry = contentLoader.Get<EntryContentBase>(relation.Child);
                    var recursiveResult = AddToCart(cart, entry, (relation.Quantity ?? 1) * quantity, deliveryMethod, warehouseCode, dynamicVariantOptionCodes);
                    if (recursiveResult.EntriesAddedToCart)
                    {
                        result.EntriesAddedToCart = true;
                    }

                    foreach (var message in recursiveResult.ValidationMessages)
                    {
                        result.ValidationMessages.Add(message);
                    }
                }

                return result;
            }

            var form = cart.GetFirstForm();
            if (form == null)
            {
                form = orderGroupFactory.CreateOrderForm(cart);
                form.Name = cart.Name;
                cart.Forms.Add(form);
            }

            var shipment = cart.GetFirstForm().Shipments.FirstOrDefault(x => string.IsNullOrEmpty(warehouseCode));

            if (shipment == null)
            {
                var cartFirstShipment = cart.GetFirstShipment();
                if (cartFirstShipment == null)
                {
                    shipment = orderGroupFactory.CreateShipment(cart);
                    cart.GetFirstForm().Shipments.Add(shipment);
                }
                else
                {
                    if (cartFirstShipment.LineItems.Count > 0)
                    {
                        shipment = orderGroupFactory.CreateShipment(cart);
                        cart.GetFirstForm().Shipments.Add(shipment);
                    }
                    else
                    {
                        shipment = cartFirstShipment;
                    }
                }
            }

            var lineItem = shipment.LineItems.FirstOrDefault(x => x.Code == entryContent.Code);
            decimal originalLineItemQuantity = 0;

            if (lineItem == null)
            {
                lineItem = cart.CreateLineItem(entryContent.Code, orderGroupFactory);
                var lineDisplayName = entryContent.DisplayName;
                if (dynamicVariantOptionCodes?.Count > 0)
                {
                    lineItem.Properties[VariantOptionCodesProperty] = string.Join(",", dynamicVariantOptionCodes.OrderBy(x => x));
                    lineDisplayName += " - " + lineItem.Properties[VariantOptionCodesProperty];
                }

                lineItem.DisplayName = lineDisplayName;
                lineItem.Quantity = quantity;
                cart.AddLineItem(shipment, lineItem);
            }
            else
            {
                if (lineItem.Properties[VariantOptionCodesProperty] != null)
                {
                    var variantOptionCodesLineItem = lineItem.Properties[VariantOptionCodesProperty].ToString().Split(',');
                    var intersectCodes = variantOptionCodesLineItem.Intersect(dynamicVariantOptionCodes);

                    if (intersectCodes != null && intersectCodes.Any()
                        && intersectCodes.Count() == variantOptionCodesLineItem.Length
                        && intersectCodes.Count() == dynamicVariantOptionCodes.Count)
                    {
                        originalLineItemQuantity = lineItem.Quantity;
                        cart.UpdateLineItemQuantity(shipment, lineItem, lineItem.Quantity + quantity);
                    }
                    else
                    {
                        lineItem = cart.CreateLineItem(entryContent.Code, orderGroupFactory);
                        lineItem.Properties[VariantOptionCodesProperty] = string.Join(",", dynamicVariantOptionCodes.OrderBy(x => x));
                        lineItem.DisplayName = entryContent.DisplayName + " - " + lineItem.Properties[VariantOptionCodesProperty];
                        lineItem.Quantity = quantity;
                        cart.AddLineItem(shipment, lineItem);
                    }
                }
                else
                {
                    originalLineItemQuantity = lineItem.Quantity;
                    cart.UpdateLineItemQuantity(shipment, lineItem, lineItem.Quantity + quantity);
                }
            }

            var validationIssues = ValidateCart(cart);
            var newLineItem = shipment.LineItems.FirstOrDefault(x => x.Code == entryContent.Code);
            var isAdded = (newLineItem != null ? newLineItem.Quantity : 0) - originalLineItemQuantity > 0;

            AddValidationMessagesToResult(result, lineItem, validationIssues, isAdded);

            return result;
        }

        public void SetCartCurrency(ICart cart, Currency currency)
        {
            if (currency.IsEmpty || currency == cart.Currency)
            {
                return;
            }

            cart.Currency = currency;
            foreach (var lineItem in cart.GetAllLineItems())
            {
                // If there is an item which has no price in the new currency, a NullReference exception will be thrown.
                // Mixing currencies in cart is not allowed.
                // It's up to site's managers to ensure that all items have prices in allowed currency.
                lineItem.PlacedPrice = PriceCalculationService.GetSalePrice(lineItem.Code, cart.MarketId, currency).UnitPrice.Amount;
            }

            ValidateCart(cart);
        }

        public Dictionary<ILineItem, List<ValidationIssue>> ValidateCart(ICart cart)
        {
            var validationIssues = new Dictionary<ILineItem, List<ValidationIssue>>();

            cart.ValidateOrRemoveLineItems((item, issue) => validationIssues.AddValidationIssues(item, issue), lineItemValidator);
            cart.UpdatePlacedPriceOrRemoveLineItems(_customerContext.GetContactById(cart.CustomerId), (item, issue) => validationIssues.AddValidationIssues(item, issue), placedPriceProcessor);
            cart.UpdateInventoryOrRemoveLineItems((item, issue) => validationIssues.AddValidationIssues(item, issue), inventoryProcessor);
            cart.ApplyDiscounts(promotionEngine, new PromotionEngineSettings());

            return validationIssues;
        }

        public CartWithValidationIssues LoadCart(string name, bool validate) => LoadCart(name, _customerContext.CurrentContactId.ToString(), validate);

        public CartWithValidationIssues LoadCart(string name, string contactId, bool validate)
        {
            var validationIssues = new Dictionary<ILineItem, List<ValidationIssue>>();
            var cart = !string.IsNullOrEmpty(contactId) ? orderRepository.LoadOrCreateCart<ICart>(new Guid(contactId), name, currentMarket) : null;
            if (cart != null)
            {
                SetCartCurrency(cart, currencyService.GetCurrentCurrency());
                if (validate)
                {
                    validationIssues = ValidateCart(cart);
                    if (validationIssues.Any())
                    {
                        orderRepository.Save(cart);
                    }
                }
            }

            return new CartWithValidationIssues
            {
                Cart = cart,
                ValidationIssues = validationIssues
            };
        }

        public ICart LoadOrCreateCart(string name) => LoadOrCreateCart(name, _customerContext.CurrentContactId.ToString());

        public ICart LoadOrCreateCart(string name, string contactId)
        {
            if (string.IsNullOrEmpty(contactId))
            {
                return null;
            }
            else
            {
                var cart = orderRepository.LoadOrCreateCart<ICart>(new Guid(contactId), name, currentMarket);
                if (cart != null)
                {
                    SetCartCurrency(cart, currencyService.GetCurrentCurrency());
                }

                return cart;
            }
        }

        public Dictionary<ILineItem, List<ValidationIssue>> ChangeQuantity(ICart cart, int shipmentId, string code, decimal quantity)
        {
            if (quantity == 0)
            {
                return RemoveLineItem(cart, shipmentId, code);
            }
            else
            {
                var shipment = cart.GetFirstForm().Shipments.First(s => s.ShipmentId == shipmentId || shipmentId == 0);
                var lineItem = shipment.LineItems.FirstOrDefault(x => x.Code == code);
                if (lineItem == null)
                {
                    throw new InvalidOperationException($"No lineitem with matching code '{code}' for shipment id {shipmentId}");
                }

                cart.UpdateLineItemQuantity(shipment, lineItem, quantity);
            }

            return ValidateCart(cart);
        }

        private Dictionary<ILineItem, List<ValidationIssue>> RemoveLineItem(ICart cart, int shipmentId, string code)
        {
            if (cart.GetFirstForm().Shipments.Any())
            {
                //gets  the shipment for shipment id or for wish list shipment id as a parameter is always equal zero( wish list).
                var shipment = cart.GetFirstForm().Shipments.FirstOrDefault(s => s.ShipmentId == shipmentId || shipmentId == 0);
                if (shipment == null)
                {
                    throw new InvalidOperationException($"No shipment with matching id {shipmentId}");
                }

                var lineItem = shipment.LineItems.FirstOrDefault(l => l.Code == code);
                if (lineItem != null)
                {
                    shipment.LineItems.Remove(lineItem);
                }

                if (!shipment.LineItems.Any())
                {
                    cart.GetFirstForm().Shipments.Remove(shipment);
                }

                if (!cart.GetFirstForm().GetAllLineItems().Any())
                {
                    orderRepository.Delete(cart.OrderLink);
                }
            }

            return ValidateCart(cart);
        }

        private static void AddValidationMessagesToResult(AddToCartResult result, ILineItem lineItem, Dictionary<ILineItem, List<ValidationIssue>> validationIssues, bool isHasAddedItem)
        {
            foreach (var validationIssue in validationIssues)
            {
                var warning = new StringBuilder();
                warning.Append(string.Format("Line Item with code {0} ", lineItem.Code));
                validationIssue.Value.Aggregate(warning, (current, issue) => current.Append(issue).Append(", "));

                result.ValidationMessages.Add(warning.ToString().TrimEnd(',', ' '));
            }

            if (!validationIssues.HasItemBeenRemoved(lineItem) && isHasAddedItem)
            {
                result.EntriesAddedToCart = true;
            }
        }

        private Dictionary<ILineItem, List<ValidationIssue>> UpdateLineItemSku(ICart cart, int shipmentId, string oldCode, string newCode, decimal quantity)
        {
            RemoveLineItem(cart, shipmentId, oldCode);

            //merge same sku's
            var newLineItem = GetFirstLineItem(cart, newCode);
            if (newLineItem != null)
            {
                var shipment = cart.GetFirstForm().Shipments.First(s => s.ShipmentId == shipmentId || shipmentId == 0);
                cart.UpdateLineItemQuantity(shipment, newLineItem, newLineItem.Quantity + quantity);
            }
            else
            {
                newLineItem = cart.CreateLineItem(newCode, orderGroupFactory);
                newLineItem.Quantity = quantity;
                cart.AddLineItem(newLineItem, orderGroupFactory);

                var price = PriceCalculationService.GetSalePrice(newCode, cart.MarketId, currentMarket.GetCurrentMarket().DefaultCurrency);
                if (price != null)
                {
                    newLineItem.PlacedPrice = price.UnitPrice.Amount;
                }
            }

            return ValidateCart(cart);
        }

        public Money? GetDiscountedPrice(ICart cart, ILineItem lineItem)
        {
            return lineItem.GetDiscountedPrice(cart.Currency, lineItemCalculator);
        }

        private ILineItem GetFirstLineItem(IOrderGroup cart, string code) => cart.GetAllLineItems().FirstOrDefault(x => x.Code == code);

        public ICart CreateNewCart()
        {
            return orderRepository.LoadOrCreateCart<ICart>(PrincipalInfo.CurrentPrincipal.GetContactId(),
                DefaultCartName);
        }

        public void DeleteCart(ICart cart) => orderRepository.Delete(cart.OrderLink);

        public ICart PlaceOrderToCart(IPurchaseOrder purchaseOrder, ICart cart)
        {
            var returnCart = cart;
            var lineItems = purchaseOrder.GetAllLineItems();
            foreach (var lineItem in lineItems)
            {
                cart.AddLineItem(lineItem);
                lineItem.IsInventoryAllocated = false;
            }

            return returnCart;
        }
    }
}