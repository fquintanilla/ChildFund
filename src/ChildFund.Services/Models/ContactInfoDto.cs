using System.Text.Json.Serialization;

namespace ChildFund.Services.Models
{
    public class ContactInfoDto
    {
        [JsonPropertyName("_acceptdffee")]
        public string? AcceptDfFee { get; set; }

        [JsonPropertyName("_amtlargestcontribution")]
        public decimal? AmtLargestContribution { get; set; }

        [JsonPropertyName("_begindate")]
        public DateTime? BeginDate { get; set; }

        [JsonPropertyName("_bypassncoa")]
        public bool? BypassNcoa { get; set; }

        [JsonPropertyName("_cansponsorchildren")]
        public bool CanSponsorChildren { get; set; }

        [JsonPropertyName("_careof")]
        public string? CareOf { get; set; }

        [JsonPropertyName("_carrierroute")]
        public string? CarrierRoute { get; set; }

        [JsonPropertyName("_ccfindvid")]
        public int? CcFindVid { get; set; }

        [JsonPropertyName("_ccfindvsubshiftredirectid")]
        public int? CcFindvSubShiftRedirectId { get; set; }

        [JsonPropertyName("_cdesstatprovterrid")]
        public int? StateProvinceTerritoryId { get; set; }

        [JsonPropertyName("_city")]
        public string? City { get; set; }

        [JsonPropertyName("_clstnmsuffixid")]
        public int? LastNameSuffixId { get; set; }

        [JsonPropertyName("_clstnmtitleid")]
        public int? LastNameTitleId { get; set; }

        [JsonPropertyName("_conttype")]
        public string? ContactType { get; set; }

        [JsonPropertyName("_ctrycode")]
        public string? CountryCode { get; set; }

        [JsonPropertyName("_dateanniversary")]
        public DateTime? DateAnniversary { get; set; }

        [JsonPropertyName("_datecreated")]
        public DateTime? DateCreated { get; set; }

        [JsonPropertyName("_datemodified")]
        public DateTime? DateModified { get; set; }

        [JsonPropertyName("_email")]
        public string? Email { get; set; }

        [JsonPropertyName("_envelopeline")]
        public string? EnvelopeLine { get; set; }

        [JsonPropertyName("_firstname")]
        public string? FirstName { get; set; }

        [JsonPropertyName("_iaid")]
        public int? IaId { get; set; }

        [JsonPropertyName("_id")]
        public long? Id { get; set; }

        [JsonPropertyName("_indallowsponsorship")]
        public string? IndAllowSponsorship { get; set; }

        [JsonPropertyName("_indboardmember")]
        public string? IndBoardMember { get; set; }

        [JsonPropertyName("_inddeceased")]
        public string? IndDeceased { get; set; }

        [JsonPropertyName("_indundesirable")]
        public string? IndUndesirable { get; set; }

        [JsonPropertyName("_indvip")]
        public string? IndVip { get; set; }

        [JsonPropertyName("_isdirty")]
        public bool? IsDirty { get; set; }

        [JsonPropertyName("_isverifiedcontact")]
        public bool? IsVerifiedContact { get; set; }

        [JsonPropertyName("_lastmodifiedby")]
        public string? LastModifiedBy { get; set; }

        [JsonPropertyName("_middlename")]
        public string? MiddleName { get; set; }

        [JsonPropertyName("_name")]
        public string? Name { get; set; }

        [JsonPropertyName("_phone")]
        public string? Phone { get; set; }

        [JsonPropertyName("_phoneareacode")]
        public string? PhoneAreaCode { get; set; }

        [JsonPropertyName("_postalcode")]
        public string? PostalCode { get; set; }

        [JsonPropertyName("_salutation")]
        public string? Salutation { get; set; }

        [JsonPropertyName("_state")]
        public string? State { get; set; }

        [JsonPropertyName("_street1")]
        public string? Street1 { get; set; }

        [JsonPropertyName("_street2")]
        public string? Street2 { get; set; }

        [JsonPropertyName("_validateForSponsorship")]
        public bool ValidateForSponsorship { get; set; }

        // Gift-related fields
        [JsonPropertyName("<GiftMessage>k__BackingField")]
        public string? GiftMessage { get; set; }

        [JsonPropertyName("<GiftOccasion>k__BackingField")]
        public string? GiftOccasion { get; set; }

        [JsonPropertyName("<GiftPaySchedId>k__BackingField")]
        public int? GiftPaySchedId { get; set; }

        [JsonPropertyName("<GiftQuantity>k__BackingField")]
        public int? GiftQuantity { get; set; }

        [JsonPropertyName("<GiftTitle>k__BackingField")]
        public string? GiftTitle { get; set; }

        [JsonPropertyName("<GiftTranType>k__BackingField")]
        public int? GiftTranType { get; set; }

        [JsonPropertyName("<GiftTransactionId>k__BackingField")]
        public long? GiftTransactionId { get; set; }

        [JsonPropertyName("<GiftUnitCost>k__BackingField")]
        public decimal? GiftUnitCost { get; set; }

        [JsonPropertyName("<IsValidAddress>k__BackingField")]
        public bool IsValidAddress { get; set; }

        [JsonPropertyName("<doesAcceptTerms>k__BackingField")]
        public bool DoesAcceptTerms { get; set; }
    }
}
