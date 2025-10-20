using System.Text.Json.Serialization;

namespace ChildFund.Services.Models;

/// <summary>
/// Automatic Gift Program (AGP) information DTO.
/// </summary>
public class AgpInfoDto
{
    [JsonPropertyName("<AccountNumberE>k__BackingField")]
    public string? AccountNumberE { get; set; }

    [JsonPropertyName("<AccountNumber>k__BackingField")]
    public string? AccountNumber { get; set; }

    [JsonPropertyName("<Active>k__BackingField")]
    public string? Active { get; set; }

    [JsonPropertyName("<AgpType>k__BackingField")]
    public string? AgpType { get; set; }

    [JsonPropertyName("<BankName>k__BackingField")]
    public string? BankName { get; set; }

    [JsonPropertyName("<BkId>k__BackingField")]
    public int BkId { get; set; }

    [JsonPropertyName("<CardName>k__BackingField")]
    public string? CardName { get; set; }

    [JsonPropertyName("<CardType>k__BackingField")]
    public string? CardType { get; set; }

    [JsonPropertyName("<CardholderZipcode>k__BackingField")]
    public string? CardholderZipcode { get; set; }

    [JsonPropertyName("<ContactId>k__BackingField")]
    public int ContactId { get; set; }

    [JsonPropertyName("<DateCreated>k__BackingField")]
    public DateTime? DateCreated { get; set; }

    [JsonPropertyName("<DateDfTaken>k__BackingField")]
    public DateTime? DateDfTaken { get; set; }

    [JsonPropertyName("<DateExpiration>k__BackingField")]
    public DateTime? DateExpiration { get; set; }

    [JsonPropertyName("<FullName>k__BackingField")]
    public string? FullName { get; set; }

    [JsonPropertyName("<Id>k__BackingField")]
    public int Id { get; set; }

    [JsonPropertyName("<IndHidden>k__BackingField")]
    public string? IndHidden { get; set; }

    [JsonPropertyName("<MktacId>k__BackingField")]
    public int MktacId { get; set; }

    [JsonPropertyName("<PreNoteFlag>k__BackingField")]
    public string? PreNoteFlag { get; set; }

    [JsonPropertyName("<TransitNumber>k__BackingField")]
    public string? TransitNumber { get; set; }

    [JsonPropertyName("<doesAcceptACHTerms>k__BackingField")]
    public bool DoesAcceptACHTerms { get; set; }
}

