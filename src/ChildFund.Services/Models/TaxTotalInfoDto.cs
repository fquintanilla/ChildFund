using System.Text.Json.Serialization;

namespace ChildFund.Services.Models;

/// <summary>
/// Tax total information DTO - matches WCF auto-generated model with k__BackingField naming.
/// </summary>
public partial class TaxTotalInfoDto
{
    [JsonPropertyName("<CurrentYearTotal>k__BackingField")]
    public string? CurrentYearTotal { get; set; }

    [JsonPropertyName("<PreviousYearTotal>k__BackingField")]
    public string? PreviousYearTotal { get; set; }
}
