using ChildFund.Services.Models;

namespace ChildFund.Services.Interfaces;

public interface ILookupClient
{
    Task<List<RefCodeInfoDto>?> GetAllCountriesAsync(CancellationToken ct = default);

    Task<List<RefCodeInfoDto>?> GetGenderAsync(CancellationToken ct = default);

    Task<List<RefCodeInfoDto>?> GetNonIACountriesAsync(CancellationToken ct = default);

    Task<List<CodeInfoDto>?> GetStatesAsync(CancellationToken ct = default);

    Task<List<CodeInfoDto>?> GetStatesAndProvincesAsync(CancellationToken ct = default);

    Task<List<RefCodeInfoDto>?> GetWebCountriesAsync(CancellationToken ct = default);

    Task<List<RefCodeInfoDto>?> GetWebCountriesAvailableSponsorshipsAsync(CancellationToken ct = default);

    Task<List<CodeInfoDto>?> GetWebSuffixesAsync(CancellationToken ct = default);

    Task<List<CodeInfoDto>?> GetWebTitlesAsync(CancellationToken ct = default);
}