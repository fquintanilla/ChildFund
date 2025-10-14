using ChildFund.Services.Models;

namespace ChildFund.Web.Repositories;

/// <summary>
/// Repository for cached lookup data (countries, reference data, etc.)
/// Provides caching layer over ChildFund.Services API calls.
/// </summary>
public interface ILookupRepository
{
    Task<CountryDto[]?> GetAllCountriesAsync(bool forceRefresh = false, CancellationToken ct = default);
}

