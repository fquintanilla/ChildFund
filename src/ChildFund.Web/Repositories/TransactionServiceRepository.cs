using ChildFund.Services.Interfaces;
using ChildFund.Services.Models;

namespace ChildFund.Web.Repositories;

/// <summary>
/// Repository for transaction operations.
/// Provides access to transaction, payment, and AGP data.
/// </summary>
public class TransactionServiceRepository : ITransactionServiceRepository
{
    private readonly ITransactionClient _transactionClient;

    public TransactionServiceRepository(ITransactionClient transactionClient)
    {
        _transactionClient = transactionClient ?? throw new ArgumentNullException(nameof(transactionClient));
    }

    public async Task<List<AgpInfoDto>> GetAGPByContactId(int contactId, CancellationToken ct = default)
    {
        var result = await _transactionClient.GetAGPByContactIdAsync(contactId, ct);
        return result ?? new List<AgpInfoDto>();
    }

    public async Task<AgpInfoDto> GetAGPByID(int contactId, int agpId, CancellationToken ct = default)
    {
        var result = await _transactionClient.GetAGPByIdAsync(contactId, agpId, ct);
        return result ?? new AgpInfoDto();
    }

    public async Task<List<TransactionInfoDto>> GetTransactionByID(TransactionInfoDto transactionInfo, CancellationToken ct = default)
    {
        var result = await _transactionClient.GetTransactionsByIdAsync(transactionInfo, ct);
        return result ?? new List<TransactionInfoDto>();
    }

    public Task SendChangePasswordEmail(TransactionInfoDto transactionInfo, CancellationToken ct = default) =>
        _transactionClient.SendChangePasswordEmailAsync(transactionInfo, ct);

    public async Task<EnvelopeDto> ValidateDonation(TransactionInfoDto transactionInfo, CancellationToken ct = default)
    {
        var result = await _transactionClient.ValidateDonationAsync(transactionInfo, ct);
        return result ?? new EnvelopeDto();
    }

    public async Task<EnvelopeDto> ValidatePayment(TransactionInfoDto transactionInfo, CancellationToken ct = default)
    {
        var result = await _transactionClient.ValidatePaymentAsync(transactionInfo, ct);
        return result ?? new EnvelopeDto();
    }

    public async Task<EnvelopeDto> SubmitTransactionToQueue(TransactionInfoDto transaction, CancellationToken ct = default)
    {
        var result = await _transactionClient.SubmitTransactionToQueueAsync(transaction, ct);
        return result ?? new EnvelopeDto();
    }

    public Task<bool> CreateCase(int contactId, string contactType, string subject, string question, Dictionary<string, byte[]>? attachmentData = null, CancellationToken ct = default) =>
        _transactionClient.CreateCaseAsync(contactId, contactType, subject, question, attachmentData, ct);
}

