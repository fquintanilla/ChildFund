using ChildFund.Features.Checkout.ViewModels;
using ChildFund.Features.Header;
using ChildFund.Features.MyAccount.AddressBook;
using ChildFund.Features.Settings;
using ChildFund.Infrastructure.Cms.Settings;
using ChildFund.Infrastructure.Commerce.Markets;
using EPiServer.Commerce.Order;
using EPiServer.Web.Routing;
using Mediachase.Commerce;
using ReferenceConverter = Mediachase.Commerce.Catalog.ReferenceConverter;

namespace ChildFund.Features.Checkout.Services
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
