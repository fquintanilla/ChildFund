using System.Text.Json.Serialization;

namespace ChildFund.Services.Models;

/// <summary>
/// Donation information DTO.
/// </summary>
public class DonationInfoDto
{
    [JsonPropertyName("<Amount>k__BackingField")]
    public float Amount { get; set; }

    [JsonPropertyName("<CartQuantity>k__BackingField")]
    public int CartQuantity { get; set; }

    [JsonPropertyName("<ChildNumber>k__BackingField")]
    public int ChildNumber { get; set; }

    [JsonPropertyName("<Contact>k__BackingField")]
    public ContactInfoDto? Contact { get; set; }

    [JsonPropertyName("<DFEffectiveDateType>k__BackingField")]
    public EnumsEffDateType DFEffectiveDateType { get; set; }

    [JsonPropertyName("<DFMessage>k__BackingField")]
    public string? DFMessage { get; set; }

    [JsonPropertyName("<DeactivatePaySched>k__BackingField")]
    public bool DeactivatePaySched { get; set; }

    [JsonPropertyName("<EmailSubscriptions>k__BackingField")]
    public string[]? EmailSubscriptions { get; set; }

    [JsonPropertyName("<ErrorList>k__BackingField")]
    public string[]? ErrorList { get; set; }

    [JsonPropertyName("<FinCode>k__BackingField")]
    public int FinCode { get; set; }

    [JsonPropertyName("<HasAnchor>k__BackingField")]
    public bool HasAnchor { get; set; }

    [JsonPropertyName("<NationalOffice>k__BackingField")]
    public int NationalOffice { get; set; }

    [JsonPropertyName("<PaySchedId>k__BackingField")]
    public int PaySchedId { get; set; }

    [JsonPropertyName("<PaymentFrequency>k__BackingField")]
    public string? PaymentFrequency { get; set; }

    [JsonPropertyName("<ProjectId>k__BackingField")]
    public int ProjectId { get; set; }

    [JsonPropertyName("<SponsorshipId>k__BackingField")]
    public int SponsorshipId { get; set; }

    [JsonPropertyName("<Title>k__BackingField")]
    public string? Title { get; set; }

    [JsonPropertyName("<TransType>k__BackingField")]
    public int TransType { get; set; }

    [JsonPropertyName("<TransferAGP>k__BackingField")]
    public bool TransferAGP { get; set; }

    [JsonPropertyName("<isVerified>k__BackingField")]
    public bool IsVerified { get; set; }

    [JsonPropertyName("<mktacid>k__BackingField")]
    public int Mktacid { get; set; }
}

/// <summary>
/// Effective date type enumeration.
/// </summary>
public enum EnumsEffDateType
{
    NOW = 0,
    WAIT = 1
}

