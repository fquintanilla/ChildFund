using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using EPiServer.SpecializedProperties;
using System.ComponentModel.DataAnnotations;
using ChildFund.Infrastructure.Cms;
using EPiServer.Web;

namespace ChildFund.Features.Commerce.Variant
{
    [CatalogContentType(DisplayName = "Generic Variant", GUID = "cfcf83dc-26b4-406b-a4f1-bfb0c9788c67", Description = "Generic Variant Page type to display products.")]
    public class GenericVariant : VariationContent
    {
        [CultureSpecific]
        [Display(Name = "Main body",
            Description = "The main body will be shown in the main content area of the page, using the XHTML-editor you can insert for example text, images and tables.",
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual XhtmlString MainBody { get; set; }

        //[CultureSpecific]
        //[AllowedTypes(typeof(SocialSharingBlock))]
        //[Display(Name = "Social Sharing", GroupName = SystemTabNames.Content, Order = 10)]
        //public virtual ContentReference SocialSharingReference { get; set; }

        [Searchable(false)]
        [Display(Name = "Page Title Text Color", GroupName = SystemTabNames.Content, Order = 20)]
        [ClientEditor(ClientEditingClass = "foundation/editors/ColorPicker")]
        public virtual string PageTitleTextColor
        {
            get { return this.GetPropertyValue(page => page.PageTitleTextColor) ?? Constants.ColorPicker.PageTitleTextColorDefault; }
            set { this.SetPropertyValue(page => page.PageTitleTextColor, value); }
        }

        [Searchable(false)]
        [Display(Name = "Page Title Text Background Color",
            GroupName = SystemTabNames.Content,
            Order = 30)]
        [ClientEditor(ClientEditingClass = "foundation/editors/ColorPicker")]
        public virtual string PageTitleTextBackgroundColor
        {
            get { return this.GetPropertyValue(page => page.PageTitleTextBackgroundColor) ?? Constants.ColorPicker.PageTitleTextBackgroundColorDefault; }
            set { this.SetPropertyValue(page => page.PageTitleTextBackgroundColor, value); }
        }

        [Display(Name = "Hide site header", GroupName = Infrastructure.TabNames.Settings, Order = 100)]
        public virtual bool HideSiteHeader { get; set; }

        [Display(Name = "Hide site footer", GroupName = Infrastructure.TabNames.Settings, Order = 200)]
        public virtual bool HideSiteFooter { get; set; }

        [Display(Name = "CSS files", GroupName = Infrastructure.TabNames.Styles, Order = 100)]
        public virtual LinkItemCollection CssFiles { get; set; }

        [Searchable(false)]
        [Display(Name = "CSS", GroupName = Infrastructure.TabNames.Styles, Order = 200)]
        [UIHint(UIHint.Textarea)]
        public virtual string Css { get; set; }

        [UIHint(UIHint.Image)]
        [Display(Name = "Image", GroupName = SystemTabNames.Content, Order = 30)]
        public virtual ContentReference ImageReference { get; set; }

        [Searchable(false)]
        [Display(Name = "Marketing Id", GroupName = Infrastructure.TabNames.Settings, Order = 10)]
        public virtual string MarketingId { get; set; }

        [Searchable(false)]
        [Display(Name = "Financial Code", GroupName = Infrastructure.TabNames.Settings, Order = 20)]
        public virtual string FinancialCode { get; set; }

        [Display(Name = "Exclude from results",
            Description = "This will determine whether or not to show on search",
            GroupName = Infrastructure.TabNames.Settings,
            Order = 40)]
        public virtual bool ExcludeFromSearch { get; set; }

        //[ScaffoldColumn(false)]
        //public virtual string Description { get => MainBody.ToStringValue(); }
    }
}
