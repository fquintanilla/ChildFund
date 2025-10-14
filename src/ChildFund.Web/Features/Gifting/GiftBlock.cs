namespace ChildFund.Web.Features.Gifting
{
    [ContentType(
        DisplayName = "Give A Gift Block",
        GUID = "F7C8520E-3B6D-4A8A-93C7-85A0E7D2B8E9",
        Description = "Presents the Give a Gift UI and hooks into cart")]
    public class GiftBlock : BlockData
    {
        // One or more catalog variants shown in the Occasion dropdown
        [Display(
            Name = "Occasion Variants",
            Description = "Choose the variant(s) to offer in the Occasion dropdown",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        [AllowedTypes(typeof(VariationContent))]
        public virtual ContentArea Variants { get; set; }

        // Single-select Payment Frequency via a selection factory
        [Display(
            Name = "Payment Frequency",
            Description = "Limit the UI to this payment frequency",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        [SelectOne(SelectionFactoryType = typeof(PaymentFrequencySelectionFactory))]
        public virtual string PaymentFrequency { get; set; }
    }
}
