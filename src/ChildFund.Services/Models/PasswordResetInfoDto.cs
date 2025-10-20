using System.Text.Json.Serialization;

namespace ChildFund.Services.Models;

/// <summary>
/// Password reset information DTO - matches WCF auto-generated model.
/// </summary>
public partial class PasswordResetInfoDto
{
    [JsonPropertyName("Email")]
    public string? Email { get; set; }

    [JsonPropertyName("IsValid")]
    public bool IsValid { get; set; }

    [JsonPropertyName("LinkHash")]
    public string? LinkHash { get; set; }

    [JsonPropertyName("ResetId")]
    public int ResetId { get; set; }

    [JsonPropertyName("StaleDate")]
    public DateTime? StaleDate { get; set; }
}
