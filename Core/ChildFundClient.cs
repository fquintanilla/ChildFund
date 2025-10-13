using ChildFund.Core.Auth;
using ChildFund.Core.Http;
using ChildFund.Core.Models;
using ChildFund.Core.Options;
using Microsoft.Extensions.Options;

namespace ChildFund.Core
{
    public interface IChildFundClient
    {
        Task<CountryDto[]> GetAllCountriesAsync(CancellationToken ct = default);
        Task<ChildSummaryDto[]> GetRandomKidsForWebAsync(CancellationToken ct = default);
    }

    [ServiceConfiguration(typeof(IChildFundClient))]
    public sealed class ChildFundClient(
        HttpClient http,
        ITokenProvider tokenProvider,
        IOptions<ChildFundOptions> options)
        : ChildFundApiClient(http, tokenProvider, options), IChildFundClient
    {
        public Task<CountryDto[]> GetAllCountriesAsync(CancellationToken ct = default) =>
            GetAsync<CountryDto[]>("Lookup/GetAllCountries", JsonDefaults.Options, ct);

        public Task<ChildSummaryDto[]> GetRandomKidsForWebAsync(CancellationToken ct = default) =>
            GetAsync<ChildSummaryDto[]>("ChildInventory/GetRandomKidsForWeb", JsonDefaults.Options, ct);
    }
}
