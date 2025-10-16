using ChildFund.Services.Models;

namespace ChildFund.Web.Repositories;

/// <summary>
/// Repository for transaction operations.
/// Provides access to transaction, payment, and AGP data.
/// </summary>
public interface ITransactionServiceRepository
{
    Task<List<AgpInfoDto>> GetAGPByContactId(int contactId, CancellationToken ct = default);

    Task<AgpInfoDto> GetAGPByID(int contactId, int agpId, CancellationToken ct = default);

    Task<List<TransactionInfoDto>> GetTransactionByID(TransactionInfoDto transactionInfo, CancellationToken ct = default);

    Task SendChangePasswordEmail(TransactionInfoDto transactionInfo, CancellationToken ct = default);

    Task<EnvelopeDto> ValidateDonation(TransactionInfoDto transactionInfo, CancellationToken ct = default);

    Task<EnvelopeDto> ValidatePayment(TransactionInfoDto transactionInfo, CancellationToken ct = default);

    Task<EnvelopeDto> SubmitTransactionToQueue(TransactionInfoDto transaction, CancellationToken ct = default);

    Task<bool> CreateCase(int contactId, string contactType, string subject, string question, Dictionary<string, byte[]>? attachmentData = null, CancellationToken ct = default);
}

