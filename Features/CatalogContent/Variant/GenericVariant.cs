using ChildFund.Features.CatalogContent.Infrastructure;
using ChildFund.Infrastructure.Cms.Constants;
using EPiServer.Commerce.Catalog.DataAnnotations;
using TabNames = ChildFund.Infrastructure.Cms.Constants.TabNames;

namespace ChildFund.Features.CatalogContent.Variant
{
    [CatalogContentType(DisplayName = "Generic Variant", GUID = "cfcf83dc-26b4-406b-a4f1-bfb0c9788c67", Description = "Generic Variant Page type to display products.")]
    public class GenericVariant : VariationContent
    {
        #region Content

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
        //[ClientEditor(ClientEditingClass = "foundation/editors/ColorPicker")]
        public virtual string PageTitleTextColor
        {
            get { return this.GetPropertyValue(page => page.PageTitleTextColor) ?? ColorPicker.PageTitleTextColorDefault; }
            set { this.SetPropertyValue(page => page.PageTitleTextColor, value); }
        }

        [Searchable(false)]
        [Display(Name = "Page Title Text Background Color",
            GroupName = SystemTabNames.Content,
            Order = 30)]
        //[ClientEditor(ClientEditingClass = "foundation/editors/ColorPicker")]
        public virtual string PageTitleTextBackgroundColor
        {
            get { return this.GetPropertyValue(page => page.PageTitleTextBackgroundColor) ?? ColorPicker.PageTitleTextBackgroundColorDefault; }
            set { this.SetPropertyValue(page => page.PageTitleTextBackgroundColor, value); }
        }

        [UIHint(UIHint.Image)]
        [Display(Name = "Image", GroupName = SystemTabNames.Content, Order = 30)]
        public virtual ContentReference ImageReference { get; set; }
        #endregion

        #region Settings
        [Display(Name = "Hide site header", GroupName = TabNames.Settings, Order = 100)]
        public virtual bool HideSiteHeader { get; set; }

        [Display(Name = "Hide site footer", GroupName = TabNames.Settings, Order = 200)]
        public virtual bool HideSiteFooter { get; set; }

        [Searchable(false)]
        [Display(Name = "Marketing Id", GroupName = TabNames.Settings, Order = 10)]
        public virtual string MarketingId { get; set; }

        [Searchable(false)]
        [Display(Name = "Financial Code", GroupName = TabNames.Settings, Order = 20)]
        public virtual string FinancialCode { get; set; }

        [Display(Name = "Exclude from results",
            Description = "This will determine whether or not to show on search",
            GroupName = TabNames.Settings,
            Order = 40)]
        public virtual bool ExcludeFromSearch { get; set; }

        [Display(
            Name = "Recurrence",
            Order = 90,
            GroupName = TabNames.PurchaseSettings,
            Description = "Select one or more: One-time, Yearly, Monthly, Quarterly, Semi-Annually, Annually")]
        [SelectMany(SelectionFactoryType = typeof(RecurrenceSelectionFactory))]
        public virtual string Recurrence { get; set; }

        [Display(
            Name = "Default Recurrence",
            Order = 100,
            GroupName = TabNames.PurchaseSettings,
            Description = "Shown by default to the buyer. Must be one of the selections above.")]
        [SelectOne(SelectionFactoryType = typeof(RecurrenceSelectionFactory))]
        public virtual string DefaultRecurrence { get; set; }

        [Display(
            Name = "Is Quantity Configurable?",
            Order = 110,
            GroupName = TabNames.PurchaseSettings,
            Description = "Indicates whether the buyer can choose the quantity of this product.")]
        public virtual bool IsQuantityConfigurable { get; set; }

        [Display(
            Name = "Buyer Can Define Price?",
            Order = 120,
            GroupName = TabNames.PurchaseSettings,
            Description = "Indicates whether the buyer can enter a custom price instead of a fixed one.")]
        public virtual bool BuyerCanDefinePrice { get; set; }

        #endregion

        #region Styles

        [Display(Name = "CSS files", GroupName = TabNames.Styles, Order = 100)]
        public virtual LinkItemCollection CssFiles { get; set; }

        [Searchable(false)]
        [Display(Name = "CSS", GroupName = TabNames.Styles, Order = 200)]
        [UIHint(UIHint.Textarea)]
        public virtual string Css { get; set; }

        #endregion

        #region Upsell Rules

        [CultureSpecific]
        [Display(
            Name = "Is Upsell Product",
            Order = 10,
            GroupName = TabNames.UpsellRules,
            Description = "Marks this variant as eligible for upsell logic.")]
        public virtual bool IsUpsell { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Upsell Sequence",
            Order = 20,
            GroupName = TabNames.UpsellRules,
            Description = "Defines global priority for upsells. Lower values = higher priority.")]
        public virtual int UpsellSequence { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Upsell Tags",
            Order = 30,
            GroupName = TabNames.UpsellRules,
            Description = "Freeform tags for grouping upsell items (e.g., 'emergency', 'premium', 'seasonal').")]
        [BackingType(typeof(PropertyStringList))]
        public virtual IList<string>? UpsellTags { get; set; }

        #endregion
    }
}
