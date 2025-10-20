using System.Text.Json.Serialization;

namespace ChildFund.Services.Models;

/// <summary>
/// LTE letter folder information DTO - matches WCF auto-generated model with k__BackingField naming.
/// </summary>
public partial class LTELetterFolderInfoDto
{
    [JsonPropertyName("<ContactId>k__BackingField")]
    public int ContactId { get; set; }

    [JsonPropertyName("<FolderId>k__BackingField")]
    public int FolderId { get; set; }

    [JsonPropertyName("<FolderName>k__BackingField")]
    public string? FolderName { get; set; }

    [JsonPropertyName("<ParentFolderId>k__BackingField")]
    public int ParentFolderId { get; set; }
}
