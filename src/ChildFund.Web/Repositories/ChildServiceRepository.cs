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

    public async Task<EnvelopeDto> GetAvailableKidsForWeb(ChildFilterDto childFilterDto, CancellationToken ct = default)
    {
        var result = await _childInventoryClient.GetAvailableKidsForWebAsync(childFilterDto, ct);
        return result ?? new EnvelopeDto();
    }

    public async Task<WebChildInfoDto> GetAvailableSingleKidForWeb(int countryCode, CancellationToken ct = default)
    {
        var result = await _childInventoryClient.GetAvailableSingleKidForWebAsync(countryCode, ct);
        return result ?? new WebChildInfoDto();
    }

    public async Task<List<WebChildInfoDto>> GetRandomKidsForWeb(CancellationToken ct = default)
    {
        var result = await _childInventoryClient.GetRandomKidsForWebAsync(ct);
        return result ?? new List<WebChildInfoDto>();
    }

    public async Task<WebChildInfoDto> GetRandomSingleKidForWeb(CancellationToken ct = default)
    {
        var result = await _childInventoryClient.GetRandomSingleKidForWebAsync(ct);
        return result ?? new WebChildInfoDto();
    }

    public Task<byte[]?> GetChildPhoto(int noId, int childNumber, CancellationToken ct = default) =>
        _childInventoryClient.GetChildPhotoAsync(noId, childNumber, ct);

    public Task<int> LockChild(int noId, int childNumber, string sessionId, CancellationToken ct = default) =>
        _childInventoryClient.LockChildAsync(noId, childNumber, sessionId, ct);

    public Task<int> UnLockChild(int noId, int childNumber, string sessionId, CancellationToken ct = default) =>
        _childInventoryClient.UnLockChildAsync(noId, childNumber, sessionId, ct);
}
