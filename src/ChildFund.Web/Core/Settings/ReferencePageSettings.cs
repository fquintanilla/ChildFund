using Advanced.CMS.GroupingHeader;
using ChildFund.Web.Features.CatalogContent.Variant;
using ChildFund.Web.Features.NamedCarts.DefaultCart;
using ChildFund.Web.Features.Shared.Pages;
using ChildFund.Web.Infrastructure.Cms.Attributes;
using ChildFund.Web.Infrastructure.Cms.Constants;
using ChildFund.Web.Infrastructure.Cms.Settings;

namespace ChildFund.Web.Core.Settings;

[SettingsContentType(DisplayName = "Site Structure Settings Page",
    GUID = "bf69f959-c91b-46cb-9829-2ecf9d11e13b",
    Description = "Site structure settings",
    SettingsName = "Page references")]
public class ReferencePageSettings : SettingsBase
{
    #region References
    [RequiredForPublish]
    //[AllowedTypes(typeof(SearchResultsPage))]
    [Display(Name = "Search page", GroupName = TabNames.SiteStructure, Order = 100)]
    [Searchable(false)]
    public virtual ContentReference SearchPage { get; set; }

    [RequiredForPublish]
    [AllowedTypes(typeof(FoundationPageData))]
    [Display(Name = "Not Found Page", GroupName = SystemTabNames.Content, Order = 300)]
    [Searchable(false)]
    public virtual ContentReference Error404 { get; set; }

    [RequiredForPublish]
    [AllowedTypes(typeof(FoundationPageData))]
    [Display(Name = "Server Error Page", GroupName = SystemTabNames.Content, Order = 400)]
    [Searchable(false)]
    public virtual ContentReference Error500 { get; set; }

    [GroupingHeader("Commerce Pages")]

    [RequiredForPublish]
    [Display(Name = "Login Page", GroupName = GroupNames.Commerce, Order = 1)]
    [Searchable(false)]
    public virtual ContentReference LoginPage { get; set; }

    [RequiredForPublish]
    [Display(Name = "Reset Password Page", GroupName = GroupNames.Commerce, Order = 2)]
    [Searchable(false)]
    public virtual ContentReference ResetPasswordPage { get; set; }

    [RequiredForPublish]
    [Display(Name = "Create an Account Page", GroupName = GroupNames.Commerce, Order = 5)]
    [Searchable(false)]
    public virtual ContentReference CreateAnAccountPage { get; set; }

    [RequiredForPublish]
    [Display(Name = "Checkout Page", GroupName = GroupNames.Commerce, Order = 10)]
    [Searchable(false)]
    public virtual ContentReference CheckoutPage { get; set; }

    [RequiredForPublish]
    [Display(Name = "Order Confirmation Page", GroupName = GroupNames.Commerce, Order = 15)]
    [Searchable(false)]
    public virtual ContentReference OrderConfirmationPage { get; set; }

    [CultureSpecific]
    [AllowedTypes(typeof(CartPage))]
    [Display(Name = "Basket page", GroupName = TabNames.SiteStructure, Order = 60)]
    public virtual ContentReference CartPage { get; set; }

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