using Microsoft.AspNetCore.Identity;

namespace ChildFund.Web.Infrastructure.Commerce.Customer
{
    public class IdentityContactResult
    {
        public IdentityResult IdentityResult { get; set; }
        public FoundationContact FoundationContact { get; set; }
        public IdentityContactResult() => IdentityResult = new IdentityResult();
    }
}
