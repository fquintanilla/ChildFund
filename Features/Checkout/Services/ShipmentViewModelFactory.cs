using ChildFund.Features.Checkout.ViewModels;
using ChildFund.Features.MyAccount.AddressBook;
using ChildFund.Infrastructure.Commerce.Markets;
using Mediachase.Commerce.Markets;
using Mediachase.Commerce.Orders;
using ReferenceConverter = Mediachase.Commerce.Catalog.ReferenceConverter;

namespace ChildFund.Features.Checkout.Services
{
	public class ShipmentViewModelFactory(
        IContentLoader contentLoader,
        IShippingService shippingService,
        LanguageService languageService,
        ReferenceConverter referenceConverter,
        IAddressBookService addressBookService,
        CartItemViewModelFactory cartItemViewModelFactory,
        IContentLanguageAccessor contentLanguageAccessor,
        IMarketService marketService)
    {
        private ShippingMethodInfoModel _instorePickup;

        public virtual ShippingMethodInfoModel InStorePickupInfoModel => _instorePickup ?? (_instorePickup = shippingService.GetInstorePickupModel());

        public virtual IEnumerable<ShipmentViewModel> CreateShipmentsViewModel(ICart cart)
        {
            var preferredCulture = contentLanguageAccessor.Language;
            foreach (var shipment in cart.GetFirstForm().Shipments)
            {
                var shipmentModel = new ShipmentViewModel
                {
                    ShipmentId = shipment.ShipmentId,
                    CartItems = new List<CartItemViewModel>(),
                    Address = addressBookService.ConvertToModel(shipment.ShippingAddress),
                    ShippingMethods = CreateShippingMethodViewModels(cart.MarketId, cart.Currency, shipment)
                };

                var currentShippingMethod = shipmentModel.ShippingMethods.FirstOrDefault();
                if (shipment.ShippingMethodId != Guid.Empty)
                {
                    currentShippingMethod = shipmentModel.ShippingMethods.FirstOrDefault(x => x.Id == shipment.ShippingMethodId);
                }
                else
                {
                    currentShippingMethod = shipmentModel.ShippingMethods.FirstOrDefault();
                }

                shipmentModel.ShippingMethodId = currentShippingMethod?.Id ?? shipment.ShippingMethodId;
                shipmentModel.CurrentShippingMethodName = currentShippingMethod?.DisplayName ?? "In store pickup";
                shipmentModel.CurrentShippingMethodPrice = currentShippingMethod?.Price ?? new Money(0, cart.Currency);
                shipmentModel.WarehouseCode = shipment.WarehouseCode;

                var entries = contentLoader.GetItems(shipment.LineItems.Select(x => referenceConverter.GetContentLink(x.Code)),
                    preferredCulture).OfType<EntryContentBase>();

                foreach (var lineItem in shipment.LineItems)
                {
                    var entry = entries.FirstOrDefault(x => x.Code == lineItem.Code);
                    if (entry == null)
                    {
                        //Entry was deleted, skip processing.
                        continue;
                    }

                    shipmentModel.CartItems.Add(cartItemViewModelFactory.CreateCartItemViewModel(cart, lineItem, entry));
                }

                yield return shipmentModel;
            }
        }

        private IEnumerable<ShippingMethodViewModel> CreateShippingMethodViewModels(MarketId marketId, Currency currency, IShipment shipment)
        {
            var shippingRates = GetShippingRates(marketId, currency, shipment);

            shippingRates = shippingRates.Where(o => o.Money > 0);
            

            var models = shippingRates.Select(r => new ShippingMethodViewModel { Id = r.Id, DisplayName = r.Name, Price = r.Money })
                .ToList();

            if (shipment.ShippingMethodId == InStorePickupInfoModel.MethodId)
            {
                models.Insert(0, new ShippingMethodViewModel
                {
                    Id = InStorePickupInfoModel.MethodId,
                    DisplayName = $"In store pickup - ({shipment.ShippingAddress.Line1} , {shipment.ShippingAddress.City} , {shipment.ShippingAddress.RegionName})",
                    Price = new Money(0m, currency),
                    IsInstorePickup = true
                });
            }

            return models;
        }

        public IEnumerable<ShippingRate> GetShippingRates(MarketId marketId, Currency currency, IShipment shipment)
        {
            var methods = shippingService.GetShippingMethodsByMarket(marketId.Value, false)
                .Where(x => x.MethodId != InStorePickupInfoModel.MethodId);
            var currentLanguage = languageService.GetCurrentLanguage().TwoLetterISOLanguageName;

            return methods.Where(shippingMethodRow => currentLanguage.Equals(shippingMethodRow.LanguageId, StringComparison.OrdinalIgnoreCase)
                && string.Equals(currency, shippingMethodRow.Currency, StringComparison.OrdinalIgnoreCase))
                .OrderBy(shippingMethodRow => shippingMethodRow.Ordering)
                .Select(shippingMethodRow => shippingService.GetRate(shipment, shippingMethodRow, marketService.GetMarket(marketId)));
        }
    }
}
