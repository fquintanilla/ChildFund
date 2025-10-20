using System.Text.Json.Serialization;

namespace ChildFund.Services.Models;

/// <summary>
/// Email subscriptions information DTO - matches WCF auto-generated model with k__BackingField naming.
/// </summary>
public partial class EmailSubscriptionsInfoDto
{
    [JsonPropertyName("<Abbreviation>k__BackingField")]
    public string? Abbreviation { get; set; }

    [JsonPropertyName("<ContId>k__BackingField")]
    public int ContId { get; set; }

    [JsonPropertyName("<Description>k__BackingField")]
    public string? Description { get; set; }

    [JsonPropertyName("<EpubId>k__BackingField")]
    public int EpubId { get; set; }
}
