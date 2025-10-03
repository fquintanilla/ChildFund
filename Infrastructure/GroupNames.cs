using System.ComponentModel.DataAnnotations;

namespace ChildFund.Infrastructure
{
    [GroupDefinitions]
    public static class GroupNames
    {
        [Display(Name = "Content", Order = 510)]
        public const string Content = "Content";

        [Display(Order = 520)]
        public const string Commerce = "Commerce";
    }
}
