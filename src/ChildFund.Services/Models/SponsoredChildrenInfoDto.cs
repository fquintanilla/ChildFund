using System.Text.Json.Serialization;

namespace ChildFund.Services.Models
{
    public class SponsoredChildrenInfoDto
    {
        [JsonPropertyName("<AccountNum>k__BackingField")]
        public string? AccountNum { get; set; }

        [JsonPropertyName("<AcctType>k__BackingField")]
        public string? AcctType { get; set; }

        [JsonPropertyName("<Address>k__BackingField")]
        public string? Address { get; set; }

        [JsonPropertyName("<AgeMonth>k__BackingField")]
        public int? AgeMonth { get; set; }

        [JsonPropertyName("<AgeYear>k__BackingField")]
        public int? AgeYear { get; set; }

        [JsonPropertyName("<AgpId>k__BackingField")]
        public int? AgpId { get; set; }

        [JsonPropertyName("<AgpReadOnly>k__BackingField")]
        public int? AgpReadOnly { get; set; }

        [JsonPropertyName("<AgpType>k__BackingField")]
        public string? AgpType { get; set; }

        [JsonPropertyName("<Amount>k__BackingField")]
        public decimal? Amount { get; set; }

        [JsonPropertyName("<AmtDue>k__BackingField")]
        public decimal? AmtDue { get; set; }

        [JsonPropertyName("<Birthday>k__BackingField")]
        public string? Birthday { get; set; }

        [JsonPropertyName("<CardType>k__BackingField")]
        public string? CardType { get; set; }

        [JsonPropertyName("<CaseNbr>k__BackingField")]
        public long? CaseNbr { get; set; }

        [JsonPropertyName("<ChildName>k__BackingField")]
        public string? ChildName { get; set; }

        [JsonPropertyName("<ChildNbr>k__BackingField")]
        public long? ChildNbr { get; set; }

        [JsonPropertyName("<ChildPhoto>k__BackingField")]
        public string? ChildPhoto { get; set; }

        [JsonPropertyName("<ContId>k__BackingField")]
        public long? ContId { get; set; }

        [JsonPropertyName("<ContType>k__BackingField")]
        public string? ContType { get; set; }

        [JsonPropertyName("<CtryCode>k__BackingField")]
        public string? CountryCode { get; set; }

        [JsonPropertyName("<CtryDesc>k__BackingField")]
        public string? CountryDescription { get; set; }

        [JsonPropertyName("<Description>k__BackingField")]
        public string? Description { get; set; }

        [JsonPropertyName("<DueDate>k__BackingField")]
        public string? DueDate { get; set; }

        [JsonPropertyName("<FinCode>k__BackingField")]
        public int? FinCode { get; set; }

        [JsonPropertyName("<Freq>k__BackingField")]
        public string? Frequency { get; set; }

        [JsonPropertyName("<LTEOptIn>k__BackingField")]
        public bool? LteOptIn { get; set; }

        [JsonPropertyName("<MktacId>k__BackingField")]
        public int? MktacId { get; set; }

        [JsonPropertyName("<NoId>k__BackingField")]
        public int? NoId { get; set; }

        [JsonPropertyName("<PaIdThru>k__BackingField")]
        public string? PaIdThru { get; set; }

        [JsonPropertyName("<ProjDesc>k__BackingField")]
        public string? ProjectDescription { get; set; }

        [JsonPropertyName("<ProjId>k__BackingField")]
        public int? ProjectId { get; set; }

        [JsonPropertyName("<PymtId>k__BackingField")]
        public long? PaymentId { get; set; }

        [JsonPropertyName("<ShortName>k__BackingField")]
        public string? ShortName { get; set; }

        [JsonPropertyName("<SponsorshipType>k__BackingField")]
        public string? SponsorshipType { get; set; }

        [JsonPropertyName("<Status>k__BackingField")]
        public string? Status { get; set; }

        [JsonPropertyName("<birthdayMonth>k__BackingField")]
        public int? BirthdayMonth { get; set; }

        [JsonPropertyName("<sponsorshipId>k__BackingField")]
        public int? SponsorshipId { get; set; }
    }
}
