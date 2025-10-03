using EPiServer.Commerce.Catalog.ContentTypes;
using Mediachase.Commerce;

namespace ChildFund.Features.Checkout.ViewModels
{
    public class CartItemViewModel
    {
        public int ProductId { get; set; }

        public string DisplayName { get; set; }

        public string ImageUrl { get; set; }

        public string Url { get; set; }
        
        public Money? DiscountedPrice { get; set; }

        public Money BasePrice { get; set; }

        public Money OptionPrice { get; set; }

        public Money PlacedPrice { get; set; }

        public string Code { get; set; }

        public EntryContentBase Entry { get; set; }

        public decimal Quantity { get; set; }

        public Money? DiscountedUnitPrice { get; set; }
        
        public bool IsAvailable { get; set; }

        public string AddressId { get; set; }

        public string Description { get; set; }
    }
}
