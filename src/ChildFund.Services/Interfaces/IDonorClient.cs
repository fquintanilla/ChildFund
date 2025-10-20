using ChildFund.Services.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChildFund.Services.Interfaces;

/// <summary>
/// Client for interacting with the ChildFund API - Donor Service.
/// </summary>
public interface IDonorClient
{
    Task<List<ContactInfoDto>?> FindContactsAsync(ContactInfoDto contactInfo, CancellationToken ct = default);
    Task<TransactionInfoDto?> GetContactByIdAsync(int contactId, CancellationToken ct = default);
    Task<int> GetContactIdByEmailAsync(string email, CancellationToken ct = default);
    Task<EnvelopeDto?> AddAgpAsync(AgpInfoDto agpInfo, CancellationToken ct = default);
    Task<EnvelopeDto?> UpdateContactAsync(ContactUpdateInfoDto contactUpdateInfo, CancellationToken ct = default);
    Task<TaxTotalInfoDto?> GetContactTaxTotalsAsync(int contactId, CancellationToken ct = default);
    Task<Dictionary<string, string>?> GetCSGStatementListAsync(int contactId, CancellationToken ct = default);
    Task<byte[]?> GetCSGStatementAsync(string statementId, CancellationToken ct = default);
    Task<List<EmailSubscriptionsInfoDto>?> GetEmailPublicationsAsync(int contactId, CancellationToken ct = default);
    Task<bool> GetHandlingFeeAsync(int contactId, CancellationToken ct = default);
    Task<List<CodeInfoDto>?> GetHearAboutUsAsync(CancellationToken ct = default);
    Task<int> GetLTELettersTotalAsync(int contactId, string folderName, int childId, bool unreadOnly, CancellationToken ct = default);
    Task<EnvelopeDto?> ChangeEmailSubscriptionsAsync(int contactId, List<string> emailSubscriptions, CancellationToken ct = default);
    Task<List<SponsoredChildrenInfoDto>?> GetLTEChildrenByContactIdAsync(int contactId, CancellationToken ct = default);
    Task<LTELetterFileInfoDto?> GetLTELetterFileInfoAsync(int contactId, int letterId, CancellationToken ct = default);
    Task<List<SponsoredChildrenInfoDto>?> GetSponsoredChildrenAsync(int contactId, CancellationToken ct = default);
    Task<List<SponsoredChildrenInfoDto>?> PaySetupAsync(int contactId, CancellationToken ct = default);
    Task<string?> UpdateContactEmailAsync(int contactId, string email, CancellationToken ct = default);
    Task<bool> UpdateHandlingFeeAsync(int contactId, bool acceptDfFee, CancellationToken ct = default);
    Task<bool> UpdateOptInByChildAsync(int contactId, int noId, int childNumber, bool optIn, CancellationToken ct = default);
    Task<string?> GetBankNameAsync(int routingNumber, CancellationToken ct = default);
    Task<EnvelopeDto?> ReplaceAgpAsync(int contactId, int oldPaymentId, int paymentId, CancellationToken ct = default);
    Task<EnvelopeDto?> UpdateAgpAsync(AgpInfoDto agpInfo, CancellationToken ct = default);
    Task<EnvelopeDto?> RemoveAgpAsync(int contactId, int agpId, CancellationToken ct = default);
    Task<List<LTELetterFileInfoDto>?> GetLTELettersPagedAsync(int contactId, int pageNumber, CancellationToken ct = default);
    Task<List<LTELetterFileInfoDto>?> GetLTELettersPagedByFolderAsync(int contactId, int pageNumber, string folderName, CancellationToken ct = default);
    Task<List<LTELetterFileInfoDto>?> GetUnreadLTELetterFilesPagedAsync(int contactId, int pageNumber, CancellationToken ct = default);
    Task<List<LTELetterFileInfoDto>?> GetUnreadLTELetterFilesPagedByChildAsync(int contactId, int childId, int pageNumber, CancellationToken ct = default);
    Task<EnvelopeDto?> SendLetterAsync(LTELetterFileInfoDto letterInfo, CancellationToken ct = default);
    Task<EnvelopeDto?> SetLTELetterAsReadAsync(int contactId, int letterId, CancellationToken ct = default);
    Task<EnvelopeDto?> SubmitContactAsync(TransactionInfoDto transactionInfo, CancellationToken ct = default);
    Task<bool> CheckAddressAsync(ContactInfoDto contactInfo, CancellationToken ct = default);
    Task<List<LTELetterFileInfoDto>?> GetLTELettersAsync(int contactId, CancellationToken ct = default);
    Task<List<LTELetterFileInfoDto>?> GetLTELettersByChildPagedAsync(int contactId, int childId, int pageNumber, CancellationToken ct = default);
    Task<LTELetterInfoDto?> GetLetterFileFromDraftsAsync(int contactId, int letterId, CancellationToken ct = default);
    Task<EnvelopeDto?> SaveLetterToDraftsAsync(LTELetterFileInfoDto letterFileInfo, CancellationToken ct = default);
    Task<EnvelopeDto?> DeleteLetterFileAsync(int contactId, int letterId, CancellationToken ct = default);
    Task<List<DonationHistoryInfoDto>?> GetPaymentInfoAsync(int contactId, CancellationToken ct = default);
    Task<List<LTEOptInInfoDto>?> GetOptInBySponsorAsync(int contactId, CancellationToken ct = default);
    Task<List<LTELetterFolderInfoDto>?> GetLetterFoldersAsync(int contactId, CancellationToken ct = default);
    Task<PasswordResetInfoDto?> GetPasswordResetByIdAsync(string hash, CancellationToken ct = default);
    Task<bool> AddPasswordResetAsync(PasswordAndTransInfoCombinedDto passwordResetInfo, CancellationToken ct = default);
    Task<bool> UpdatePasswordResetAsync(PasswordResetInfoDto passwordResetInfo, CancellationToken ct = default);
    Task<bool> UnSubscribePreferencesAsync(string emailId, List<string> preferences, CancellationToken ct = default);
}
