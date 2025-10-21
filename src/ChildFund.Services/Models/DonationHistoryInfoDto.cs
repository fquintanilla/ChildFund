using System.Text.Json.Serialization;

namespace ChildFund.Services.Models;

/// <summary>
/// Donation history information DTO - matches WCF auto-generated model with k__BackingField naming.
/// </summary>
public partial class DonationHistoryInfoDto
{
    [JsonPropertyName("_TransDate")]
    public DateTime _TransDate { get; set; }

    [JsonPropertyName("<TransAmount>k__BackingField")]
    public float TransAmount { get; set; }

    [JsonPropertyName("<agpid>k__BackingField")]
    public int AgpId { get; set; }

    [JsonPropertyName("<amtlifetime>k__BackingField")]
    public string? AmtLifetime { get; set; }

    [JsonPropertyName("<amtpreviousyear>k__BackingField")]
    public string? AmtPreviousYear { get; set; }

    [JsonPropertyName("<amtyeartodate>k__BackingField")]
    public string? AmtYearToDate { get; set; }

    [JsonPropertyName("<contid>k__BackingField")]
    public int ContId { get; set; }

    [JsonPropertyName("<desc>k__BackingField")]
    public string? Description { get; set; }

    [JsonPropertyName("<dontype>k__BackingField")]
    public string? DonType { get; set; }

    [JsonPropertyName("<errormessage>k__BackingField")]
    public string? ErrorMessage { get; set; }

    [JsonPropertyName("<fromDate>k__BackingField")]
    public DateTime? FromDate { get; set; }

    [JsonPropertyName("<toDate>k__BackingField")]
    public DateTime? ToDate { get; set; }

    [JsonPropertyName("<updatedDate>k__BackingField")]
    public DateTime? UpdatedDate { get; set; }
}
