using ChildFund.Services.Interfaces;
using ChildFund.Services.Models;

namespace ChildFund.Web.Repositories;

/// <summary>
/// Repository for donor operations.
/// Provides access to donor, contact, and related data.
/// </summary>
public class DonorPortalServiceRepository : IDonorServiceRepository
{
    private readonly IDonorClient _donorClient;

    public DonorPortalServiceRepository(IDonorClient donorClient)
    {
        _donorClient = donorClient ?? throw new ArgumentNullException(nameof(donorClient));
    }

    public async Task<List<ContactInfoDto>?> FindContacts(ContactInfoDto contactInfo, CancellationToken ct = default) =>
        await _donorClient.FindContactsAsync(contactInfo, ct);

    public async Task<TransactionInfoDto?> GetContactById(int contactId, CancellationToken ct = default) =>
        await _donorClient.GetContactByIdAsync(contactId, ct);

    public Task<int> GetContactIdByEmail(string email, CancellationToken ct = default) =>
        _donorClient.GetContactIdByEmailAsync(email, ct);

    public async Task<EnvelopeDto?> AddAgp(AgpInfoDto agpInfo, CancellationToken ct = default) =>
        await _donorClient.AddAgpAsync(agpInfo, ct);

    public async Task<EnvelopeDto?> UpdateContact(ContactUpdateInfoDto contactUpdateInfo, CancellationToken ct = default) =>
        await _donorClient.UpdateContactAsync(contactUpdateInfo, ct);

    public async Task<TaxTotalInfoDto?> GetContactTaxTotals(int contactId, CancellationToken ct = default) =>
        await _donorClient.GetContactTaxTotalsAsync(contactId, ct);

    public async Task<Dictionary<string, string>?> GetCSGStatementList(int contactId, CancellationToken ct = default) =>
        await _donorClient.GetCSGStatementListAsync(contactId, ct);

    public Task<byte[]?> GetCSGStatement(string statementId, CancellationToken ct = default) =>
        _donorClient.GetCSGStatementAsync(statementId, ct);

    public async Task<List<EmailSubscriptionsInfoDto>?> GetEmailPublications(int contactId, CancellationToken ct = default) =>
        await _donorClient.GetEmailPublicationsAsync(contactId, ct);

    public Task<bool> GetHandlingFee(int contactId, CancellationToken ct = default) =>
        _donorClient.GetHandlingFeeAsync(contactId, ct);

    public async Task<List<CodeInfoDto>?> GetHearAboutUs(CancellationToken ct = default) =>
        await _donorClient.GetHearAboutUsAsync(ct);

    public Task<int> GetLTELettersTotal(int contactId, string folderName, int childId, bool unreadOnly, CancellationToken ct = default) =>
        _donorClient.GetLTELettersTotalAsync(contactId, folderName, childId, unreadOnly, ct);

    public async Task<EnvelopeDto?> ChangeEmailSubscriptions(int contactId, List<string> emailSubscriptions, CancellationToken ct = default) =>
        await _donorClient.ChangeEmailSubscriptionsAsync(contactId, emailSubscriptions, ct);

    public async Task<List<SponsoredChildrenInfoDto>?> GetLTEChildrenByContactId(int contactId, CancellationToken ct = default) =>
        await _donorClient.GetLTEChildrenByContactIdAsync(contactId, ct);

    public async Task<LTELetterFileInfoDto?> GetLTELetterFileInfo(int contactId, int letterId, CancellationToken ct = default) =>
        await _donorClient.GetLTELetterFileInfoAsync(contactId, letterId, ct);

    public async Task<List<SponsoredChildrenInfoDto>?> GetSponsoredChildren(int contactId, CancellationToken ct = default) =>
        await _donorClient.GetSponsoredChildrenAsync(contactId, ct);

    public async Task<List<SponsoredChildrenInfoDto>?> PaySetup(int contactId, CancellationToken ct = default) =>
        await _donorClient.PaySetupAsync(contactId, ct);

    public async Task<string?> UpdateContactEmail(int contactId, string email, CancellationToken ct = default) =>
        await _donorClient.UpdateContactEmailAsync(contactId, email, ct);

    public Task<bool> UpdateHandlingFee(int contactId, bool acceptDfFee, CancellationToken ct = default) =>
        _donorClient.UpdateHandlingFeeAsync(contactId, acceptDfFee, ct);

    public Task<bool> UpdateOptInByChild(int contactId, int noId, int childNumber, bool optIn, CancellationToken ct = default) =>
        _donorClient.UpdateOptInByChildAsync(contactId, noId, childNumber, optIn, ct);

    public Task<string?> GetBankName(int routingNumber, CancellationToken ct = default) =>
        _donorClient.GetBankNameAsync(routingNumber, ct);

    public async Task<EnvelopeDto?> ReplaceAgp(int contactId, int oldPaymentId, int paymentId, CancellationToken ct = default) =>
        await _donorClient.ReplaceAgpAsync(contactId, oldPaymentId, paymentId, ct);

    public async Task<EnvelopeDto?> UpdateAgp(AgpInfoDto agpInfo, CancellationToken ct = default) =>
        await _donorClient.UpdateAgpAsync(agpInfo, ct);

    public async Task<EnvelopeDto?> RemoveAgp(int contactId, int agpId, CancellationToken ct = default) =>
        await _donorClient.RemoveAgpAsync(contactId, agpId, ct);

    public async Task<List<LTELetterFileInfoDto>?> GetLTELettersPaged(int contactId, int pageNumber, CancellationToken ct = default) =>
        await _donorClient.GetLTELettersPagedAsync(contactId, pageNumber, ct);

    public async Task<List<LTELetterFileInfoDto>?> GetLTELettersPagedByFolder(int contactId, int pageNumber, string folderName, CancellationToken ct = default) =>
        await _donorClient.GetLTELettersPagedByFolderAsync(contactId, pageNumber, folderName, ct);

    public async Task<List<LTELetterFileInfoDto>?> GetUnreadLTELetterFilesPaged(int contactId, int pageNumber, CancellationToken ct = default) =>
        await _donorClient.GetUnreadLTELetterFilesPagedAsync(contactId, pageNumber, ct);

    public async Task<List<LTELetterFileInfoDto>?> GetUnreadLTELetterFilesPagedByChild(int contactId, int childId, int pageNumber, CancellationToken ct = default) =>
        await _donorClient.GetUnreadLTELetterFilesPagedByChildAsync(contactId, childId, pageNumber, ct);

    public async Task<EnvelopeDto?> SendLetter(LTELetterFileInfoDto letterInfo, CancellationToken ct = default) =>
        await _donorClient.SendLetterAsync(letterInfo, ct);

    public async Task<EnvelopeDto?> SetLTELetterAsRead(int contactId, int letterId, CancellationToken ct = default) =>
        await _donorClient.SetLTELetterAsReadAsync(contactId, letterId, ct);

    public async Task<EnvelopeDto?> SubmitContact(TransactionInfoDto transactionInfo, CancellationToken ct = default) =>
        await _donorClient.SubmitContactAsync(transactionInfo, ct);

    public Task<bool> CheckAddress(ContactInfoDto contactInfo, CancellationToken ct = default) =>
        _donorClient.CheckAddressAsync(contactInfo, ct);

    public async Task<List<LTELetterFileInfoDto>?> GetLTELetters(int contactId, CancellationToken ct = default) =>
        await _donorClient.GetLTELettersAsync(contactId, ct);

    public async Task<List<LTELetterFileInfoDto>?> GetLTELettersByChildPaged(int contactId, int childId, int pageNumber, CancellationToken ct = default) =>
        await _donorClient.GetLTELettersByChildPagedAsync(contactId, childId, pageNumber, ct);

    public async Task<LTELetterInfoDto?> GetLetterFileFromDrafts(int contactId, int letterId, CancellationToken ct = default) =>
        await _donorClient.GetLetterFileFromDraftsAsync(contactId, letterId, ct);

    public async Task<EnvelopeDto?> SaveLetterToDrafts(LTELetterFileInfoDto letterFileInfo, CancellationToken ct = default) =>
        await _donorClient.SaveLetterToDraftsAsync(letterFileInfo, ct);

    public async Task<EnvelopeDto?> DeleteLetterFile(int contactId, int letterId, CancellationToken ct = default) =>
        await _donorClient.DeleteLetterFileAsync(contactId, letterId, ct);

    public async Task<List<DonationHistoryInfoDto>?> GetPaymentInfo(int contactId, CancellationToken ct = default) =>
        await _donorClient.GetPaymentInfoAsync(contactId, ct);

    public async Task<List<LTEOptInInfoDto>?> GetOptInBySponsor(int contactId, CancellationToken ct = default) =>
        await _donorClient.GetOptInBySponsorAsync(contactId, ct);

    public async Task<List<LTELetterFolderInfoDto>?> GetLetterFolders(int contactId, CancellationToken ct = default) =>
        await _donorClient.GetLetterFoldersAsync(contactId, ct);

    public async Task<PasswordResetInfoDto?> GetPasswordResetById(string hash, CancellationToken ct = default) =>
        await _donorClient.GetPasswordResetByIdAsync(hash, ct);

    public Task<bool> AddPasswordReset(PasswordAndTransInfoCombinedDto passwordResetInfo, CancellationToken ct = default) =>
        _donorClient.AddPasswordResetAsync(passwordResetInfo, ct);

    public Task<bool> UpdatePasswordReset(PasswordResetInfoDto passwordResetInfo, CancellationToken ct = default) =>
        _donorClient.UpdatePasswordResetAsync(passwordResetInfo, ct);

    public Task<bool> UnSubscribePreferences(string emailId, List<string> preferences, CancellationToken ct = default) =>
        _donorClient.UnSubscribePreferencesAsync(emailId, preferences, ct);
}
