using ChildFund.Services.Interfaces;
using ChildFund.Services.Models;

namespace ChildFund.Web.Repositories;

/// <summary>
/// Repository for child inventory operations.
/// Provides access to child data and operations.
/// </summary>
public class ChildServiceRepository : IChildServiceRepository
{
    private readonly IChildInventoryClient _childInventoryClient;

    public ChildServiceRepository(IChildInventoryClient childInventoryClient)
    {
        _childInventoryClient = childInventoryClient ?? throw new ArgumentNullException(nameof(childInventoryClient));
    }

    public async Task<EnvelopeDto?> GetAvailableKidsForWeb(ChildFilterDto childFilterDto,
        CancellationToken ct = default) =>
        await _childInventoryClient.GetAvailableKidsForWebAsync(childFilterDto, ct);

    public async Task<WebChildInfoDto?> GetAvailableSingleKidForWeb(int countryCode,
        CancellationToken ct = default) =>
        await _childInventoryClient.GetAvailableSingleKidForWebAsync(countryCode, ct);

    public async Task<List<WebChildInfoDto>?> GetRandomKidsForWeb(CancellationToken ct = default) =>
        await _childInventoryClient.GetRandomKidsForWebAsync(ct);

    public async Task<WebChildInfoDto?> GetRandomSingleKidForWeb(CancellationToken ct = default) =>
        await _childInventoryClient.GetRandomSingleKidForWebAsync(ct);

    public Task<byte[]?> GetChildPhoto(int noId, int childNumber, CancellationToken ct = default) =>
        _childInventoryClient.GetChildPhotoAsync(noId, childNumber, ct);

    public Task<int> LockChild(int noId, int childNumber, string sessionId, CancellationToken ct = default) =>
        _childInventoryClient.LockChildAsync(noId, childNumber, sessionId, ct);

    public Task<int> UnLockChild(int noId, int childNumber, string sessionId, CancellationToken ct = default) =>
        _childInventoryClient.UnLockChildAsync(noId, childNumber, sessionId, ct);
}
