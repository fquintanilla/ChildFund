namespace ChildFund.Web.Features.Sponsorship
{
    [ContentType(
        DisplayName = "Sponsor a Child",
        GUID = "40a8ce2c-4a9e-4a3c-9f4e-7b0c59b1f6eb",
        Description = "Shows a carousel of children and an Add to Cart button.")]
    public class SponsorChildBlock : BlockData
    {
        [Display(
            Name = "Heading",
            Order = 5)]
        public virtual string? Heading { get; set; }

        [Display(
            Name = "How many children",
            Order = 10,
            Description = "How many items to fetch and display")]
        [Range(1, 12)]
        public virtual int? Count { get; set; }
    }
}
