using ChildFund.Services.Models;

namespace ChildFund.Services.Interfaces
{
    public interface IDonorPortalClient
    {
        Task<List<SponsoredChildrenInfoDto>> GetLteChildrenByContactId(string id, CancellationToken ct = default);

        Task<TransactionInfoDto> GetContactByIdAsync(string id, CancellationToken ct = default);

        Task<ContactInfoDto[]> FindContactsAsync(string email, CancellationToken ct = default);
    }
}
