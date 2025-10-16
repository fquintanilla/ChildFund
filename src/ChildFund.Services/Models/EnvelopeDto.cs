using System.Text.Json.Serialization;

namespace ChildFund.Services.Models;

/// <summary>
/// Base envelope for API responses.
/// </summary>
public class EnvelopeBaseDto
{
    [JsonPropertyName("ErrorList")]
    public string[]? ErrorList { get; set; }

    [JsonPropertyName("ErrorMessage")]
    public string? ErrorMessage { get; set; }

    [JsonPropertyName("NewID")]
    public int NewID { get; set; }

    [JsonPropertyName("NewOutput")]
    public byte[]? NewOutput { get; set; }

    [JsonPropertyName("NewValue")]
    public string? NewValue { get; set; }

    [JsonPropertyName("NumberOfRecordsAffected")]
    public int NumberOfRecordsAffected { get; set; }

    [JsonPropertyName("ReturnCode")]
    public int ReturnCode { get; set; }

    [JsonPropertyName("ReturnMessage")]
    public ReturnMsg ReturnMessage { get; set; }

    [JsonPropertyName("StackTrace")]
    public string? StackTrace { get; set; }
}

/// <summary>
/// Envelope containing API response data.
/// </summary>
public class EnvelopeDto : EnvelopeBaseDto
{
    [JsonPropertyName("<AgpOut>k__BackingField")]
    public AgpInfoDto? AgpOut { get; set; }

    [JsonPropertyName("<AvailableKids>k__BackingField")]
    public WebChildInfoDto[]? AvailableKids { get; set; }

    [JsonPropertyName("<HouseholdMatches>k__BackingField")]
    public ContactInfoDto[]? HouseholdMatches { get; set; }

    [JsonPropertyName("<TransOut>k__BackingField")]
    public TransactionInfoDto? TransOut { get; set; }
}

/// <summary>
/// Return message enumeration.
/// </summary>
public enum ReturnMsg
{
    Success = 0,
    Failure = 1,
    Unknown = 2,
    Error = 1,
    Warning = 2
}
