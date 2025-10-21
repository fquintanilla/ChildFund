using ChildFund.Services.Models;

namespace ChildFund.Web.Repositories;

/// <summary>
/// Repository for donor operations.
/// Provides access to donor, contact, and related data.
/// </summary>
public interface IDonorServiceRepository
{
    Task<List<ContactInfoDto>?> FindContacts(ContactInfoDto contactInfo, CancellationToken ct = default);
    Task<TransactionInfoDto?> GetContactById(int contactId, CancellationToken ct = default);
    Task<int> GetContactIdByEmail(string email, CancellationToken ct = default);
    Task<EnvelopeDto?> AddAgp(AgpInfoDto agpInfo, CancellationToken ct = default);
    Task<EnvelopeDto?> UpdateContact(ContactUpdateInfoDto contactUpdateInfo, CancellationToken ct = default);
    Task<TaxTotalInfoDto?> GetContactTaxTotals(int contactId, CancellationToken ct = default);
    Task<Dictionary<string, string>?> GetCSGStatementList(int contactId, CancellationToken ct = default);
    Task<byte[]?> GetCSGStatement(string statementId, CancellationToken ct = default);
    Task<List<EmailSubscriptionsInfoDto>?> GetEmailPublications(int contactId, CancellationToken ct = default);
    Task<bool> GetHandlingFee(int contactId, CancellationToken ct = default);
    Task<List<CodeInfoDto>?> GetHearAboutUs(CancellationToken ct = default);
    Task<int> GetLTELettersTotal(int contactId, string folderName, int childId, bool unreadOnly, CancellationToken ct = default);
    Task<EnvelopeDto?> ChangeEmailSubscriptions(int contactId, List<string> emailSubscriptions, CancellationToken ct = default);
    Task<List<SponsoredChildrenInfoDto>?> GetLTEChildrenByContactId(int contactId, CancellationToken ct = default);
    Task<LTELetterFileInfoDto?> GetLTELetterFileInfo(int contactId, int letterId, CancellationToken ct = default);
    Task<List<SponsoredChildrenInfoDto>?> GetSponsoredChildren(int contactId, CancellationToken ct = default);
    Task<List<SponsoredChildrenInfoDto>?> PaySetup(int contactId, CancellationToken ct = default);
    Task<string?> UpdateContactEmail(int contactId, string email, CancellationToken ct = default);
    Task<bool> UpdateHandlingFee(int contactId, bool acceptDfFee, CancellationToken ct = default);
    Task<bool> UpdateOptInByChild(int contactId, int noId, int childNumber, bool optIn, CancellationToken ct = default);
    Task<string?> GetBankName(int routingNumber, CancellationToken ct = default);
    Task<EnvelopeDto?> ReplaceAgp(int contactId, int oldPaymentId, int paymentId, CancellationToken ct = default);
    Task<EnvelopeDto?> UpdateAgp(AgpInfoDto agpInfo, CancellationToken ct = default);
    Task<EnvelopeDto?> RemoveAgp(int contactId, int agpId, CancellationToken ct = default);
    Task<List<LTELetterFileInfoDto>?> GetLTELettersPaged(int contactId, int pageNumber, CancellationToken ct = default);
    Task<List<LTELetterFileInfoDto>?> GetLTELettersPagedByFolder(int contactId, int pageNumber, string folderName, CancellationToken ct = default);
    Task<List<LTELetterFileInfoDto>?> GetUnreadLTELetterFilesPaged(int contactId, int pageNumber, CancellationToken ct = default);
    Task<List<LTELetterFileInfoDto>?> GetUnreadLTELetterFilesPagedByChild(int contactId, int childId, int pageNumber, CancellationToken ct = default);
    Task<EnvelopeDto?> SendLetter(LTELetterFileInfoDto letterInfo, CancellationToken ct = default);
    Task<EnvelopeDto?> SetLTELetterAsRead(int contactId, int letterId, CancellationToken ct = default);
    Task<EnvelopeDto?> SubmitContact(TransactionInfoDto transactionInfo, CancellationToken ct = default);
    Task<bool> CheckAddress(ContactInfoDto contactInfo, CancellationToken ct = default);
    Task<List<LTELetterFileInfoDto>?> GetLTELetters(int contactId, CancellationToken ct = default);
    Task<List<LTELetterFileInfoDto>?> GetLTELettersByChildPaged(int contactId, int childId, int pageNumber, CancellationToken ct = default);
    Task<LTELetterInfoDto?> GetLetterFileFromDrafts(int contactId, int letterId, CancellationToken ct = default);
    Task<EnvelopeDto?> SaveLetterToDrafts(LTELetterFileInfoDto letterFileInfo, CancellationToken ct = default);
    Task<EnvelopeDto?> DeleteLetterFile(int contactId, int letterId, CancellationToken ct = default);
    Task<List<DonationHistoryInfoDto>?> GetPaymentInfo(int contactId, CancellationToken ct = default);
    Task<List<LTEOptInInfoDto>?> GetOptInBySponsor(int contactId, CancellationToken ct = default);
    Task<List<LTELetterFolderInfoDto>?> GetLetterFolders(int contactId, CancellationToken ct = default);
    Task<PasswordResetInfoDto?> GetPasswordResetById(string hash, CancellationToken ct = default);
    Task<bool> AddPasswordReset(PasswordAndTransInfoCombinedDto passwordResetInfo, CancellationToken ct = default);
    Task<bool> UpdatePasswordReset(PasswordResetInfoDto passwordResetInfo, CancellationToken ct = default);
    Task<bool> UnSubscribePreferences(string emailId, List<string> preferences, CancellationToken ct = default);
}
