using ChildFund.Core.Media.Models;
using ChildFund.Features.Shared.Interfaces;
using ChildFund.Infrastructure.Cms.Constants;
using Newtonsoft.Json;

namespace ChildFund.Features.Shared.Pages
{
	public abstract class FoundationPageData : PageData, IFoundationContent
    {
        #region Page Header

        [Categories]
        [Display(Name = "Categories",
            Description = "Categories associated with this content.",
            GroupName = SystemTabNames.PageHeader,
            Order = 10)]
        public virtual IList<ContentReference> Categories { get; set; }

        #endregion

        #region Content

        [CultureSpecific]
        [Display(Name = "Main body", GroupName = SystemTabNames.Content, Order = 100)]
        public virtual XhtmlString MainBody { get; set; }

        [CultureSpecific]
        [Display(Name = "Main content area", GroupName = SystemTabNames.Content, Order = 200)]
        [AllowedTypes(new[] { typeof(IContentData) })]
        public virtual ContentArea MainContentArea { get; set; }

        #endregion

        #region Metadata

        [CultureSpecific]
        [Display(Name = "Meta Title", GroupName = TabNames.MetaData, Order = 100)]
        public virtual string MetaTitle
        {
            get
            {
                var metaTitle = this.GetPropertyValue(p => p.MetaTitle);

                return !string.IsNullOrWhiteSpace(metaTitle)
                    ? metaTitle
                    : PageName;
            }
            set => this.SetPropertyValue(p => p.MetaTitle, value);
        }

        [CultureSpecific]
        [UIHint(UIHint.Textarea)]
        [Display(GroupName = TabNames.MetaData, Order = 200)]
        public virtual string Keywords { get; set; }

        [CultureSpecific]
        [UIHint(UIHint.Textarea)]
        [Display(Name = "Page description", GroupName = TabNames.MetaData, Order = 300)]
        public virtual string PageDescription { get; set; }

        [CultureSpecific]
        [Display(Name = "Author", GroupName = TabNames.MetaData, Order = 320)]
        public virtual string AuthorMetaData { get; set; }

        [UIHint(UIHint.Image)]
        [AllowedTypes(typeof(ImageMediaData))]
        [Display(Name = "Open Graph Image",
	        Description =
		        "Page template will always inherit the OpenGraph image from the parent page, recursively, until an image is managed for the current page." +
		        "Recommended Image Size: 1200x630 px (1.91:1 Aspect Ratio)",
	        GroupName = TabNames.MetaData, Order = 340)]
        [Searchable(false)]
        [JsonIgnore]
        public virtual ContentReference OpenGraphImage { get; set; }

        [CultureSpecific]
        [Display(Name = "Open Graph Title",
	        Description =
		        "If managed, this field will overwrite the Page Title value that has been managed for the current page",
	        GroupName = TabNames.MetaData, Order = 350)]
        [Searchable(false)]
        [JsonIgnore]
        public virtual string OpenGraphTitle { get; set; }

		[CultureSpecific]
        [Display(Name = "Disable indexing", GroupName = TabNames.MetaData, Order = 400)]
        public virtual bool DisableIndexing { get; set; }

        #endregion

        #region Settings
        [CultureSpecific]
        [Display(Name = "Searchable",
            Description = "This will determine whether or not to show on search",
            GroupName = TabNames.Settings,
            Order = 200)]
        [Searchable]
        public virtual bool Searchable { get; set; }

        [CultureSpecific]
        [Display(Name = "Show Breadcrumbs",
            Description = "This will determine whether or not breadcrumb will be displayed",
            GroupName = TabNames.Settings, Order = 300)]
        [Searchable]
        public virtual bool ShowBreadcrumbs { get; set; }

        [CultureSpecific]
        [Display(Name = "Hide site header", GroupName = TabNames.Settings, Order = 400)]
        [Searchable]
        public virtual bool HideSiteHeader { get; set; }

        [CultureSpecific]
        [Display(Name = "Hide site footer", GroupName = TabNames.Settings, Order = 500)]
        [Searchable]
        public virtual bool HideSiteFooter { get; set; }

        [CultureSpecific]
        [Display(Name = "Track ODP", GroupName = TabNames.Settings, Order = 1100)]
        [Searchable]
        public virtual bool TrackOdp { get; set; }

        #endregion

        #region Styles

        [Display(Name = "CSS files", GroupName = TabNames.Styles, Order = 100)]
        [Searchable(false)]
        [JsonIgnore]
        public virtual LinkItemCollection CssFiles { get; set; }

        [UIHint(UIHint.Textarea)]
        [Display(Name = "CSS", GroupName = TabNames.Styles, Order = 200)]
        [Searchable(false)]
        [JsonIgnore]
        public virtual string Css { get; set; }

        #endregion
    }
}
