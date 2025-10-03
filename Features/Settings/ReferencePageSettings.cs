using System.ComponentModel.DataAnnotations;
using ChildFund.Features.Checkout;
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
        [Display(Name = "Checkout page", GroupName = TabNames.SiteStructure, Order = 170)]
        public virtual ContentReference CheckoutPage { get; set; }

        [CultureSpecific]
        [AllowedTypes(typeof(CartPage))]
        [Display(Name = "Shopping cart page", GroupName = TabNames.SiteStructure, Order = 60)]
        public virtual ContentReference CartPage { get; set; }

        #endregion

        #region Mail templates

        [CultureSpecific]
        [Display(Name = "Send order confirmations", GroupName = TabNames.MailTemplates, Order = 10)]
        public virtual bool SendOrderConfirmationMail { get; set; }

        #endregion
    }
}
