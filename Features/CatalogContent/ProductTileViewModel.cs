using Mediachase.Commerce;


namespace ChildFund.Features.CatalogContent
{
    public class ProductTileViewModel : IProductModel
    {
        public int ProductId { get; set; }
        public string DisplayName { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public Money? DiscountedPrice { get; set; }
        public Money PlacedPrice { get; set; }
        public string Code { get; set; }
        public bool IsAvailable { get; set; }
        public string FirstVariationCode { get; set; }
        public Type EntryType { get; set; }
        public DateTime Created { get; set; }
    }
}
