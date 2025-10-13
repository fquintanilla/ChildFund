using ChildFund.Web.Core.Settings;
using ChildFund.Web.Features.Checkout.ViewModels;
using ChildFund.Web.Features.Header;
using ChildFund.Web.Features.MyAccount.AddressBook;
using ChildFund.Web.Features.NamedCarts.DefaultCart;
using ChildFund.Web.Infrastructure.Cms.Settings;
using ChildFund.Web.Infrastructure.Commerce.Markets;
using EPiServer.Security;
using Mediachase.Commerce.Security;
using ReferenceConverter = Mediachase.Commerce.Catalog.ReferenceConverter;

namespace ChildFund.Web.Features.Checkout.Services
{
    public class CartViewModelFactory(
        IContentLoader contentLoader,
        ICurrencyService currencyService,
        IOrderGroupCalculator orderGroupCalculator,
        UrlResolver urlResolver,
        IHttpContextAccessor httpContextAccessor,
        IAddressBookService addressBookService,
        ISettingsService settingsService,
        ShipmentViewModelFactory shipmentViewModelFactory,
        ReferenceConverter referenceConverter)
    {
        public virtual MiniCartViewModel CreateMiniCartViewModel(ICart cart, bool isSharedCart = false)
        {
            var labelSettings = settingsService.GetSiteSettings<LabelSettings>();
            var pageSettings = settingsService.GetSiteSettings<ReferencePageSettings>();
            if (cart == null)
            {
                return new MiniCartViewModel
                {
                    ItemCount = 0,
                    CheckoutPage = pageSettings?.CheckoutPage,
                    CartPage = pageSettings?.CartPage,
                    Label = labelSettings?.CartLabel,
                    Shipments = Enumerable.Empty<ShipmentViewModel>(),
                    Total = new Money(0, currencyService.GetCurrentCurrency()),
                    IsSharedCart = isSharedCart
                };
            }

            return new MiniCartViewModel
            {
                ItemCount = GetLineItemsTotalQuantity(cart),
                CheckoutPage = pageSettings?.CheckoutPage,
                CartPage = pageSettings?.CartPage,
                Label = labelSettings?.CartLabel,
                Shipments = shipmentViewModelFactory.CreateShipmentsViewModel(cart),
                Total = orderGroupCalculator.GetSubTotal(cart),
                IsSharedCart = isSharedCart
            };
        }

        public virtual LargeCartViewModel CreateLargeCartViewModel(ICart cart, CartPage cartPage)
        {
            var pageSettings = settingsService.GetSiteSettings<ReferencePageSettings>();
            var contact = PrincipalInfo.CurrentPrincipal.GetCustomerContact();
            AddressModel addressModel;
            if (cart == null)
            {
                var zeroAmount = new Money(0, currencyService.GetCurrentCurrency());
                addressModel = new AddressModel();
                addressBookService.LoadCountriesAndRegionsForAddress(addressModel);
                return new LargeCartViewModel(cartPage)
                {
                    Shipments = Enumerable.Empty<ShipmentViewModel>(),
                    TotalDiscount = zeroAmount,
                    Total = zeroAmount,
                    TaxTotal = zeroAmount,
                    ShippingTotal = zeroAmount,
                    Subtotal = zeroAmount,
                    ReferrerUrl = GetReferrerUrl(),
                    CheckoutPage = pageSettings?.CheckoutPage,
                    //MultiShipmentPage = checkoutPage.MultiShipmentPage,
                    AppliedCouponCodes = Enumerable.Empty<string>(),
                    AddressModel = addressModel,
                    ShowRecommendations = true
                };
            }

            var totals = orderGroupCalculator.GetOrderGroupTotals(cart);
            var orderDiscountTotal = orderGroupCalculator.GetOrderDiscountTotal(cart);
            var shippingDiscountTotal = cart.GetShippingDiscountTotal();
            var discountTotal = shippingDiscountTotal + orderDiscountTotal;

            var model = new LargeCartViewModel(cartPage)
            {
                Shipments = shipmentViewModelFactory.CreateShipmentsViewModel(cart),
                TotalDiscount = discountTotal,
                Total = totals.Total,
                ShippingTotal = totals.ShippingTotal,
                Subtotal = totals.SubTotal,
                TaxTotal = totals.TaxTotal,
                ReferrerUrl = GetReferrerUrl(),
                CheckoutPage = pageSettings?.CheckoutPage,
                //MultiShipmentPage = checkoutPage.MultiShipmentPage,
                AppliedCouponCodes = cart.GetFirstForm().CouponCodes.Distinct(),
                HasOrganization = contact?.OwnerId != null
            };

            var shipment = model.Shipments.FirstOrDefault();
            addressModel = shipment?.Address ?? new AddressModel();
            addressBookService.LoadCountriesAndRegionsForAddress(addressModel);
            model.AddressModel = addressModel;

            return model;
        }

        private decimal GetLineItemsTotalQuantity(ICart cart)
        {
            if (cart != null)
            {
                var cartItems = cart
                .GetAllLineItems()
                .Where(c => !ContentReference.IsNullOrEmpty(referenceConverter.GetContentLink(c.Code)));
                return cartItems.Sum(x => x.Quantity);
            }
            else
            {
                return 0;
            }
        }

        private string GetReferrerUrl()
        {
            var httpContext = httpContextAccessor.HttpContext;
            var urlReferer = httpContext.Request.Headers["UrlReferrer"].ToString();
            var hostUrlReferer = string.IsNullOrEmpty(urlReferer) ? "" : new Uri(urlReferer).Host;
            if (urlReferer != null && hostUrlReferer.Equals(httpContext.Request.Host.Host.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return urlReferer;
            }

            return urlResolver.GetUrl(ContentReference.StartPage);
        }
    }
}
