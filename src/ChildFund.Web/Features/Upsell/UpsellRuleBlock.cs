using ChildFund.Web.Features.Upsell.Infrastructure;

namespace ChildFund.Web.Features.Upsell
{
    [ContentType(
        GUID = "B75C1C39-32FB-4D3B-AC4B-9BE02D1C7C21",
        DisplayName = "Upsell Rule Block",
        Description = "Defines a set of conditions and candidate products for upsell logic.")]
    public class UpsellRuleBlock : BlockData
    {
        [Display(
            Name = "Rule Priority",
            Order = 10,
            GroupName = "General",
            Description = "Defines evaluation order of rules. Lower number = higher priority.")]
        public virtual int RulePriority { get; set; }

        [Display(
            Name = "Enabled",
            Order = 20,
            GroupName = "General",
            Description = "Enable or disable this rule.")]
        public virtual bool Enabled { get; set; }

        [Display(
            Name = "Rule Name",
            Order = 30,
            GroupName = "General",
            Description = "Friendly name for identifying this upsell rule.")]
        public virtual string RuleName { get; set; }

        [Display(
            Name = "Conditions (JSON)",
            Order = 40,
            GroupName = "Conditions",
            Description = "Serialized JSON containing DSL conditions (e.g., cart total, categories, SKUs).")]
        [UIHint(UIHint.Textarea)]
        public virtual string ConditionsJson { get; set; }

        [Display(
            Name = "Candidate Variants",
            Order = 50,
            GroupName = "Candidates",
            Description = "Specific product variants to recommend when this rule matches.")]
        [AllowedTypes(typeof(VariationContent), typeof(ProductContent))]
        public virtual IList<ContentReference>? CandidateVariants { get; set; }

        [Display(
            Name = "Candidate Categories",
            Order = 60,
            GroupName = "Candidates",
            Description = "Select categories; upsell items will be chosen from these categories.")]
        [AllowedTypes(typeof(NodeContent))]
        public virtual IList<ContentReference>? CandidateCategories { get; set; }

        [Display(
            Name = "Candidate Tag Filters",
            Order = 70,
            GroupName = "Candidates",
            Description = "Filter candidates by upsell tags (e.g., 'emergency', 'premium').")]
        public virtual IList<string>? CandidateTagFilters { get; set; }

        [Display(
            Name = "Max Suggestions",
            Order = 80,
            GroupName = "Output",
            Description = "Maximum number of upsell products to suggest from this rule.")]
        public virtual int MaxSuggestions { get; set; }

        [Display(
            Name = "Sort Mode",
            Order = 90,
            GroupName = "Output",
            Description = "How to sort candidates (SequenceAsc, PriceAsc, RandomWithinTopN).")]
        [SelectOne(SelectionFactoryType = typeof(SortModeSelectionFactory))]
        public virtual string SortMode { get; set; }

        [Display(
            Name = "Suppress If SKUs In Cart",
            Order = 100,
            GroupName = "Conditions",
            Description = "Do not show these upsell products if the cart already contains any of these SKUs.")]
        [AllowedTypes(typeof(VariationContent), typeof(ProductContent))]
        public virtual IList<ContentReference>? SuppressIfSkusInCart { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            SortMode = "SequenceAsc";
            MaxSuggestions = 1;
        }
    }
}
