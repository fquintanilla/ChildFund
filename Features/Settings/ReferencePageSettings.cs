using System.ComponentModel.DataAnnotations;
using ChildFund.Features.CatalogContent.Variant;
using ChildFund.Features.Checkout;
using ChildFund.Features.MyAccount.OrderConfirmation;
using ChildFund.Features.NamedCarts.DefaultCart;
using ChildFund.Infrastructure;
using ChildFund.Infrastructure.Cms.Settings;

namespace ChildFund.Features.Settings
{
    [SettingsContentType(DisplayName = "Site Structure Settings Page",
        GUID = "bf69f959-c91b-46cb-9829-2ecf9d11e13b",
        Description = "Site structure settings",
        SettingsName = "Page references")]
    public class ReferencePageSettings : SettingsBase
    {
        #region Site Structure

        [CultureSpecific]
        //[AllowedTypes(typeof(SearchResultPage))]
        [Display(Name = "Search page", GroupName = TabNames.SiteStructure, Order = 10)]
        public virtual ContentReference SearchPage { get; set; }

        [CultureSpecific]
        [AllowedTypes(typeof(CheckoutPage))]
        [Display(Name = "Checkout page", GroupName = TabNames.SiteStructure, Order = 20)]
        public virtual ContentReference CheckoutPage { get; set; }

        [CultureSpecific]
        [AllowedTypes(typeof(CartPage))]
        [Display(Name = "Shopping cart page", GroupName = TabNames.SiteStructure, Order = 30)]
        public virtual ContentReference CartPage { get; set; }

        [CultureSpecific]
        [AllowedTypes(typeof(OrderConfirmationPage))]
        [Display(Name = "Order confirmation page", GroupName = TabNames.SiteStructure, Order = 40)]
        public virtual ContentReference OrderConfirmationPage { get; set; }

        [CultureSpecific]
        [AllowedTypes(typeof(SponsorshipVariant))]
        [Display(Name = "Sponsorship Variant", GroupName = TabNames.SiteStructure, Order = 50)]
        public virtual ContentReference SponsorshipVariant { get; set; }

        #endregion

        #region Mail templates

        [CultureSpecific]
        [Display(Name = "Send order confirmations", GroupName = TabNames.MailTemplates, Order = 10)]
        public virtual bool SendOrderConfirmationMail { get; set; }

        [CultureSpecific]
        //[AllowedTypes(typeof(OrderConfirmationMailPage))]
        [Display(Name = "Order confirmation", GroupName = TabNames.MailTemplates, Order = 20)]
        public virtual ContentReference OrderConfirmationMail { get; set; }

       

        #endregion
    }
}
