using ChildFund.Services.Interfaces;
using ChildFund.Services.Models;
using ChildFund.Services.Providers;
using ChildFund.Services.Serialization;
using Microsoft.Extensions.Options;

namespace ChildFund.Services.ApiClients;

/// <summary>
/// Client for interacting with the ChildFund API - ChildInventory Service.
/// </summary>
public sealed class ChildInventoryClient(
    HttpClient http,
    ITokenProvider tokenProvider,
    IOptions<ChildFundApiOptions> options)
    : ChildFundApiClient(http, tokenProvider, options), IChildInventoryClient
{
    /// <summary>
    /// Retrieves random children for web display from the ChildFund API.
    /// </summary>
    public Task<ChildSummaryDto[]> GetRandomKidsForWebAsync(CancellationToken ct = default) =>
        GetAsync<ChildSummaryDto[]>("ChildInventory/GetRandomKidsForWeb", JsonDefaults.Options, ct);


    public Task<ChildSummaryDto[]> GetAvailableKidsForWebAsync(CancellationToken ct = default) =>
        PostAsync<ChildSummaryDto[]>("ChildInventory/GetAvailableKidsForWeb", null, JsonDefaults.Options, ct);

    public Task<AvailableKidsForWebResponseDto> GetAvailableKidsForWebAsync(int countryCode, CancellationToken ct = default)
    {
        var body = new
        {
            CountryCodek__BackingFieldField = countryCode
        };

        return PostAsync<AvailableKidsForWebResponseDto>(
            "ChildInventory/GetAvailableKidsForWeb",
            body,
            JsonDefaults.Options,
            ct);
    }
}

