using System.Text.Json.Serialization;

namespace ChildFund.Services.Models
{
    public class TransactionInfoDto
    {
        [JsonPropertyName("<AdminContactId>k__BackingField")]
        public int? AdminContactId { get; set; }

        [JsonPropertyName("<AnniversaryDate>k__BackingField")]
        public string? AnniversaryDate { get; set; }

        [JsonPropertyName("<BankName>k__BackingField")]
        public string? BankName { get; set; }

        [JsonPropertyName("<BypassHouseHoldMatch>k__BackingField")]
        public bool? BypassHouseHoldMatch { get; set; }

        [JsonPropertyName("<CVV>k__BackingField")]
        public string? CVV { get; set; }

        [JsonPropertyName("<CanSponsorChildren>k__BackingField")]
        public bool? CanSponsorChildren { get; set; }

        [JsonPropertyName("<ContactErrorList>k__BackingField")]
        public List<string>? ContactErrorList { get; set; }

        [JsonPropertyName("<ContactMatchType>k__BackingField")]
        public string? ContactMatchType { get; set; }

        [JsonPropertyName("<CreateEktronAccount>k__BackingField")]
        public bool? CreateEktronAccount { get; set; }

        [JsonPropertyName("<DonorCareOf>k__BackingField")]
        public string? DonorCareOf { get; set; }

        [JsonPropertyName("<DonorCity>k__BackingField")]
        public string? DonorCity { get; set; }

        [JsonPropertyName("<DonorContactType>k__BackingField")]
        public string? DonorContactType { get; set; }

        [JsonPropertyName("<DonorCountryCode>k__BackingField")]
        public string? DonorCountryCode { get; set; }

        [JsonPropertyName("<DonorEmail>k__BackingField")]
        public string? DonorEmail { get; set; }

        [JsonPropertyName("<DonorFirstName>k__BackingField")]
        public string? DonorFirstName { get; set; }

        [JsonPropertyName("<DonorLastName>k__BackingField")]
        public string? DonorLastName { get; set; }

        [JsonPropertyName("<DonorId>k__BackingField")]
        public long? DonorId { get; set; }

        [JsonPropertyName("<DonorPhone>k__BackingField")]
        public string? DonorPhone { get; set; }

        [JsonPropertyName("<DonorPhoneAreaCode>k__BackingField")]
        public string? DonorPhoneAreaCode { get; set; }

        [JsonPropertyName("<DonorPostalCode>k__BackingField")]
        public string? DonorPostalCode { get; set; }

        [JsonPropertyName("<DonorState>k__BackingField")]
        public int? DonorState { get; set; }

        [JsonPropertyName("<DonorStreet1>k__BackingField")]
        public string? DonorStreet1 { get; set; }

        [JsonPropertyName("<DonorStreet2>k__BackingField")]
        public string? DonorStreet2 { get; set; }

        [JsonPropertyName("<EktronPassword>k__BackingField")]
        public string? EktronPassword { get; set; }

        [JsonPropertyName("<Errors>k__BackingField")]
        public DonorErrorDto? Errors { get; set; }

        [JsonPropertyName("<HasAGPs>k__BackingField")]
        public bool? HasAgps { get; set; }

        [JsonPropertyName("<IsAdmin>k__BackingField")]
        public bool? IsAdmin { get; set; }

        [JsonPropertyName("<IsAgpTransferred>k__BackingField")]
        public bool? IsAgpTransferred { get; set; }

        [JsonPropertyName("<IsDeactivatePaySched>k__BackingField")]
        public bool? IsDeactivatePaySched { get; set; }

        [JsonPropertyName("<IsEncrypted>k__BackingField")]
        public bool? IsEncrypted { get; set; }

        [JsonPropertyName("<IsGuest>k__BackingField")]
        public bool? IsGuest { get; set; }

        [JsonPropertyName("<IsImpersonate>k__BackingField")]
        public bool? IsImpersonate { get; set; }

        [JsonPropertyName("<PaymentAccountNumber>k__BackingField")]
        public string? PaymentAccountNumber { get; set; }

        [JsonPropertyName("<PaymentCardType>k__BackingField")]
        public string? PaymentCardType { get; set; }

        [JsonPropertyName("<PaymentCardholderFullName>k__BackingField")]
        public string? PaymentCardholderFullName { get; set; }

        [JsonPropertyName("<PaymentCardholderZipcode>k__BackingField")]
        public string? PaymentCardholderZipcode { get; set; }

        [JsonPropertyName("<PaymentDateDfTaken>k__BackingField")]
        public string? PaymentDateDfTaken { get; set; }

        [JsonPropertyName("<PaymentDateExpiration>k__BackingField")]
        public string? PaymentDateExpiration { get; set; }

        [JsonPropertyName("<PaymentDonorId>k__BackingField")]
        public long? PaymentDonorId { get; set; }

        [JsonPropertyName("<PaymentErrorList>k__BackingField")]
        public List<string>? PaymentErrorList { get; set; }

        [JsonPropertyName("<PaymentId>k__BackingField")]
        public long? PaymentId { get; set; }

        [JsonPropertyName("<PaymentTransitNumber>k__BackingField")]
        public string? PaymentTransitNumber { get; set; }

        [JsonPropertyName("<PaymentType>k__BackingField")]
        public string? PaymentType { get; set; }

        [JsonPropertyName("<ReturnMessage>k__BackingField")]
        public int? ReturnMessage { get; set; }

        [JsonPropertyName("<TransactionBetweenDate>k__BackingField")]
        public string? TransactionBetweenDate { get; set; }

        [JsonPropertyName("<TransactionCreateDate>k__BackingField")]
        public string? TransactionCreateDate { get; set; }

        [JsonPropertyName("<TransactionId>k__BackingField")]
        public long? TransactionId { get; set; }

        [JsonPropertyName("<TransactionModifiedDate>k__BackingField")]
        public string? TransactionModifiedDate { get; set; }

        [JsonPropertyName("<TransactionStatus>k__BackingField")]
        public string? TransactionStatus { get; set; }

        [JsonPropertyName("<TransactionSuccessful>k__BackingField")]
        public bool? TransactionSuccessful { get; set; }

        [JsonPropertyName("<TransactionType>k__BackingField")]
        public string? TransactionType { get; set; }

        [JsonPropertyName("<TransactionUserComments>k__BackingField")]
        public string? TransactionUserComments { get; set; }

        [JsonPropertyName("<TransactionUserCreate>k__BackingField")]
        public string? TransactionUserCreate { get; set; }

        [JsonPropertyName("<TransactionUserModified>k__BackingField")]
        public string? TransactionUserModified { get; set; }

        [JsonPropertyName("<doesAcceptACHTerms>k__BackingField")]
        public bool? DoesAcceptAchTerms { get; set; }

        [JsonPropertyName("<doesAcceptDfFee>k__BackingField")]
        public bool? DoesAcceptDfFee { get; set; }

        [JsonPropertyName("<doesAcceptTerms>k__BackingField")]
        public bool? DoesAcceptTerms { get; set; }

        [JsonPropertyName("<isDirtyDonor>k__BackingField")]
        public bool? IsDirtyDonor { get; set; }

        [JsonPropertyName("<isDirtyPayment>k__BackingField")]
        public bool? IsDirtyPayment { get; set; }

        [JsonPropertyName("<isReactivating>k__BackingField")]
        public bool? IsReactivating { get; set; }

        [JsonPropertyName("<isVerifiedDonations>k__BackingField")]
        public bool? IsVerifiedDonations { get; set; }

        [JsonPropertyName("<isVerifiedDonor>k__BackingField")]
        public bool? IsVerifiedDonor { get; set; }

        [JsonPropertyName("<isVerifiedPayment>k__BackingField")]
        public bool? IsVerifiedPayment { get; set; }

        [JsonPropertyName("<validateForSponsorship>k__BackingField")]
        public bool? ValidateForSponsorship { get; set; }
    }
}
