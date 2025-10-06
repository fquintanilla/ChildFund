using ChildFund.Infrastructure.Commerce.Customer;

namespace ChildFund.Features.MyOrganization.Organization
{
    public interface IOrganizationService
    {
        OrganizationModel GetOrganizationModel(FoundationOrganization organization = null);
        OrganizationModel GetOrganizationModel(Guid id);
        List<OrganizationModel> GetOrganizationModels();
        void CreateOrganization(OrganizationModel organizationInfo);
        void UpdateOrganization(OrganizationModel organizationInfo);
        FoundationOrganization GetCurrentFoundationOrganization();
        FoundationOrganization GetFoundationOrganizationById(string organizationId);
        List<FoundationOrganization> GetOrganizations();
    }
}
