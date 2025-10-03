using System.ComponentModel.DataAnnotations;
using EPiServer.Security;

namespace ChildFund.Infrastructure
{
    [GroupDefinitions]
    public static class TabNames
    {
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
