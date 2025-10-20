using System.Text.Json.Serialization;

namespace ChildFund.Services.Models;

/// <summary>
/// Code information DTO for lookup data.
/// </summary>
public class CodeInfoDto
{
    [JsonPropertyName("<code>k__BackingField")]
    public string? Code { get; set; }

    [JsonPropertyName("<name>k__BackingField")]
    public string? Name { get; set; }
}
