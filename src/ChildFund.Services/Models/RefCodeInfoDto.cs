using System.Text.Json.Serialization;

namespace ChildFund.Services.Models;

/// <summary>
/// Reference code information DTO for lookup data.
/// </summary>
public class RefCodeInfoDto
{
    [JsonPropertyName("<rvlowvalue>k__BackingField")]
    public string? Code { get; set; }

    [JsonPropertyName("<rvmeaning>k__BackingField")]
    public string? Name { get; set; }
}
