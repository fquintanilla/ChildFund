using ChildFund.Services.Models;

namespace ChildFund.Web.Repositories;

/// <summary>
/// Repository for child inventory operations.
/// Provides access to child data and operations.
/// </summary>
public interface IChildServiceRepository
{
    Task<EnvelopeDto> GetAvailableKidsForWeb(ChildFilterDto childFilterDto, CancellationToken ct = default);

    Task<WebChildInfoDto> GetAvailableSingleKidForWeb(int countryCode, CancellationToken ct = default);

    Task<List<WebChildInfoDto>> GetRandomKidsForWeb(CancellationToken ct = default);

    Task<WebChildInfoDto> GetRandomSingleKidForWeb(CancellationToken ct = default);

    Task<byte[]?> GetChildPhoto(int noId, int childNumber, CancellationToken ct = default);

    Task<int> LockChild(int noId, int childNumber, string sessionId, CancellationToken ct = default);

    Task<int> UnLockChild(int noId, int childNumber, string sessionId, CancellationToken ct = default);
}
