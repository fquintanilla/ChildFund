using System.ComponentModel.DataAnnotations;
using EPiServer.Security;

namespace ChildFund.Infrastructure
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

        [Display(Order = 250)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string UpsellRules = "Upsell Rules";

        [Display(Order = 260)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string MetaData = "Metadata";

        [Display(Order = 270)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string Styles = "Styles";

        [Display(Name = "Settings", Order = 292)]
        public const string Settings = SystemTabNames.Settings;
    }
}
