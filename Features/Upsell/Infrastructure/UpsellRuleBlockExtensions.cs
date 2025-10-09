using ChildFund.Features.Upsell.Models;

namespace ChildFund.Features.Upsell.Infrastructure
{
    public static class UpsellRuleBlockExtensions
    {
        public static UpsellRule ToUpsellRule(this UpsellRuleBlock block)
        {
            return new UpsellRule
            {
                RuleName = block.RuleName,
                Enabled = block.Enabled,
                ConditionsJson = block.ConditionsJson,
                CandidateVariantLinks = block.CandidateVariants?.ToList() ?? [],
                CandidateCategoryLinks = block.CandidateCategories?.ToList() ?? [],
                CandidateTagFilters = block.CandidateTagFilters?.ToList() ?? [],
                MaxSuggestions = block.MaxSuggestions,
                SortMode = string.IsNullOrWhiteSpace(block.SortMode) ? "SequenceAsc" : block.SortMode,
                SuppressIfSkusInCart = block.SuppressIfSkusInCart?.ToList() ?? []
            };
        }
    }
}
