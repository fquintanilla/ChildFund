using ChildFund.Services.Interfaces;
using ChildFund.Services.Models;
using ChildFund.Web.Infrastructure.Cms.Services;

namespace ChildFund.Web.Repositories;

/// <summary>
/// Repository for donor operations.
/// Provides access to donor, contact, and related data.
/// </summary>
public class DonorPortalServiceRepository : IDonorServiceRepository
{
    private readonly IDonorClient _donorClient;
    private readonly ICacheService _cache;

    #region CacheKeys
    private const string ContactIdByEmailCacheKeyPrefix = "ChildFund:Donor:ContactIdByEmail:v1:";
    private const string ContactTaxTotalsCacheKeyPrefix = "ChildFund:Donor:TaxTotals:v1:";
    private const string CSGStatementListCacheKeyPrefix = "ChildFund:Donor:CSGStatementList:v1:";
    private const string CSGStatementCacheKeyPrefix = "ChildFund:Donor:CSGStatement:v1:";
    private const string HearAboutUsCacheKey = "ChildFund:Donor:HearAboutUs:v1";
    private const string BankNameCacheKeyPrefix = "ChildFund:Donor:BankName:v1:";
    #endregion

    #region CacheDurations
    private const int ContactDataCacheDurationSeconds = 900; // 15 minutes for contact-specific data
    private const int ReferenceDataCacheDurationSeconds = 3600; // 1 hour for reference/lookup data
    #endregion

    public DonorPortalServiceRepository(
        IDonorClient donorClient,
        ICacheService cache)
    {
        _donorClient = donorClient ?? throw new ArgumentNullException(nameof(donorClient));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public async Task<List<ContactInfoDto>?> FindContacts(ContactInfoDto contactInfo, CancellationToken ct = default) =>
        await _donorClient.FindContactsAsync(contactInfo, ct);

    public async Task<TransactionInfoDto?> GetContactById(int contactId, CancellationToken ct = default) =>
        await _donorClient.GetContactByIdAsync(contactId, ct);

    public async Task<int> GetContactIdByEmail(string email, CancellationToken ct = default)
    {
        string cacheKey = $"{ContactIdByEmailCacheKeyPrefix}{email.ToLowerInvariant()}";
        if (_cache.Exists(cacheKey))
            return _cache.Get<int>(cacheKey);
        var contactId = await _donorClient.GetContactIdByEmailAsync(email, ct);
        if (contactId > 0)
            _cache.AddBySeconds(cacheKey, contactId, ContactDataCacheDurationSeconds);
        return contactId;
    }

    public async Task<EnvelopeDto?> AddAgp(AgpInfoDto agpInfo, CancellationToken ct = default) =>
        await _donorClient.AddAgpAsync(agpInfo, ct);

    public async Task<EnvelopeDto?> UpdateContact(ContactUpdateInfoDto contactUpdateInfo, CancellationToken ct = default) =>
        await _donorClient.UpdateContactAsync(contactUpdateInfo, ct);

    public async Task<TaxTotalInfoDto?> GetContactTaxTotals(int contactId, CancellationToken ct = default)
    {
        string cacheKey = $"{ContactTaxTotalsCacheKeyPrefix}{contactId}";
        if (_cache.Exists(cacheKey))
            return _cache.Get<TaxTotalInfoDto>(cacheKey);
        var taxTotals = await _donorClient.GetContactTaxTotalsAsync(contactId, ct);
        if (taxTotals != null)
            _cache.AddBySeconds(cacheKey, taxTotals, ContactDataCacheDurationSeconds);
        return taxTotals;
    }

    public async Task<Dictionary<string, string>?> GetCSGStatementList(int contactId, CancellationToken ct = default)
    {
        string cacheKey = $"{CSGStatementListCacheKeyPrefix}{contactId}";
        if (_cache.Exists(cacheKey))
            return _cache.Get<Dictionary<string, string>>(cacheKey);
        var statementList = await _donorClient.GetCSGStatementListAsync(contactId, ct);
        if (statementList != null)
            _cache.AddBySeconds(cacheKey, statementList, ContactDataCacheDurationSeconds);
        return statementList;
    }

    public async Task<byte[]?> GetCSGStatement(string statementId, CancellationToken ct = default)
    {
        string cacheKey = $"{CSGStatementCacheKeyPrefix}{statementId}";
        if (_cache.Exists(cacheKey))
            return _cache.Get<byte[]>(cacheKey);
        var statementData = await _donorClient.GetCSGStatementAsync(statementId, ct);
        if (statementData != null)
            _cache.AddBySeconds(cacheKey, statementData, ContactDataCacheDurationSeconds);
        return statementData;
    }

    public async Task<List<EmailSubscriptionsInfoDto>?> GetEmailPublications(int contactId, CancellationToken ct = default) =>
        await _donorClient.GetEmailPublicationsAsync(contactId, ct);

    public Task<bool> GetHandlingFee(int contactId, CancellationToken ct = default) =>
        _donorClient.GetHandlingFeeAsync(contactId, ct);

    public async Task<List<CodeInfoDto>?> GetHearAboutUs(CancellationToken ct = default)
    {
        if (_cache.Exists(HearAboutUsCacheKey))
            return _cache.Get<List<CodeInfoDto>>(HearAboutUsCacheKey);
        var hearAboutUsList = await _donorClient.GetHearAboutUsAsync(ct);
        if (hearAboutUsList != null)
            _cache.AddBySeconds(HearAboutUsCacheKey, hearAboutUsList, ReferenceDataCacheDurationSeconds);
        return hearAboutUsList;
    }

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

    public async Task<string?> GetBankName(int routingNumber, CancellationToken ct = default)
    {
        string cacheKey = $"{BankNameCacheKeyPrefix}{routingNumber}";
        if (_cache.Exists(cacheKey))
            return _cache.Get<string>(cacheKey);
        var bankName = await _donorClient.GetBankNameAsync(routingNumber, ct);
        if (!string.IsNullOrEmpty(bankName))
            _cache.AddBySeconds(cacheKey, bankName, ReferenceDataCacheDurationSeconds);
        return bankName;
    }

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
