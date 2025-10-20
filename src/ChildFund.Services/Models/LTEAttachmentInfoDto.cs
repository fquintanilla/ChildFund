using System.Text.Json.Serialization;

namespace ChildFund.Services.Models;

/// <summary>
/// LTE attachment information DTO - matches WCF auto-generated model.
/// </summary>
public partial class LTEAttachmentInfoDto
{
    [JsonPropertyName("Caption")]
    public string? Caption { get; set; }

    [JsonPropertyName("DateReceived")]
    public DateTime? DateReceived { get; set; }

    [JsonPropertyName("FileName")]
    public string? FileName { get; set; }

    [JsonPropertyName("RawFile")]
    public byte[]? RawFile { get; set; }
}
