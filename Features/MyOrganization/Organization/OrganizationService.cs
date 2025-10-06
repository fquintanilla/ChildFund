using ChildFund.Features.MyAccount.AddressBook;
using ChildFund.Infrastructure.Commerce.Customer;
using Mediachase.Commerce.Customers;

namespace ChildFund.Features.MyOrganization.Organization
{
    public class OrganizationService(IAddressBookService addressBookService) : IOrganizationService
    {
        private readonly CustomerContext _customerContext = CustomerContext.Current;

        public OrganizationModel GetOrganizationModel(FoundationOrganization organization = null)
        {
            if (organization == null)
            {
                organization = GetCurrentFoundationOrganization();
            }

            if (organization == null)
            {
                return null;
            }

            if (organization.ParentOrganizationId == Guid.Empty)
            {
                return new OrganizationModel(organization);
            }

            var parentOrganization = GetFoundationOrganizationById(organization.ParentOrganizationId.ToString());
            return new OrganizationModel(organization)
            {
                ParentOrganization = new OrganizationModel(parentOrganization),
                ParentOrganizationId = parentOrganization.OrganizationId
            };
        }
        
        public void CreateOrganization(OrganizationModel organizationInfo)
        {
            var organization = FoundationOrganization.New();
            organization.Name = organizationInfo.Name;
            organization.SaveChanges();

            var contact = GetCurrentContact();
            if (contact != null)
            {
                AddContactToOrganization(organization, contact, B2BUserRoles.Admin);
            }

            addressBookService.UpdateOrganizationAddress(organization, organizationInfo.Address);
        }

        public void UpdateOrganization(OrganizationModel organizationInfo)
        {
            var organization = GetFoundationOrganizationById(organizationInfo.OrganizationId.ToString());
            organization.Name = organizationInfo.Name;
            organization.SaveChanges();
            addressBookService.UpdateOrganizationAddress(organization, organizationInfo.Address);
        }

        public OrganizationModel GetOrganizationModel(Guid id) => new OrganizationModel(GetFoundationOrganizationById(id.ToString()));

        public List<OrganizationModel> GetOrganizationModels()
        {
            return GetOrganizations()
                .Select(x => new OrganizationModel(x))
                .ToList();
        }

        public FoundationOrganization GetCurrentFoundationOrganization() => GetCurrentContact()?.FoundationOrganization;

        public FoundationOrganization GetFoundationOrganizationById(string organizationId)
        {
            if (string.IsNullOrEmpty(organizationId))
            {
                return null;
            }

            var organization = _customerContext.GetOrganizationById(organizationId);
            return organization != null ? new FoundationOrganization(organization) : null;
        }

        public List<FoundationOrganization> GetOrganizations()
        {
            return CustomerContext.Current.GetOrganizations().Where(x => !x.ParentId.HasValue)
                .Select(x => new FoundationOrganization(x))
                .ToList();
        }

        private void AddContactToOrganization(FoundationOrganization organization, FoundationContact contact, B2BUserRoles userRole)
        {
            contact.FoundationOrganization = organization;
            contact.UserRole = userRole.ToString();
            contact.SaveChanges();
        }

        private FoundationContact GetCurrentContact()
        {
            var contact = _customerContext.CurrentContact;
            if (contact == null)
            {
                return null;
            }

            return new FoundationContact(contact);
        }
    }
}
