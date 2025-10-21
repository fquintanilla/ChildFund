using ChildFund.Services.Interfaces;
using ChildFund.Services.Models;
using ChildFund.Services.Providers;
using ChildFund.Services.Serialization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ChildFund.Services.ApiClients;

/// <summary>
/// Client for interacting with the ChildFund API - Child Inventory Service.
/// </summary>
public sealed class ChildInventoryClient : ChildFundApiClient, IChildInventoryClient
{
    public ChildInventoryClient(
        HttpClient http,
        ITokenProvider tokenProvider,
        IOptions<ChildFundApiOptions> options)
        : base(http, tokenProvider, options)
    {
    }

    public Task<EnvelopeDto?> GetAvailableKidsForWebAsync(ChildFilterDto childFilterDto, CancellationToken ct = default) =>
        PostAsync<EnvelopeDto?>("ChildInventory/GetAvailableKidsForWeb", childFilterDto, JsonDefaults.Options, ct);

    public Task<WebChildInfoDto?> GetAvailableSingleKidForWebAsync(int countryCode, CancellationToken ct = default) =>
        GetAsync<WebChildInfoDto?>($"ChildInventory/GetAvailableSingleKidForWeb?countryCode={countryCode}", JsonDefaults.Options, ct);

    public Task<List<WebChildInfoDto>?> GetRandomKidsForWebAsync(CancellationToken ct = default) =>
        GetAsync<List<WebChildInfoDto>?>("ChildInventory/GetRandomKidsForWeb", JsonDefaults.Options, ct);

    public Task<WebChildInfoDto?> GetRandomSingleKidForWebAsync(CancellationToken ct = default) =>
        GetAsync<WebChildInfoDto?>("ChildInventory/GetRandomSingleKidForWeb", JsonDefaults.Options, ct);

    public async Task<byte[]?> GetChildPhotoAsync(int noId, int childNumber, CancellationToken ct = default)
    {
        using var response = await GetResponseAsync($"ChildInventory/GetChildPhoto?noId={noId}&childNumber={childNumber}", ct);
        var jsonData = await response.Content.ReadAsStringAsync(ct);
        return JsonConvert.DeserializeObject<byte[]>(jsonData);
    }

    public async Task<int> LockChildAsync(int noId, int childNumber, string sessionId, CancellationToken ct = default)
    {
        using var response = await PostResponseAsync($"ChildInventory/LockChild?noId={noId}&childNumber={childNumber}&sessionId={sessionId}", null, null, ct);
        var content = await response.Content.ReadAsStringAsync(ct);
        return int.TryParse(content, out var result) ? result : 0;
    }

    public async Task<int> UnLockChildAsync(int noId, int childNumber, string sessionId, CancellationToken ct = default)
    {
        using var response = await PostResponseAsync($"ChildInventory/UnLockChild?noId={noId}&childNumber={childNumber}&sessionId={sessionId}", null, null, ct);
        var content = await response.Content.ReadAsStringAsync(ct);
        return int.TryParse(content, out var result) ? result : 0;
    }
}