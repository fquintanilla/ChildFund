using ChildFund.Services.Interfaces;
using ChildFund.Services.Models;
using ChildFund.Services.Providers;
using ChildFund.Services.Serialization;
using Microsoft.Extensions.Options;

namespace ChildFund.Services.ApiClients;

/// <summary>
/// Client for interacting with the ChildFund API - Donor Service.
/// </summary>
public sealed class DonorPortalClient : ChildFundApiClient, IDonorClient
{
    public DonorPortalClient(
        HttpClient http,
        ITokenProvider tokenProvider,
        IOptions<ChildFundApiOptions> options)
        : base(http, tokenProvider, options)
    {
    }

    public Task<List<ContactInfoDto>?> FindContactsAsync(ContactInfoDto contactInfo, CancellationToken ct = default) =>
        PostAsync<List<ContactInfoDto>?>("DonorPortal/FindContacts", contactInfo, JsonDefaults.Options, ct);

    public Task<TransactionInfoDto?> GetContactByIdAsync(int contactId, CancellationToken ct = default) =>
        GetAsync<TransactionInfoDto?>($"DonorPortal/GetContactById/{contactId}", JsonDefaults.Options, ct);

    public async Task<int> GetContactIdByEmailAsync(string email, CancellationToken ct = default)
    {
        using var response = await GetResponseAsync($"DonorPortal/GetContactIdByEmail?email={Uri.EscapeDataString(email)}", ct);
        var content = await response.Content.ReadAsStringAsync(ct);
        return int.TryParse(content, out var result) ? result : 0;
    }

    public Task<EnvelopeDto?> AddAgpAsync(AgpInfoDto agpInfo, CancellationToken ct = default) =>
        PostAsync<EnvelopeDto?>("DonorPortal/AddAgp", agpInfo, JsonDefaults.Options, ct);

    public Task<EnvelopeDto?> UpdateContactAsync(ContactUpdateInfoDto contactUpdateInfo, CancellationToken ct = default) =>
        PostAsync<EnvelopeDto?>("DonorPortal/UpdateContact", contactUpdateInfo, JsonDefaults.Options, ct);

    public Task<TaxTotalInfoDto?> GetContactTaxTotalsAsync(int contactId, CancellationToken ct = default) =>
        GetAsync<TaxTotalInfoDto?>($"DonorPortal/GetContactTaxTotals/{contactId}", JsonDefaults.Options, ct);

    public Task<Dictionary<string, string>?> GetCSGStatementListAsync(int contactId, CancellationToken ct = default) =>
        GetAsync<Dictionary<string, string>?>($"DonorPortal/GetCSGStatementList/{contactId}", JsonDefaults.Options, ct);

    public async Task<byte[]?> GetCSGStatementAsync(string statementId, CancellationToken ct = default)
    {
        using var response = await GetResponseAsync($"DonorPortal/GetCSGStatement?statementId={Uri.EscapeDataString(statementId)}", ct);
        return await response.Content.ReadAsByteArrayAsync(ct);
    }

    public Task<List<EmailSubscriptionsInfoDto>?> GetEmailPublicationsAsync(int contactId, CancellationToken ct = default) =>
        GetAsync<List<EmailSubscriptionsInfoDto>?>($"DonorPortal/GetEmailPublications/{contactId}", JsonDefaults.Options, ct);

    public async Task<bool> GetHandlingFeeAsync(int contactId, CancellationToken ct = default)
    {
        using var response = await GetResponseAsync($"DonorPortal/GetHandlingFee/{contactId}", ct);
        var content = await response.Content.ReadAsStringAsync(ct);
        return bool.TryParse(content, out var result) && result;
    }

    public Task<List<CodeInfoDto>?> GetHearAboutUsAsync(CancellationToken ct = default) =>
        GetAsync<List<CodeInfoDto>?>("DonorPortal/GetHearAboutUs", JsonDefaults.Options, ct);

    public async Task<int> GetLTELettersTotalAsync(int contactId, string folderName, int childId, bool unreadOnly, CancellationToken ct = default)
    {
        var url = $"DonorPortal/GetLTELettersTotal/{contactId}?folderName={Uri.EscapeDataString(folderName)}&childId={childId}&unreadOnly={unreadOnly}";
        using var response = await GetResponseAsync(url, ct);
        var content = await response.Content.ReadAsStringAsync(ct);
        return int.TryParse(content, out var result) ? result : 0;
    }

    public Task<EnvelopeDto?> ChangeEmailSubscriptionsAsync(int contactId, List<string> emailSubscriptions, CancellationToken ct = default) =>
        PostAsync<EnvelopeDto?>($"DonorPortal/ChangeEmailSubscriptions?contactId={contactId}", emailSubscriptions, JsonDefaults.Options, ct);

    public Task<List<SponsoredChildrenInfoDto>?> GetLTEChildrenByContactIdAsync(int contactId, CancellationToken ct = default) =>
        GetAsync<List<SponsoredChildrenInfoDto>?>($"DonorPortal/GetLTEChildrenByContactID/{contactId}", JsonDefaults.Options, ct);

    public Task<LTELetterFileInfoDto?> GetLTELetterFileInfoAsync(int contactId, int letterId, CancellationToken ct = default) =>
        GetAsync<LTELetterFileInfoDto?>($"DonorPortal/GetLTELetterFileInfo?contactId={contactId}&letterId={letterId}", JsonDefaults.Options, ct);

    public Task<List<SponsoredChildrenInfoDto>?> GetSponsoredChildrenAsync(int contactId, CancellationToken ct = default) =>
        GetAsync<List<SponsoredChildrenInfoDto>?>($"DonorPortal/GetSponsoredChildren/{contactId}", JsonDefaults.Options, ct);

    public Task<List<SponsoredChildrenInfoDto>?> PaySetupAsync(int contactId, CancellationToken ct = default) =>
        GetAsync<List<SponsoredChildrenInfoDto>?>($"DonorPortal/PaySetup/{contactId}", JsonDefaults.Options, ct);

    public async Task<string?> UpdateContactEmailAsync(int contactId, string email, CancellationToken ct = default)
    {
        using var response = await PostResponseAsync($"DonorPortal/UpdateContactEmail/{contactId}?email={Uri.EscapeDataString(email)}", null, null, ct);
        return await response.Content.ReadAsStringAsync(ct);
    }

    public async Task<bool> UpdateHandlingFeeAsync(int contactId, bool acceptDfFee, CancellationToken ct = default)
    {
        using var response = await PostResponseAsync($"DonorPortal/UpdateHandlingFee/{contactId}?acceptDfFee={acceptDfFee}", null, null, ct);
        var content = await response.Content.ReadAsStringAsync(ct);
        return bool.TryParse(content, out var result) && result;
    }

    public async Task<bool> UpdateOptInByChildAsync(int contactId, int noId, int childNumber, bool optIn, CancellationToken ct = default)
    {
        var url = $"DonorPortal/UpdateOptInByChild/{contactId}?noId={noId}&childNumber={childNumber}&optIn={optIn}";
        using var response = await PostResponseAsync(url, null, null, ct);
        var content = await response.Content.ReadAsStringAsync(ct);
        return bool.TryParse(content, out var result) && result;
    }

    public async Task<string?> GetBankNameAsync(int routingNumber, CancellationToken ct = default)
    {
        using var response = await GetResponseAsync($"DonorPortal/GetBankName?routingNumber={routingNumber}", ct);
        return await response.Content.ReadAsStringAsync(ct);
    }

    public Task<EnvelopeDto?> ReplaceAgpAsync(int contactId, int oldPaymentId, int paymentId, CancellationToken ct = default) =>
        PostAsync<EnvelopeDto?>($"DonorPortal/ReplaceAgp/{contactId}?oldPaymentId={oldPaymentId}&paymentId={paymentId}", null, JsonDefaults.Options, ct);

    public Task<EnvelopeDto?> UpdateAgpAsync(AgpInfoDto agpInfo, CancellationToken ct = default) =>
        PostAsync<EnvelopeDto?>("DonorPortal/UpdateAgp", agpInfo, JsonDefaults.Options, ct);

    public Task<EnvelopeDto?> RemoveAgpAsync(int contactId, int agpId, CancellationToken ct = default) =>
        PostAsync<EnvelopeDto?>($"DonorPortal/RemoveAgp/{contactId}?agpId={agpId}", null, JsonDefaults.Options, ct);

    public Task<List<LTELetterFileInfoDto>?> GetLTELettersPagedAsync(int contactId, int pageNumber, CancellationToken ct = default) =>
        GetAsync<List<LTELetterFileInfoDto>?>($"DonorPortal/GetLTELettersPaged/{contactId}?pageNumber={pageNumber}", JsonDefaults.Options, ct);

    public Task<List<LTELetterFileInfoDto>?> GetLTELettersPagedByFolderAsync(int contactId, int pageNumber, string folderName, CancellationToken ct = default) =>
        GetAsync<List<LTELetterFileInfoDto>?>($"DonorPortal/GetLTELettersPagedByFolder/{contactId}?pageNumber={pageNumber}&folderName={Uri.EscapeDataString(folderName)}", JsonDefaults.Options, ct);

    public Task<List<LTELetterFileInfoDto>?> GetUnreadLTELetterFilesPagedAsync(int contactId, int pageNumber, CancellationToken ct = default) =>
        GetAsync<List<LTELetterFileInfoDto>?>($"DonorPortal/GetUnreadLTELetterFilesPaged/{contactId}?pageNumber={pageNumber}", JsonDefaults.Options, ct);

    public Task<List<LTELetterFileInfoDto>?> GetUnreadLTELetterFilesPagedByChildAsync(int contactId, int childId, int pageNumber, CancellationToken ct = default) =>
        GetAsync<List<LTELetterFileInfoDto>?>($"DonorPortal/GetUnreadLTELetterFilesPagedByChild/{contactId}?childId={childId}&pageNumber={pageNumber}", JsonDefaults.Options, ct);

    public Task<EnvelopeDto?> SendLetterAsync(LTELetterFileInfoDto letterInfo, CancellationToken ct = default) =>
        PostAsync<EnvelopeDto?>("DonorPortal/SendLetter", letterInfo, JsonDefaults.Options, ct);

    public Task<EnvelopeDto?> SetLTELetterAsReadAsync(int contactId, int letterId, CancellationToken ct = default) =>
        PostAsync<EnvelopeDto?>($"DonorPortal/SetLTELetterAsRead/{contactId}?letterId={letterId}", null, JsonDefaults.Options, ct);

    public Task<EnvelopeDto?> SubmitContactAsync(TransactionInfoDto transactionInfo, CancellationToken ct = default) =>
        PostAsync<EnvelopeDto?>("DonorPortal/SubmitContact", transactionInfo, JsonDefaults.Options, ct);

    public async Task<bool> CheckAddressAsync(ContactInfoDto contactInfo, CancellationToken ct = default)
    {
        using var response = await PostResponseAsync("DonorPortal/CheckAddress", contactInfo, JsonDefaults.Options, ct);
        var content = await response.Content.ReadAsStringAsync(ct);
        return bool.TryParse(content, out var result) && result;
    }

    public Task<List<LTELetterFileInfoDto>?> GetLTELettersAsync(int contactId, CancellationToken ct = default) =>
        GetAsync<List<LTELetterFileInfoDto>?>($"DonorPortal/GetLTELetters/{contactId}", JsonDefaults.Options, ct);

    public Task<List<LTELetterFileInfoDto>?> GetLTELettersByChildPagedAsync(int contactId, int childId, int pageNumber, CancellationToken ct = default) =>
        GetAsync<List<LTELetterFileInfoDto>?>($"DonorPortal/GetLTELettersByChildPaged/{contactId}?childId={childId}&pageNumber={pageNumber}", JsonDefaults.Options, ct);

    public Task<LTELetterInfoDto?> GetLetterFileFromDraftsAsync(int contactId, int letterId, CancellationToken ct = default) =>
        GetAsync<LTELetterInfoDto?>($"DonorPortal/GetLetterFileFromDrafts/{contactId}?letterId={letterId}", JsonDefaults.Options, ct);

    public Task<EnvelopeDto?> SaveLetterToDraftsAsync(LTELetterFileInfoDto letterFileInfo, CancellationToken ct = default) =>
        PostAsync<EnvelopeDto?>("DonorPortal/SaveLetterToDrafts", letterFileInfo, JsonDefaults.Options, ct);

    public Task<EnvelopeDto?> DeleteLetterFileAsync(int contactId, int letterId, CancellationToken ct = default) =>
        PostAsync<EnvelopeDto?>($"DonorPortal/DeleteLetterFile/{contactId}?letterId={letterId}", null, JsonDefaults.Options, ct);

    public Task<List<DonationHistoryInfoDto>?> GetPaymentInfoAsync(int contactId, CancellationToken ct = default) =>
        GetAsync<List<DonationHistoryInfoDto>?>($"DonorPortal/GetPaymentInfo/{contactId}", JsonDefaults.Options, ct);

    public Task<List<LTEOptInInfoDto>?> GetOptInBySponsorAsync(int contactId, CancellationToken ct = default) =>
        GetAsync<List<LTEOptInInfoDto>?>($"DonorPortal/GetOptInBySponsor/{contactId}", JsonDefaults.Options, ct);

    public Task<List<LTELetterFolderInfoDto>?> GetLetterFoldersAsync(int contactId, CancellationToken ct = default) =>
        GetAsync<List<LTELetterFolderInfoDto>?>($"DonorPortal/GetLetterFolders/{contactId}", JsonDefaults.Options, ct);

    public Task<PasswordResetInfoDto?> GetPasswordResetByIdAsync(string hash, CancellationToken ct = default) =>
        GetAsync<PasswordResetInfoDto?>($"DonorPortal/GetPasswordResetByID?hash={Uri.EscapeDataString(hash)}", JsonDefaults.Options, ct);

    public async Task<bool> AddPasswordResetAsync(PasswordAndTransInfoCombinedDto passwordResetInfo, CancellationToken ct = default)
    {
        using var response = await PostResponseAsync("DonorPortal/AddPasswordReset", passwordResetInfo, JsonDefaults.Options, ct);
        var content = await response.Content.ReadAsStringAsync(ct);
        return bool.TryParse(content, out var result) && result;
    }

    public async Task<bool> UpdatePasswordResetAsync(PasswordResetInfoDto passwordResetInfo, CancellationToken ct = default)
    {
        using var response = await PostResponseAsync("DonorPortal/UpdatePasswordReset", passwordResetInfo, JsonDefaults.Options, ct);
        var content = await response.Content.ReadAsStringAsync(ct);
        return bool.TryParse(content, out var result) && result;
    }

    public async Task<bool> UnSubscribePreferencesAsync(string emailId, List<string> preferences, CancellationToken ct = default)
    {
        using var response = await PostResponseAsync($"DonorPortal/UnSubscribePreferences?emailId={Uri.EscapeDataString(emailId)}", preferences, JsonDefaults.Options, ct);
        var content = await response.Content.ReadAsStringAsync(ct);
        return bool.TryParse(content, out var result) && result;
    }
}
