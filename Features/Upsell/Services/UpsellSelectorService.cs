using ChildFund.Features.Upsell.Dsl;
using ChildFund.Features.Upsell.Models;
using EPiServer.Commerce.Catalog.ContentTypes;

namespace ChildFund.Features.Upsell.Services
{
    public class UpsellSelectorService(IDslEvaluator dsl, 
        IUpsellCandidateRepository repo,
        IContentLoader contentLoader)
        : IUpsellSelectorService
    {
        public IEnumerable<VariantView> GetUpsells(
            IEnumerable<UpsellRule> rulesInPriorityOrder,
            CartContext ctx,
            int max)
        {
            if (max <= 0) yield break;

            var emitted = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var sku in ctx.CartSkus) emitted.Add(sku); // avoid suggesting items already in cart

            var collected = 0;

            foreach (var rule in rulesInPriorityOrder.Where(r => r.Enabled))
            {
                if (!dsl.Evaluate(rule.ConditionsJson, ctx))
                    continue;

                var candidates = repo.ResolveCandidates(
                    rule.CandidateVariantLinks,
                    rule.CandidateCategoryLinks,
                    rule.CandidateTagFilters,
                    ctx.MarketId,
                    ctx.Language);

                // Suppress if cart already contains any blocked SKU
                var suppressed = new HashSet<ContentReference>(rule.SuppressIfSkusInCart ?? []);

                var suppressedCodes = suppressed
                    .Select(x => contentLoader.Get<EntryContentBase>(x).Code)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                var filtered = candidates
                    .Where(v => !emitted.Contains(v.Code)) // skip already emitted
                    .Where(v => !(emitted.Overlaps(suppressedCodes)));

                foreach (var v in UpsellSort.Apply(rule.SortMode, filtered).Take(rule.MaxSuggestions))
                {
                    yield return v;
                    emitted.Add(v.Code);
                    collected++;
                    if (collected >= max) yield break;
                }
            }

            // Optional global fallback: ask repo for a global pool (e.g., by tags) if needed.
            // Example: a repo method ResolveGlobalFallback(ctx) that returns IsUpsell=true variants.
            // foreach (var v in UpsellSort.Apply("SequenceAsc", _repo.ResolveGlobalFallback(ctx)))
            // {
            //     if (!emitted.Contains(v.Code))
            //     {
            //         yield return v;
            //         if (++collected >= max) yield break;
            //     }
            // }
        }
    }
}
