using ChildFund.Services.Interfaces;
using ChildFund.Services.Models;
using ChildFund.Web.Infrastructure.Cms.Services;

namespace ChildFund.Web.Repositories;

/// <summary>
/// Repository for cached lookup data (countries, reference data, etc.)
/// Provides caching layer over ChildFund.Services API calls.
/// </summary>
public class LookupServiceRepository : ILookupServiceRepository
{
    private readonly ILookupClient _lookupClient;
    private readonly ICacheService _cache;

    #region CacheKeys
    private const string CountriesCacheKey = "ChildFund:Lookup:Countries:v1";
    private const string GenderCacheKey = "ChildFund:Lookup:Gender:v1";
    private const string NonIACountriesCacheKey = "ChildFund:Lookup:NonIACountries:v1";
    private const string StatesCacheKey = "ChildFund:Lookup:States:v1";
    private const string StatesAndProvincesCacheKey = "ChildFund:Lookup:StatesAndProvinces:v1";
    private const string WebCountriesCacheKey = "ChildFund:Lookup:WebCountries:v1";
    private const string WebCountriesAvailableSponsorshipsCacheKey = "ChildFund:Lookup:WebCountriesAvailableSponsorships:v1";
    private const string WebSuffixesCacheKey = "ChildFund:Lookup:WebSuffixes:v1";
    private const string WebTitlesCacheKey = "ChildFund:Lookup:WebTitles:v1";
    #endregion

    private const int LookupCacheDurationSeconds = 3600; // 1 hour for all lookup data

    public LookupServiceRepository(
        ILookupClient lookupClient,
        ICacheService cache)
    {
        _lookupClient = lookupClient ?? throw new ArgumentNullException(nameof(lookupClient));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public async Task<List<RefCodeInfoDto>?> GetAllCountriesAsync(bool forceRefresh = false, CancellationToken ct = default)
    {
        return await GetCachedLookupDataAsync(CountriesCacheKey, () => _lookupClient.GetAllCountriesAsync(ct), forceRefresh, ct);
    }

    public async Task<List<RefCodeInfoDto>?> GetGenderAsync(bool forceRefresh = false, CancellationToken ct = default)
    {
        return await GetCachedLookupDataAsync(GenderCacheKey, () => _lookupClient.GetGenderAsync(ct), forceRefresh, ct);
    }

    public async Task<List<RefCodeInfoDto>?> GetNonIACountriesAsync(bool forceRefresh = false, CancellationToken ct = default)
    {
        return await GetCachedLookupDataAsync(NonIACountriesCacheKey, () => _lookupClient.GetNonIACountriesAsync(ct), forceRefresh, ct);
    }

    public async Task<List<CodeInfoDto>?> GetStatesAsync(bool forceRefresh = false, CancellationToken ct = default)
    {
        return await GetCachedLookupDataAsync(StatesCacheKey, () => _lookupClient.GetStatesAsync(ct), forceRefresh, ct);
    }

    public async Task<List<CodeInfoDto>?> GetStatesAndProvincesAsync(bool forceRefresh = false, CancellationToken ct = default)
    {
        return await GetCachedLookupDataAsync(StatesAndProvincesCacheKey, () => _lookupClient.GetStatesAndProvincesAsync(ct), forceRefresh, ct);
    }

    public async Task<List<RefCodeInfoDto>?> GetWebCountriesAsync(bool forceRefresh = false, CancellationToken ct = default)
    {
        return await GetCachedLookupDataAsync(WebCountriesCacheKey, () => _lookupClient.GetWebCountriesAsync(ct), forceRefresh, ct);
    }

    public async Task<List<RefCodeInfoDto>?> GetWebCountriesAvailableSponsorshipsAsync(bool forceRefresh = false, CancellationToken ct = default)
    {
        return await GetCachedLookupDataAsync(WebCountriesAvailableSponsorshipsCacheKey, () => _lookupClient.GetWebCountriesAvailableSponsorshipsAsync(ct), forceRefresh, ct);
    }

    public async Task<List<CodeInfoDto>?> GetWebSuffixesAsync(bool forceRefresh = false, CancellationToken ct = default)
    {
        return await GetCachedLookupDataAsync(WebSuffixesCacheKey, () => _lookupClient.GetWebSuffixesAsync(ct), forceRefresh, ct);
    }

    public async Task<List<CodeInfoDto>?> GetWebTitlesAsync(bool forceRefresh = false, CancellationToken ct = default)
    {
        return await GetCachedLookupDataAsync(WebTitlesCacheKey, () => _lookupClient.GetWebTitlesAsync(ct), forceRefresh, ct);
    }

    /// <summary>
    /// Generic method for caching lookup data with force refresh capability.
    /// </summary>
    private async Task<T?> GetCachedLookupDataAsync<T>(
        string cacheKey,
        Func<Task<T>> fetchData,
        bool forceRefresh = false,
        CancellationToken ct = default) where T : class?
    {
        // If not forcing refresh, try to get from cache first
        if (!forceRefresh && _cache.Exists(cacheKey))
        {
            return _cache.Get<T>(cacheKey);
        }

        // Cache miss or force refresh - fetch from API
        var data = await fetchData().WaitAsync(ct);

        // Cache the result if it's not null/empty
        if (data != null)
        {
            _cache.AddBySeconds(cacheKey, data, LookupCacheDurationSeconds);
        }

        return data;
    }
}