using ChildFund.Services.Models;

namespace ChildFund.Services.Interfaces;

/// <summary>
/// Client for interacting with the ChildFund API - Transaction Service.
/// </summary>
public interface ITransactionClient
{
    Task<List<AgpInfoDto>?> GetAGPByContactIdAsync(int contactId, CancellationToken ct = default);

    Task<AgpInfoDto?> GetAGPByIdAsync(int contactId, int agpId, CancellationToken ct = default);

    Task<List<TransactionInfoDto>?> GetTransactionsByIdAsync(TransactionInfoDto transactionInfo, CancellationToken ct = default);

    Task SendChangePasswordEmailAsync(TransactionInfoDto transactionInfo, CancellationToken ct = default);

    Task<EnvelopeDto?> ValidateDonationAsync(TransactionInfoDto transactionInfo, CancellationToken ct = default);

    Task<EnvelopeDto?> ValidatePaymentAsync(TransactionInfoDto transactionInfo, CancellationToken ct = default);

    Task<EnvelopeDto?> SubmitTransactionToQueueAsync(TransactionInfoDto transaction, CancellationToken ct = default);

    Task<bool> CreateCaseAsync(int contactId, string contactType, string subject, string question, Dictionary<string, byte[]>? attachmentData = null, CancellationToken ct = default);
}

