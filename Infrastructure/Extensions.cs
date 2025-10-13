using ChildFund.Features.Home;
using ChildFund.Features.MyAccount.AddressBook;
using ChildFund.Features.MyOrganization;
using ChildFund.Infrastructure.Commerce.Customer;
using ChildFund.Infrastructure.Commerce.Customer.Services;
using EPiServer.Commerce.Catalog.Linking;

namespace ChildFund.Infrastructure
{
	public static class Extensions
    {
        private static readonly Lazy<IContentRepository> _contentRepository =
            new Lazy<IContentRepository>(() => ServiceLocator.Current.GetInstance<IContentRepository>());

        private static readonly Lazy<IRelationRepository> RelationRepository =
          new Lazy<IRelationRepository>(() => ServiceLocator.Current.GetInstance<IRelationRepository>());
        
        public static ContentReference GetRelativeStartPage(this IContent content)
        {
            if (content is HomePage)
            {
                return content.ContentLink;
            }

            var ancestors = _contentRepository.Value.GetAncestors(content.ContentLink);
            var startPage = ancestors.FirstOrDefault(x => x is HomePage) as HomePage;
            return startPage == null ? ContentReference.StartPage : startPage.ContentLink;
        }

        public static bool IsEqual(this AddressModel address,
           AddressModel compareAddressViewModel)
        {
            return address.FirstName == compareAddressViewModel.FirstName &&
                   address.LastName == compareAddressViewModel.LastName &&
                   address.Line1 == compareAddressViewModel.Line1 &&
                   address.Line2 == compareAddressViewModel.Line2 &&
                   address.Organization == compareAddressViewModel.Organization &&
                   address.PostalCode == compareAddressViewModel.PostalCode &&
                   address.City == compareAddressViewModel.City &&
                   address.CountryCode == compareAddressViewModel.CountryCode;
        }

        public static ContactViewModel GetCurrentContactViewModel(this ICustomerService customerService)
        {
            var currentContact = customerService.GetCurrentContact();
            return currentContact?.Contact != null ? new ContactViewModel(currentContact) : new ContactViewModel();
        }

        public static ContactViewModel GetContactViewModelById(this ICustomerService customerService, string id) => new ContactViewModel(customerService.GetContactById(id));

        public static List<ContactViewModel> GetContactViewModelsForOrganization(this ICustomerService customerService, FoundationOrganization organization = null)
        {
            if (organization == null)
            {
                organization = GetCurrentOrganization(customerService);
            }

            if (organization == null)
            {
                return new List<ContactViewModel>();
            }

            var organizationUsers = customerService.GetContactsForOrganization(organization);

            if (organization.SubOrganizations.Count > 0)
            {
                foreach (var subOrg in organization.SubOrganizations)
                {
                    var contacts = customerService.GetContactsForOrganization(subOrg);
                    organizationUsers.AddRange(contacts);
                }
            }

            return organizationUsers.Select(user => new ContactViewModel(user)).ToList();
        }
        
        public static bool IsAjaxRequest(this HttpRequest httpRequest) => httpRequest.Headers["X-Requested-With"] == "XMLHttpRequest";

        private static FoundationOrganization GetCurrentOrganization(ICustomerService customerService)
        {
            var contact = customerService.GetCurrentContact();
            if (contact != null)
            {
                return contact.FoundationOrganization;
            }

            return null;
        }
    }
}
