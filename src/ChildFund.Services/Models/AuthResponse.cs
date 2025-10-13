using System.Text.Json.Serialization;

namespace ChildFund.Services.Models;

internal sealed class AuthResponse
{
    [JsonPropertyName("Token")]
    public string Token { get; set; } = "";

    [JsonPropertyName("ExpireDate")]
    public DateTimeOffset ExpireDate { get; set; }
}

