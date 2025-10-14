using ChildFund.Services.Interfaces;
using ChildFund.Services.Models;
using ChildFund.Services.Providers;
using ChildFund.Services.Serialization;
using Microsoft.Extensions.Options;

namespace ChildFund.Services.ApiClients;

/// <summary>
/// Client for interacting with the ChildFund API - Lookup Service.
/// </summary>
public sealed class LookupClient : ChildFundApiClient, ILookupClient
{
    public LookupClient(
        HttpClient http,
        ITokenProvider tokenProvider,
        IOptions<ChildFundApiOptions> options)
        : base(http, tokenProvider, options)
    {
    }

    /// <summary>
    /// Retrieves all available countries from the ChildFund API.
    /// </summary>
    public Task<CountryDto[]?> GetAllCountriesAsync(CancellationToken ct = default) =>
        GetAsync<CountryDto[]?>("Lookup/GetAllCountries", JsonDefaults.Options, ct);
}

