using ChildFund.Features.Upsell.Models;

namespace ChildFund.Features.Upsell.Services
{
    public interface IUpsellCandidateRepository
    {
        // Resolve the union of candidates from variants, categories, and tag filters
        IEnumerable<VariantView> ResolveCandidates(
            IReadOnlyList<ContentReference> variantLinks,
            IReadOnlyList<ContentReference> categoryLinks,
            IReadOnlyList<string> tagFilters,
            string marketId,
            string language);
    }
}
