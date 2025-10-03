using ChildFund.Features.Shared;
using System.ComponentModel.DataAnnotations;

namespace ChildFund.Features.MyAccount.ResetPassword
{
    public abstract class MailBasePage : FoundationPageData
    {
        [CultureSpecific]
        [Display(Name = "Subject", GroupName = SystemTabNames.Content, Order = 1)]
        public virtual string Subject { get; set; }
    }
}
