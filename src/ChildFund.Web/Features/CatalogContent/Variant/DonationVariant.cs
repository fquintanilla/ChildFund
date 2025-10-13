using EPiServer.Commerce.Catalog.DataAnnotations;

namespace ChildFund.Web.Features.CatalogContent.Variant
{
    [CatalogContentType(
        DisplayName = "Donation Variant",
        GUID = "{24AC3A2F-1657-405C-8931-E2D28F750A4D}",
        Description = "")]
    public class DonationVariant : GenericVariant
    {
        [CultureSpecific]
        [Display(Name = "Amounts", Order = 20)]
        [BackingType(typeof(PropertyIntegerList))]
        public virtual IList<int> Amounts { get; set; }
    }
}
