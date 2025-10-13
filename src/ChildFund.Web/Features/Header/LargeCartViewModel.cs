using ChildFund.Web.Features.Checkout.ViewModels;
using ChildFund.Web.Features.MyAccount.AddressBook;
using ChildFund.Web.Features.NamedCarts.DefaultCart;
using ChildFund.Web.Features.Shared.ViewModels;

namespace ChildFund.Web.Features.Header
{
    public class LargeCartViewModel : ContentViewModel<CartPage>
    {
        public LargeCartViewModel()
        {
        }

        public LargeCartViewModel(CartPage cartPage) : base(cartPage)
        {
        }

        public string ReferrerUrl { get; set; }

        public IEnumerable<ShipmentViewModel> Shipments { get; set; }

        public Money TotalDiscount { get; set; }

        public Money Total { get; set; }

        public Money Subtotal { get; set; }

        public Money ShippingTotal { get; set; }

        public Money TaxTotal { get; set; }

        public ContentReference CheckoutPage { get; set; }

        public ContentReference MultiShipmentPage { get; set; }

        public AddressModel AddressModel { get; set; }

        public IEnumerable<string> AppliedCouponCodes { get; set; }

        public bool HasOrganization { get; set; }

        public string Message { get; set; }

        public bool ShowRecommendations { get; set; }
    }
}
