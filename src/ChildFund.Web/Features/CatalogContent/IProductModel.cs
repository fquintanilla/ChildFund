namespace ChildFund.Web.Features.CatalogContent
{
    public interface IProductModel
    {
        int ProductId { get; set; }
        string Code { get; set; }
        string DisplayName { get; set; }
        string Description { get; set; }
        Money? DiscountedPrice { get; set; }
        string ImageUrl { get; set; }
        Money PlacedPrice { get; set; }
        string Url { get; set; }
        bool IsAvailable { get; set; }
        string FirstVariationCode { get; set; }
        Type EntryType { get; set; }
    }
}
