using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.DataAnnotations;
using EPiServer.SpecializedProperties;
using System.ComponentModel.DataAnnotations;
using ChildFund.Infrastructure.Cms;
using EPiServer.Web;

namespace ChildFund.Features.CatalogContent.Category
{
    [CatalogContentType(DisplayName = "Default Category",
        GUID = "57ffab7b-e0ec-40dc-8cc7-3011336891a3",
        Description = "")]
    public class DefaultCategory : NodeContent
    {
        //[CultureSpecific]
        //[Display(
        //    Name = "Main body",
        //    Description = "The main body will be shown in the main content area of the page, using the XHTML-editor you can insert for example text, images and tables.",
        //    GroupName = SystemTabNames.Content,
        //    Order = 1)]
        //public virtual XhtmlString MainBody { get; set; }
        //[CultureSpecific]
        //[AllowedTypes(new[] { typeof(BreadcrumbBlock) })]
        //[Display(Name = "Breadcrumb", GroupName = SystemTabNames.Content, Order = 10)]
        //public virtual ContentReference BreadcrumbReference { get; set; }

        [Searchable(false)]
        [Display(Name = "Page Title Text Color", GroupName = SystemTabNames.Content, Order = 20)]
        //[ClientEditor(ClientEditingClass = "foundation/editors/ColorPicker")]
        public virtual string PageTitleTextColor
        {
            get { return this.GetPropertyValue(page => page.PageTitleTextColor) ?? Constant.ColorPicker.PageTitleTextColorDefault; }
            set { this.SetPropertyValue(page => page.PageTitleTextColor, value); }
        }

        [Searchable(false)]
        [Display(Name = "Page Title Text Background Color", GroupName = SystemTabNames.Content, Order = 30)]
        //[ClientEditor(ClientEditingClass = "foundation/editors/ColorPicker")]
        public virtual string PageTitleTextBackgroundColor
        {
            get { return this.GetPropertyValue(page => page.PageTitleTextBackgroundColor) ?? Constant.ColorPicker.PageTitleTextBackgroundColorDefault; }
            set { this.SetPropertyValue(page => page.PageTitleTextBackgroundColor, value); }
        }

        [Display(Name = "Hide site header", GroupName = ChildFund.Infrastructure.TabNames.Settings, Order = 100)]
        public virtual bool HideSiteHeader { get; set; }

        [Display(Name = "Hide site footer", GroupName = ChildFund.Infrastructure.TabNames.Settings, Order = 200)]
        public virtual bool HideSiteFooter { get; set; }

        [Display(Name = "CSS files", GroupName = ChildFund.Infrastructure.TabNames.Styles, Order = 100)]
        public virtual LinkItemCollection CssFiles { get; set; }

        [Searchable(false)]
        [Display(Name = "CSS", GroupName = ChildFund.Infrastructure.TabNames.Styles, Order = 200)]
        [UIHint(UIHint.Textarea)]
        public virtual string Css { get; set; }
    }
}
