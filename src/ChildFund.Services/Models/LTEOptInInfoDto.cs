using System.Text.Json.Serialization;

namespace ChildFund.Services.Models;

/// <summary>
/// LTE opt-in information DTO - matches WCF auto-generated model.
/// </summary>
public partial class LTEOptInInfoDto
{
    [JsonPropertyName("Childnumber")]
    public int ChildNumber { get; set; }

    [JsonPropertyName("Contactid")]
    public int ContactId { get; set; }

    [JsonPropertyName("Isoptedin")]
    public bool IsOptedIn { get; set; }

    [JsonPropertyName("Noid")]
    public int NoId { get; set; }

    [JsonPropertyName("Optindate")]
    public DateTime? OptInDate { get; set; }

    [JsonPropertyName("Sponsorshipid")]
    public int SponsorshipId { get; set; }

    [JsonPropertyName("Status")]
    public string? Status { get; set; }
}
