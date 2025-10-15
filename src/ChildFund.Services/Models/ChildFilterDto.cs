using System.Text.Json.Serialization;

namespace ChildFund.Services.Models;

/// <summary>
/// Filter criteria for child searches.
/// </summary>
public class ChildFilterDto
{
    [JsonPropertyName("<BirthMonth>k__BackingField")]
    public string? BirthMonth { get; set; }

    [JsonPropertyName("<Birthday>k__BackingField")]
    public string? Birthday { get; set; }

    [JsonPropertyName("<CountryCodeFromAge>k__BackingField")]
    public int CountryCodeFromAge { get; set; }

    [JsonPropertyName("<CountryCode>k__BackingField")]
    public int CountryCode { get; set; }

    [JsonPropertyName("<FromAge>k__BackingField")]
    public int FromAge { get; set; }

    [JsonPropertyName("<Gender>k__BackingField")]
    public string? Gender { get; set; }

    [JsonPropertyName("<IpAddress>k__BackingField")]
    public string? IpAddress { get; set; }

    [JsonPropertyName("<KidsReturned>k__BackingField")]
    public int KidsReturned { get; set; }

    [JsonPropertyName("<NoId>k__BackingField")]
    public int NoId { get; set; }

    [JsonPropertyName("<ProjectId>k__BackingField")]
    public int ProjectId { get; set; }

    [JsonPropertyName("<ToAge>k__BackingField")]
    public int ToAge { get; set; }

    [JsonPropertyName("<ChildId>k__BackingField")]
    public int ChildId { get; set; }
}