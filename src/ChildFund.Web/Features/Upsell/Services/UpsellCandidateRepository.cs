using ChildFund.Web.Features.CatalogContent.Variant;
using ChildFund.Web.Features.Upsell.Models;
using ChildFund.Web.Infrastructure.Commerce.Extensions;

namespace ChildFund.Web.Features.Upsell.Services
{
    public class UpsellCandidateRepository(IContentLoader contentLoader)
        : IUpsellCandidateRepository
    {

        public IEnumerable<VariantView> ResolveCandidates(
            IReadOnlyList<ContentReference>? variantLinks,
            IReadOnlyList<ContentReference>? categoryLinks,
            IReadOnlyList<string>? tagFilters,
            string marketId,
            string language)
        {
            var results = new List<VariantView>();
            var processed = new HashSet<ContentReference>();

            // 1️. Explicit variant links
            if (variantLinks != null)
            {
                foreach (var link in variantLinks)
                {
                    if (processed.Contains(link)) continue;
                    var variant = LoadVariant(link);
                    if (variant != null)
                    {
                        results.Add(Map(variant));
                        processed.Add(link);
                    }
                }
            }

            // 2️. Variants under selected categories
            if (categoryLinks != null)
            {
                foreach (var catRef in categoryLinks)
                {
                    foreach (var child in catRef.GetAllVariants<GenericVariant>())
                    {
                        if (processed.Contains(child.ContentLink)) continue;
                        results.Add(Map(child));
                        processed.Add(child.ContentLink);
                    }
                }
            }

            // 3️. Filter by tags (optional)
            if (tagFilters != null && tagFilters.Count > 0)
            {
                results = results
                    .Where(v => v.Tags.Overlaps(tagFilters))
                    .ToList();
            }

            // 4️.  Market & Language filtering (optional — add if required)
            results = FilterByMarketAndLanguage(results, marketId, language).ToList();

            return results;
        }

        private GenericVariant? LoadVariant(ContentReference contentLink)
        {
            if (!contentLoader.TryGet(contentLink, out GenericVariant variant))
                return null;
            return variant.IsUpsell ? variant : null;
        }

        private static VariantView Map(GenericVariant variant)
        {
            return new VariantView
            {
                ContentLink = variant.ContentLink,
                Name = variant.Name,
                Code = variant.Code,
                Price = variant.DefaultPrice(),
                UpsellSequence = variant.UpsellSequence,
                Tags = variant.UpsellTags != null
                    ? new HashSet<string>(variant.UpsellTags, StringComparer.OrdinalIgnoreCase)
                    : new HashSet<string>()
            };
        }

        private static IEnumerable<VariantView> FilterByMarketAndLanguage(IEnumerable<VariantView> variants, string marketId, string language)
        {
            // Extend later if variants need market/language filtering.
            return variants;
        }
    }
}
