using System.Text.Json.Serialization;

namespace ChildFund.Services.Models;

/// <summary>
/// LTE letter file information DTO - matches WCF auto-generated model with k__BackingField naming.
/// </summary>
public partial class LTELetterFileInfoDto
{
    [JsonPropertyName("<BoxFileUploadId>k__BackingField")]
    public int BoxFileUploadId { get; set; }

    [JsonPropertyName("<BoxSharedLink>k__BackingField")]
    public string? BoxSharedLink { get; set; }

    [JsonPropertyName("<ContactId>k__BackingField")]
    public int ContactId { get; set; }

    [JsonPropertyName("<FolderId>k__BackingField")]
    public int FolderId { get; set; }

    [JsonPropertyName("<LetterId>k__BackingField")]
    public int LetterId { get; set; }

    [JsonPropertyName("<Letter>k__BackingField")]
    public LTELetterInfoDto? Letter { get; set; }
}
