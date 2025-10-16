using System.Text.Json.Serialization;

namespace ChildFund.Services.Models;

/// <summary>
/// Contact information DTO - extremely large model with many fields.
/// </summary>
public class ContactInfoDto
{
    // Underscore-prefixed properties (direct DataMember attributes)
    [JsonPropertyName("_acceptdffee")]
    public object? AcceptDfFee { get; set; }

    [JsonPropertyName("_amtlargestcontribution")]
    public int AmtLargestContribution { get; set; }

    [JsonPropertyName("_begindate")]
    public string? BeginDate { get; set; }

    [JsonPropertyName("_bypassncoa")]
    public string? BypassNcoa { get; set; }

    [JsonPropertyName("_cansponsorchildren")]
    public bool CanSponsorChildren { get; set; }

    [JsonPropertyName("_careof")]
    public string? CareOf { get; set; }

    [JsonPropertyName("_carrierroute")]
    public string? CarrierRoute { get; set; }

    [JsonPropertyName("_ccfindvid")]
    public int CcfIndvId { get; set; }

    [JsonPropertyName("_ccfindvsubshiftredirectid")]
    public int CcfIndvSubshiftRedirectId { get; set; }

    [JsonPropertyName("_cdesstatprovterrid")]
    public int CdesStatProvTerrId { get; set; }

    [JsonPropertyName("_city")]
    public string? City { get; set; }

    [JsonPropertyName("_clstnmsuffixid")]
    public int ClstnmSuffixId { get; set; }

    [JsonPropertyName("_clstnmtitleid")]
    public int ClstnmTitleId { get; set; }

    [JsonPropertyName("_contacctid")]
    public string? ContAcctId { get; set; }

    [JsonPropertyName("_contcorrespondencerepid")]
    public int ContCorrespondenceRepId { get; set; }

    [JsonPropertyName("_contfinancialrepid")]
    public int ContFinancialRepId { get; set; }

    [JsonPropertyName("_contmotivatedbyid")]
    public int ContMotivatedById { get; set; }

    [JsonPropertyName("_contreferredbyid")]
    public int ContReferredById { get; set; }

    [JsonPropertyName("_conttype")]
    public string? ContType { get; set; }

    [JsonPropertyName("_ctrycode")]
    public string? CtryCode { get; set; }

    [JsonPropertyName("_dateanniversary")]
    public object? DateAnniversary { get; set; }

    [JsonPropertyName("_datecreated")]
    public object? DateCreated { get; set; }

    [JsonPropertyName("_dateendstmtpull")]
    public object? DateEndStmtPull { get; set; }

    [JsonPropertyName("_datelastcontribution")]
    public object? DateLastContribution { get; set; }

    [JsonPropertyName("_datemodified")]
    public object? DateModified { get; set; }

    [JsonPropertyName("_datestartiocorrespond")]
    public object? DateStartIoCorrespond { get; set; }

    [JsonPropertyName("_datestartstmtpull")]
    public object? DateStartStmtPull { get; set; }

    [JsonPropertyName("_datestopiocorrespond")]
    public object? DateStopIoCorrespond { get; set; }

    [JsonPropertyName("_dateundesirable")]
    public object? DateUndesirable { get; set; }

    [JsonPropertyName("_deliverypoint")]
    public string? DeliveryPoint { get; set; }

    [JsonPropertyName("_email")]
    public string? Email { get; set; }

    [JsonPropertyName("_enddate")]
    public string? EndDate { get; set; }

    [JsonPropertyName("_envelopeline")]
    public string? EnvelopeLine { get; set; }

    [JsonPropertyName("_errormessage")]
    public string? ErrorMessage { get; set; }

    [JsonPropertyName("_extraaddress1")]
    public string? ExtraAddress1 { get; set; }

    [JsonPropertyName("_extraaddress2")]
    public string? ExtraAddress2 { get; set; }

    [JsonPropertyName("_extraaddress3")]
    public string? ExtraAddress3 { get; set; }

    [JsonPropertyName("_faxareacode")]
    public string? FaxAreaCode { get; set; }

    [JsonPropertyName("_firstname")]
    public string? FirstName { get; set; }

    [JsonPropertyName("_freqgrpid")]
    public int FreqGrpId { get; set; }

    [JsonPropertyName("_iaacctno")]
    public string? IaAcctNo { get; set; }

    [JsonPropertyName("_iaid")]
    public int IaId { get; set; }

    [JsonPropertyName("_id")]
    public int Id { get; set; }

    [JsonPropertyName("_indallowsponsorship")]
    public string? IndAllowSponsorship { get; set; }

    [JsonPropertyName("_indannualreport")]
    public string? IndAnnualReport { get; set; }

    [JsonPropertyName("_indanonymoussponsor")]
    public string? IndAnonymousSponsor { get; set; }

    [JsonPropertyName("_indbillingaddresssame")]
    public string? IndBillingAddressSame { get; set; }

    [JsonPropertyName("_indboardmember")]
    public string? IndBoardMember { get; set; }

    [JsonPropertyName("_indchildrenscirclenews")]
    public string? IndChildrensCircleNews { get; set; }

    [JsonPropertyName("_indchildworld")]
    public string? IndChildWorld { get; set; }

    [JsonPropertyName("_indcorinthian")]
    public string? IndCorinthian { get; set; }

    [JsonPropertyName("_inddeceased")]
    public string? IndDeceased { get; set; }

    [JsonPropertyName("_inddonotcall")]
    public string? IndDoNotCall { get; set; }

    [JsonPropertyName("_indincludeonannualreport")]
    public string? IndIncludeOnAnnualReport { get; set; }

    [JsonPropertyName("_indiocorrespond")]
    public string? IndIoCorrespond { get; set; }

    [JsonPropertyName("_indphonedayunpublished")]
    public string? IndPhoneDayUnpublished { get; set; }

    [JsonPropertyName("_indphoneunpublished")]
    public string? IndPhoneUnpublished { get; set; }

    [JsonPropertyName("_indpullstatement")]
    public string? IndPullStatement { get; set; }

    [JsonPropertyName("_indreceiveacks")]
    public string? IndReceiveAcks { get; set; }

    [JsonPropertyName("_indreminderupcomingdonation")]
    public string? IndReminderUpcomingDonation { get; set; }

    [JsonPropertyName("_indreturnedmail")]
    public string? IndReturnedMail { get; set; }

    [JsonPropertyName("_indstatement")]
    public string? IndStatement { get; set; }

    [JsonPropertyName("_indstuffer")]
    public string? IndStuffer { get; set; }

    [JsonPropertyName("_indsupervisorhandling")]
    public string? IndSupervisorHandling { get; set; }

    [JsonPropertyName("_indundesirable")]
    public string? IndUndesirable { get; set; }

    [JsonPropertyName("_indvip")]
    public string? IndVip { get; set; }

    [JsonPropertyName("_isdirty")]
    public bool IsDirty { get; set; }

    [JsonPropertyName("_isverifiedcontact")]
    public bool IsVerifiedContact { get; set; }

    [JsonPropertyName("_lastmodifiedby")]
    public string? LastModifiedBy { get; set; }

    [JsonPropertyName("_middlename")]
    public string? MiddleName { get; set; }

    [JsonPropertyName("_mktacid")]
    public int MktacId { get; set; }

    [JsonPropertyName("_modelscore")]
    public int ModelScore { get; set; }

    [JsonPropertyName("_name")]
    public string? Name { get; set; }

    [JsonPropertyName("_namesoundx")]
    public object? NameSoundx { get; set; }

    [JsonPropertyName("_oldaccountnbr")]
    public string? OldAccountNbr { get; set; }

    [JsonPropertyName("_orgtypid")]
    public int OrgTypId { get; set; }

    [JsonPropertyName("_origincode")]
    public string? OriginCode { get; set; }

    [JsonPropertyName("_phone")]
    public string? Phone { get; set; }

    [JsonPropertyName("_phoneareacode")]
    public string? PhoneAreaCode { get; set; }

    [JsonPropertyName("_phoneday")]
    public string? PhoneDay { get; set; }

    [JsonPropertyName("_phonedayareacode")]
    public string? PhoneDayAreaCode { get; set; }

    [JsonPropertyName("_phonedayextension")]
    public int PhoneDayExtension { get; set; }

    [JsonPropertyName("_phoneextension")]
    public int PhoneExtension { get; set; }

    [JsonPropertyName("_phonefax")]
    public string? PhoneFax { get; set; }

    [JsonPropertyName("_postalcode")]
    public string? PostalCode { get; set; }

    [JsonPropertyName("_prefcorrmethod")]
    public string? PrefCorrMethod { get; set; }

    [JsonPropertyName("_qtyactiveprespns")]
    public int QtyActivePreSpns { get; set; }

    [JsonPropertyName("_qtyactivespns")]
    public int QtyActiveSpns { get; set; }

    [JsonPropertyName("_qtydelinquencycanceledspns")]
    public int QtyDelinquencyCanceledSpns { get; set; }

    [JsonPropertyName("_qtyonholdspns")]
    public int QtyOnHoldSpns { get; set; }

    [JsonPropertyName("_qtyprespnscancels")]
    public int QtyPreSpnsCancels { get; set; }

    [JsonPropertyName("_qtyprespnsdrops")]
    public int QtyPreSpnsDrops { get; set; }

    [JsonPropertyName("_qtyrecentdelinquentcancels")]
    public int QtyRecentDelinquentCancels { get; set; }

    [JsonPropertyName("_qtyrequestedspnscancel")]
    public int QtyRequestedSpnsCancel { get; set; }

    [JsonPropertyName("_qtysubshiftpending")]
    public int QtySubShiftPending { get; set; }

    [JsonPropertyName("_salutation")]
    public string? Salutation { get; set; }

    [JsonPropertyName("_state")]
    public string? State { get; set; }

    [JsonPropertyName("_street1")]
    public string? Street1 { get; set; }

    [JsonPropertyName("_street2")]
    public string? Street2 { get; set; }

    [JsonPropertyName("_ucity")]
    public string? UCity { get; set; }

    [JsonPropertyName("_uextraaddress1")]
    public string? UExtraAddress1 { get; set; }

    [JsonPropertyName("_uextraaddress2")]
    public string? UExtraAddress2 { get; set; }

    [JsonPropertyName("_uextraaddress3")]
    public string? UExtraAddress3 { get; set; }

    [JsonPropertyName("_ufirstname")]
    public string? UFirstName { get; set; }

    [JsonPropertyName("_uname")]
    public string? UName { get; set; }

    [JsonPropertyName("_undesirablecomment")]
    public string? UndesirableComment { get; set; }

    [JsonPropertyName("_upostalcode")]
    public string? UPostalCode { get; set; }

    [JsonPropertyName("_urbanization")]
    public string? Urbanization { get; set; }

    [JsonPropertyName("_uspschangecode")]
    public string? UspsChangeCode { get; set; }

    [JsonPropertyName("_ustreet1")]
    public string? UStreet1 { get; set; }

    [JsonPropertyName("_ustreet2")]
    public string? UStreet2 { get; set; }

    [JsonPropertyName("_validateForSponsorship")]
    public bool ValidateForSponsorship { get; set; }

    // k__BackingField properties
    [JsonPropertyName("<GiftMessage>k__BackingField")]
    public string? GiftMessage { get; set; }

    [JsonPropertyName("<GiftOccasion>k__BackingField")]
    public string? GiftOccasion { get; set; }

    [JsonPropertyName("<GiftPaySchedId>k__BackingField")]
    public long GiftPaySchedId { get; set; }

    [JsonPropertyName("<GiftQuantity>k__BackingField")]
    public long GiftQuantity { get; set; }

    [JsonPropertyName("<GiftTitle>k__BackingField")]
    public string? GiftTitle { get; set; }

    [JsonPropertyName("<GiftTranType>k__BackingField")]
    public long GiftTranType { get; set; }

    [JsonPropertyName("<GiftTransactionId>k__BackingField")]
    public long GiftTransactionId { get; set; }

    [JsonPropertyName("<GiftUnitCost>k__BackingField")]
    public float GiftUnitCost { get; set; }

    [JsonPropertyName("<IsValidAddress>k__BackingField")]
    public bool IsValidAddress { get; set; }

    [JsonPropertyName("<doesAcceptTerms>k__BackingField")]
    public bool DoesAcceptTerms { get; set; }
}
