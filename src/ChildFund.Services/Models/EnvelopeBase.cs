using System.Text.Json.Serialization;

namespace ChildFund.Services.Models;

/// <summary>
/// Base envelope for API responses.
/// </summary>
public class EnvelopeBase
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
    public ReturnMsg? ReturnMessage { get; set; }

    [JsonPropertyName("StackTrace")]
    public string? StackTrace { get; set; }
}

/// <summary>
/// Return message enumeration.
/// </summary>
public enum ReturnMsg
{
    Success = 0,
    Error = 1,
    Warning = 2
}

