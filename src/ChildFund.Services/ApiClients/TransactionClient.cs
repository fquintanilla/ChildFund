using ChildFund.Services.Interfaces;
using ChildFund.Services.Models;
using ChildFund.Services.Providers;
using ChildFund.Services.Serialization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ChildFund.Services.ApiClients;

/// <summary>
/// Client for interacting with the ChildFund API - Transaction Service.
/// </summary>
public sealed class TransactionClient : ChildFundApiClient, ITransactionClient
{
    public TransactionClient(
        HttpClient http,
        ITokenProvider tokenProvider,
        IOptions<ChildFundApiOptions> options)
        : base(http, tokenProvider, options)
    {
    }

    public Task<List<AgpInfoDto>?> GetAGPByContactIdAsync(int contactId, CancellationToken ct = default) =>
        GetAsync<List<AgpInfoDto>?>($"Transaction/GetAGPByContactID?contactId={contactId}", JsonDefaults.Options, ct);

    public Task<AgpInfoDto?> GetAGPByIdAsync(int contactId, int agpId, CancellationToken ct = default) =>
        GetAsync<AgpInfoDto?>($"Transaction/GetAGPByID?contactId={contactId}&agpId={agpId}", JsonDefaults.Options, ct);

    public Task<List<TransactionInfoDto>?> GetTransactionsByIdAsync(TransactionInfoDto transactionInfo, CancellationToken ct = default) =>
        PostAsync<List<TransactionInfoDto>?>("Transaction/GetTransactionsByID", transactionInfo, JsonDefaults.Options, ct);

    public async Task SendChangePasswordEmailAsync(TransactionInfoDto transactionInfo, CancellationToken ct = default)
    {
        await PostAsync<object?>("Transaction/SendChangePasswordEmail", transactionInfo, JsonDefaults.Options, ct);
    }

    public Task<EnvelopeDto?> ValidateDonationAsync(TransactionInfoDto transactionInfo, CancellationToken ct = default) =>
        PostAsync<EnvelopeDto?>("Transaction/ValidateDonation", transactionInfo, JsonDefaults.Options, ct);

    public Task<EnvelopeDto?> ValidatePaymentAsync(TransactionInfoDto transactionInfo, CancellationToken ct = default) =>
        PostAsync<EnvelopeDto?>("Transaction/ValidatePayment", transactionInfo, JsonDefaults.Options, ct);

    public Task<EnvelopeDto?> SubmitTransactionToQueueAsync(TransactionInfoDto transaction, CancellationToken ct = default) =>
        PostAsync<EnvelopeDto?>("Transaction/SubmitTransactionToQueue", transaction, JsonDefaults.Options, ct);

    public async Task<bool> CreateCaseAsync(
        int contactId,
        string contactType,
        string subject,
        string question,
        Dictionary<string, byte[]>? attachmentData = null,
        CancellationToken ct = default)
    {
        var queryParams = $"enterpriseContactId={Uri.EscapeDataString(contactId.ToString())}&contactType={Uri.EscapeDataString(contactType)}&subject={Uri.EscapeDataString(subject)}&question={Uri.EscapeDataString(question)}";

        var jsonAttachmentData = attachmentData is null || attachmentData.Count == 0
            ? null
            : JsonConvert.SerializeObject(attachmentData);

        var content = jsonAttachmentData is null
            ? null
            : new StringContent(jsonAttachmentData, System.Text.Encoding.UTF8, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));

        using var response = await PostResponseAsync($"Transaction/CreateCase?{queryParams}", content, null, ct);
        var responseContent = await response.Content.ReadAsStringAsync(ct);
        return bool.TryParse(responseContent, out var result) && result;
    }
}

