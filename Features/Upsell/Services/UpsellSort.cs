using ChildFund.Features.Upsell.Models;

namespace ChildFund.Features.Upsell.Services
{
    public static class UpsellSort
    {
        private static readonly ThreadLocal<Random> Rng = new(() => new Random());

        public static IEnumerable<VariantView> Apply(string mode, IEnumerable<VariantView> variants)
        {
            switch (mode)
            {
                case "PriceAsc": return variants.OrderBy(v => v.Price ?? decimal.MaxValue);
                case "PriceDesc": return variants.OrderByDescending(v => v.Price ?? decimal.Zero);
                case "RandomWithinTopN": return variants.OrderBy(_ => Rng.Value.Next());
                case "SequenceAsc":
                default: return variants.OrderBy(v => v.UpsellSequence).ThenBy(v => v.Code, StringComparer.OrdinalIgnoreCase);
            }
        }
    }
}
