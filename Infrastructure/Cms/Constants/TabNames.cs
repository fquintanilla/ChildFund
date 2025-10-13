using EPiServer.Security;

namespace ChildFund.Infrastructure.Cms.Constants
{
	[GroupDefinitions]
    public static class TabNames
    {
        [Display(Name = "Site labels", Order = 75)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string SiteLabels = "SiteLabels";

        [Display(Name = "Site structure", Order = 77)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string SiteStructure = "SiteStructure";

        [Display(Name = "Mail templates", Order = 78)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string MailTemplates = "MailTemplates";

        [Display(Name = "Cache", Order = 90)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string Cache = "Cache";

		[Display(Order = 260)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string MetaData = "Metadata";

        [Display(Order = 270)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string Styles = "Styles";

        [Display(Order = 290)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string Scripts = "Scripts";

		[Display(Name = "Cache", Order = 291)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string PurchaseSettings = "Purchase Settings";

        [Display(Name = "Cache", Order = 292)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string UpsellRules = "Upsell Rules";

        [Display(Name = "Settings", Order = 300)]
        public const string Settings = SystemTabNames.Settings;
    }
}
