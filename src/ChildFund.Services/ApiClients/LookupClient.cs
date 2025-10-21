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

    public Task<List<RefCodeInfoDto>?> GetAllCountriesAsync(CancellationToken ct = default) =>
        GetAsync<List<RefCodeInfoDto>?>("Lookup/GetAllCountries", JsonDefaults.Options, ct);

    public Task<List<RefCodeInfoDto>?> GetGenderAsync(CancellationToken ct = default) =>
        GetAsync<List<RefCodeInfoDto>?>("Lookup/GetGender", JsonDefaults.Options, ct);

    public Task<List<RefCodeInfoDto>?> GetNonIACountriesAsync(CancellationToken ct = default) =>
        GetAsync<List<RefCodeInfoDto>?>("Lookup/GetNonIACountries", JsonDefaults.Options, ct);

    public Task<List<CodeInfoDto>?> GetStatesAsync(CancellationToken ct = default) =>
        GetAsync<List<CodeInfoDto>?>("Lookup/GetStates", JsonDefaults.Options, ct);

    public Task<List<CodeInfoDto>?> GetStatesAndProvincesAsync(CancellationToken ct = default) =>
        GetAsync<List<CodeInfoDto>?>("Lookup/GetStatesAndProvinces", JsonDefaults.Options, ct);

    public Task<List<RefCodeInfoDto>?> GetWebCountriesAsync(CancellationToken ct = default) =>
        GetAsync<List<RefCodeInfoDto>?>("Lookup/GetWebCountries", JsonDefaults.Options, ct);

    public Task<List<RefCodeInfoDto>?> GetWebCountriesAvailableSponsorshipsAsync(CancellationToken ct = default) =>
        GetAsync<List<RefCodeInfoDto>?>("Lookup/GetWebCountriesAvailableSponsorships", JsonDefaults.Options, ct);

    public Task<List<CodeInfoDto>?> GetWebSuffixesAsync(CancellationToken ct = default) =>
        GetAsync<List<CodeInfoDto>?>("Lookup/GetWebSuffixes", JsonDefaults.Options, ct);

    public Task<List<CodeInfoDto>?> GetWebTitlesAsync(CancellationToken ct = default) =>
        GetAsync<List<CodeInfoDto>?>("Lookup/GetWebTitles", JsonDefaults.Options, ct);
}

