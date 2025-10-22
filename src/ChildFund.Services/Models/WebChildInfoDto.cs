using System.Text.Json.Serialization;

namespace ChildFund.Services.Models;

/// <summary>
/// Information about a child available for sponsorship.
/// </summary>
public class WebChildInfoDto
{
    [JsonPropertyName("<ChildUsed>k__BackingField")]
    public string? ChildUsed { get; set; }

    [JsonPropertyName("<CountryName>k__BackingField")]
    public string? CountryName { get; set; }

    [JsonPropertyName("<DOB>k__BackingField")]
    public string? DateOfBirth { get; set; }

    [JsonPropertyName("<EnterpriseLegacyId>k__BackingField")]
    public string? EnterpriseLegacyId { get; set; }

    [JsonPropertyName("<IsOriginalSearchCriteria>k__BackingField")]
    public bool IsOriginalSearchCriteria { get; set; }

    [JsonPropertyName("<MonthlySponsorship>k__BackingField")]
    public int MonthlySponsorship { get; set; }

    [JsonPropertyName("<NatOfficeThreshold>k__BackingField")]
    public int NatOfficeThreshold { get; set; }

    [JsonPropertyName("<NatOfficeUsed>k__BackingField")]
    public string? NatOfficeUsed { get; set; }

    [JsonPropertyName("<ProjectThreshold>k__BackingField")]
    public int ProjectThreshold { get; set; }

    [JsonPropertyName("<ProjectUsed>k__BackingField")]
    public string? ProjectUsed { get; set; }

    [JsonPropertyName("<age>k__BackingField")]
    public int? Age { get; set; }

    [JsonPropertyName("<childnbr>k__BackingField")]
    public int ChildNumber { get; set; }

    [JsonPropertyName("<childphoto>k__BackingField")]
    public string? ChildPhoto { get; set; }

    [JsonPropertyName("<chstaid>k__BackingField")]
    public int ChildStatusId { get; set; }

    [JsonPropertyName("<cifinfo>k__BackingField")]
    public string? InfoHtml { get; set; }

    [JsonPropertyName("<gender>k__BackingField")]
    public string? Gender { get; set; }

    [JsonPropertyName("<name>k__BackingField")]
    public string? Name { get; set; }

    [JsonPropertyName("<noid>k__BackingField")]
    public int NoId { get; set; }

    [JsonPropertyName("<projid>k__BackingField")]
    public int ProjectId { get; set; }

    [JsonPropertyName("<shortname>k__BackingField")]
    public string? ShortName { get; set; }
}
