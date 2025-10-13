namespace ChildFund.Web.Features.Upsell.Models
{
    public class VariantView
    {
        public ContentReference ContentLink { get; init; }
        public string Name { get; init; }
        public string Code { get; init; }
        public decimal? Price { get; init; }
        public int UpsellSequence { get; init; }
        public ISet<string> Tags { get; init; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    }
}
