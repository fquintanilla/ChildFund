using ChildFund.Services.Interfaces;
using ChildFund.Services.Models;
using ChildFund.Services.Providers;
using ChildFund.Services.Serialization;
using Microsoft.Extensions.Options;

namespace ChildFund.Services.ApiClients
{
    public class DonorPortalClient (
        HttpClient http,
        ITokenProvider tokenProvider,
        IOptions<ChildFundApiOptions> options)
        : ChildFundApiClient(http, tokenProvider, options), IDonorPortalClient
    {
        public Task<List<SponsoredChildrenInfoDto>> GetLteChildrenByContactId(string id, CancellationToken ct = default) =>
            GetAsync<List<SponsoredChildrenInfoDto>>($"DonorPortal/GetLTEChildrenByContactID/{id}", JsonDefaults.Options, ct);

        public Task<TransactionInfoDto> GetContactByIdAsync(string id, CancellationToken ct = default) =>
            GetAsync<TransactionInfoDto>($"DonorPortal/GetContactByID/{id}", JsonDefaults.Options, ct);

        public Task<ContactInfoDto[]> FindContactsAsync(string email, CancellationToken ct = default)
        {
            var body = new
            {
                _email = email
            };

            return PostAsync<ContactInfoDto[]>(
                "DonorPortal/FindContacts",
                body,
                JsonDefaults.Options,
                ct);
        }
    }
}
