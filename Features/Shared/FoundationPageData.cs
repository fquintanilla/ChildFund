using System.ComponentModel.DataAnnotations;
using ChildFund.Infrastructure;
using EPiServer.Web;
using Geta.Optimizely.Categories.DataAnnotations;

namespace ChildFund.Features.Shared
{
    public abstract class FoundationPageData : PageData
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
        [Display(Name = "Title", GroupName = TabNames.MetaData, Order = 100)]
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
        [Display(Name = "Content type", GroupName = TabNames.MetaData, Order = 310)]
        public virtual string MetaContentType { get; set; }

        [CultureSpecific]
        [Display(Name = "Industry", GroupName = TabNames.MetaData, Order = 320)]
        public virtual string Industry { get; set; }

        [CultureSpecific]
        [Display(Name = "Author", GroupName = TabNames.MetaData, Order = 320)]
        public virtual string AuthorMetaData { get; set; }

        [CultureSpecific]
        [Display(Name = "Disable indexing", GroupName = TabNames.MetaData, Order = 400)]
        public virtual bool DisableIndexing { get; set; }

        #endregion
    }
}
