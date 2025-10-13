using System.Text.Json.Serialization;

namespace ChildFund.Web.Core.Auth
{
    internal sealed class AuthResponse
    {
        [JsonPropertyName("Token")]
        public string Token { get; set; } = "";

        [JsonPropertyName("ExpireDate")]
        public DateTimeOffset ExpireDate { get; set; }
    }
}
