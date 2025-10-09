namespace ChildFund.Features.Upsell.Models
{
    public class UpsellRule
    {
        public string RuleName { get; init; }
        public bool Enabled { get; init; }
        public string ConditionsJson { get; init; } // the DSL
        public IReadOnlyList<ContentReference> CandidateVariantLinks { get; init; } = [];
        public IReadOnlyList<ContentReference> CandidateCategoryLinks { get; init; } = [];
        public IReadOnlyList<string> CandidateTagFilters { get; init; } = [];
        public int MaxSuggestions { get; init; } = 1;
        public string SortMode { get; init; } = "SequenceAsc"; // SequenceAsc | PriceAsc | PriceDesc | RandomWithinTopN
        public IReadOnlyList<ContentReference>? SuppressIfSkusInCart { get; init; } = [];
    }
}
