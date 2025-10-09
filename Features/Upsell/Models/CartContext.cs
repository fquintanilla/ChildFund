namespace ChildFund.Features.Upsell.Models
{
    public class CartContext
    {
        public decimal CartTotal { get; init; }
        public ISet<string> CartSkus { get; init; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        public DateTime UtcNow { get; init; } = DateTime.UtcNow;
        public ISet<string> CustomerSegments { get; init; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        public string MarketId { get; init; }
        public string Language { get; init; }
    }
}
