using ChildFund.Web.Features.Shared.Pages;

namespace ChildFund.Web.Features.MyAccount.ResetPassword
{
    public abstract class MailBasePage : FoundationPageData
    {
        [CultureSpecific]
        [Display(Name = "Subject", GroupName = SystemTabNames.Content, Order = 1)]
        public virtual string Subject { get; set; }
    }
}
