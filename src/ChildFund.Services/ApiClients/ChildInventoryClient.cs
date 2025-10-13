using ChildFund.Services.Interfaces;
using ChildFund.Services.Models;
using ChildFund.Services.Providers;
using ChildFund.Services.Serialization;
using Microsoft.Extensions.Options;

namespace ChildFund.Services.ApiClients;

/// <summary>
/// Client for interacting with the ChildFund API - ChildInventory Service.
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

    /// <summary>
    /// Retrieves random children for web display from the ChildFund API.
    /// </summary>
    public Task<ChildSummaryDto[]> GetRandomKidsForWebAsync(CancellationToken ct = default) =>
        GetAsync<ChildSummaryDto[]>("ChildInventory/GetRandomKidsForWeb", JsonDefaults.Options, ct);
}

