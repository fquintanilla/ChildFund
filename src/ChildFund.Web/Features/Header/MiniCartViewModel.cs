using ChildFund.Web.Features.Checkout.ViewModels;

namespace ChildFund.Web.Features.Header
{
    public class MiniCartViewModel
    {
        public MiniCartViewModel()
        {
            Shipments = new List<ShipmentViewModel>();
        }

        public ContentReference CheckoutPage { get; set; }

        public ContentReference CartPage { get; set; }

        public decimal ItemCount { get; set; }

        public IEnumerable<ShipmentViewModel> Shipments { get; set; }

        public Money Total { get; set; }

        public string Label { get; set; }

        public bool IsSharedCart { get; set; }
    }
}
