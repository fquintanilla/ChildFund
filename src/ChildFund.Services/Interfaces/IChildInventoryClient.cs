using ChildFund.Services.Models;

namespace ChildFund.Services.Interfaces;

/// <summary>
/// Client for interacting with the ChildFund API - Child Inventory Service.
/// </summary>
public interface IChildInventoryClient
{
    Task<EnvelopeDto?> GetAvailableKidsForWebAsync(ChildFilterDto childFilterDto, CancellationToken ct = default);

    Task<WebChildInfoDto?> GetAvailableSingleKidForWebAsync(int countryCode, CancellationToken ct = default);

    Task<List<WebChildInfoDto>?> GetRandomKidsForWebAsync(CancellationToken ct = default);

    Task<WebChildInfoDto?> GetRandomSingleKidForWebAsync(CancellationToken ct = default);

    Task<byte[]?> GetChildPhotoAsync(int noId, int childNumber, CancellationToken ct = default);

    Task<int> LockChildAsync(int noId, int childNumber, string sessionId, CancellationToken ct = default);

    Task<int> UnLockChildAsync(int noId, int childNumber, string sessionId, CancellationToken ct = default);
}