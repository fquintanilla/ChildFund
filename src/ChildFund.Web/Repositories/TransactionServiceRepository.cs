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

    public async Task<List<AgpInfoDto>?> GetAGPByContactId(int contactId, CancellationToken ct = default) =>
        await _transactionClient.GetAGPByContactIdAsync(contactId, ct);

    public async Task<AgpInfoDto?> GetAGPByID(int contactId, int agpId, CancellationToken ct = default) =>
        await _transactionClient.GetAGPByIdAsync(contactId, agpId, ct);

    public async Task<List<TransactionInfoDto>?> GetTransactionByID(TransactionInfoDto transactionInfo, CancellationToken ct = default) =>
        await _transactionClient.GetTransactionsByIdAsync(transactionInfo, ct);

    public Task SendChangePasswordEmail(TransactionInfoDto transactionInfo, CancellationToken ct = default) =>
        _transactionClient.SendChangePasswordEmailAsync(transactionInfo, ct);

    public async Task<EnvelopeDto?> ValidateDonation(TransactionInfoDto transactionInfo, CancellationToken ct = default) =>
        await _transactionClient.ValidateDonationAsync(transactionInfo, ct);

    public async Task<EnvelopeDto?> ValidatePayment(TransactionInfoDto transactionInfo, CancellationToken ct = default) =>
        await _transactionClient.ValidatePaymentAsync(transactionInfo, ct);

    public async Task<EnvelopeDto?> SubmitTransactionToQueue(TransactionInfoDto transaction, CancellationToken ct = default) =>
        await _transactionClient.SubmitTransactionToQueueAsync(transaction, ct);

    public Task<bool> CreateCase(int contactId, string contactType, string subject, string question, Dictionary<string, byte[]>? attachmentData = null, CancellationToken ct = default) =>
        _transactionClient.CreateCaseAsync(contactId, contactType, subject, question, attachmentData, ct);
}
