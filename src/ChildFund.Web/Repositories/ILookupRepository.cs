using ChildFund.Services.Models;

namespace ChildFund.Web.Repositories;

/// <summary>
/// Repository for cached lookup data (countries, reference data, etc.)
/// Provides caching layer over ChildFund.Services API calls.
/// </summary>
public interface ILookupRepository
{
    Task<List<RefCodeInfoDto>?> GetAllCountriesAsync(bool forceRefresh = false, CancellationToken ct = default);

    Task<List<RefCodeInfoDto>?> GetGenderAsync(bool forceRefresh = false, CancellationToken ct = default);

    Task<List<RefCodeInfoDto>?> GetNonIACountriesAsync(bool forceRefresh = false, CancellationToken ct = default);

    Task<List<CodeInfoDto>?> GetStatesAsync(bool forceRefresh = false, CancellationToken ct = default);

    Task<List<CodeInfoDto>?> GetStatesAndProvincesAsync(bool forceRefresh = false, CancellationToken ct = default);

    Task<List<RefCodeInfoDto>?> GetWebCountriesAsync(bool forceRefresh = false, CancellationToken ct = default);

    Task<List<RefCodeInfoDto>?> GetWebCountriesAvailableSponsorshipsAsync(bool forceRefresh = false, CancellationToken ct = default);

    Task<List<CodeInfoDto>?> GetWebSuffixesAsync(bool forceRefresh = false, CancellationToken ct = default);

    Task<List<CodeInfoDto>?> GetWebTitlesAsync(bool forceRefresh = false, CancellationToken ct = default);
}

