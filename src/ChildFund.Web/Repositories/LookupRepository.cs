using ChildFund.Services.Interfaces;
using ChildFund.Services.Models;
using ChildFund.Web.Infrastructure.Cms.Services;

namespace ChildFund.Web.Repositories;

/// <summary>
/// Repository for cached lookup data (countries, reference data, etc.)
/// Provides caching layer over ChildFund.Services API calls.
/// </summary>
public class LookupRepository : ILookupRepository
{
    private readonly ILookupClient _lookupClient;
    private readonly ICacheService _cache;
    private const int CountriesCacheDurationSeconds = 3600; // 1 hour

    #region CacheKeys
    private const string CountriesCacheKey = "ChildFund:Lookup:Countries:v1";
    #endregion

    public LookupRepository(
        ILookupClient lookupClient,
        ICacheService cache)
    {
        _lookupClient = lookupClient ?? throw new ArgumentNullException(nameof(lookupClient));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public async Task<CountryDto[]?> GetAllCountriesAsync(bool forceRefresh = false, CancellationToken ct = default)
    {
        if (!forceRefresh && _cache.Exists(CountriesCacheKey))
            return _cache.Get<CountryDto[]>(CountriesCacheKey);

        var countries = await _lookupClient.GetAllCountriesAsync(ct);
        if (countries != null && countries.Length > 0)
            _cache.AddBySeconds(CountriesCacheKey, countries, CountriesCacheDurationSeconds);

        return countries;
    }
}