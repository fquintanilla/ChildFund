using System.Text.Json.Serialization;

namespace ChildFund.Web.Core.Models
{
    public sealed class CountryDto
    {

        [JsonPropertyName("<rvmeaning>k__BackingField")]
        public string? Name { get; set; }

        [JsonPropertyName("<rvlowvalue>k__BackingField")]
        public string? Code { get; set; }

        [JsonPropertyName("<rvhighvalue>k__BackingField")]
        public string? HighValue { get; set; }
    }
}
