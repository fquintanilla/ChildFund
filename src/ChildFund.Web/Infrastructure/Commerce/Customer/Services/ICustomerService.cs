using ChildFund.Web.Features.MyOrganization;
using ChildFund.Web.Infrastructure.Cms.Users;
using EPiServer.Cms.UI.AspNetIdentity;
using Microsoft.AspNetCore.Identity;

namespace ChildFund.Web.Infrastructure.Commerce.Customer.Services
{
    public interface ICustomerService
    {
        ServiceAccessor<ApplicationUserManager<SiteUser>> UserManager { get; }
        ServiceAccessor<ApplicationSignInManager<SiteUser>> SignInManager { get; }
        Guid CurrentContactId { get; }
        void CreateContact(FoundationContact contact, string contactId);
        void EditContact(FoundationContact model);
        void RemoveContactFromOrganization(string id);
        bool CanSeeOrganizationNav();
        void AddContactToOrganization(FoundationContact contact, string organizationId = null);
        void UpdateContact(string contactId, string userRole, string location = null);
        FoundationContact GetContactByEmail(string email);
        FoundationContact GetCurrentContact();
        FoundationContact GetContactById(string contactId);
        List<FoundationContact> GetContactsForOrganization(FoundationOrganization organization = null);
        void AddContactToOrganization(FoundationOrganization organization, FoundationContact contact, B2BUserRoles userRole);
        List<FoundationContact> GetContacts();
        Task<SiteUser> GetSiteUserAsync(string email);
        Task<ExternalLoginInfo> GetExternalLoginInfoAsync();
        Task<IdentityContactResult> CreateUser(SiteUser user);
        Task SignOutAsync();
        bool HasOrganization(string contactId);
        ContactViewModel GetCurrentContactViewModel();
    }
}
