using System.Text.Json.Serialization;

namespace ChildFund.Web.Core.Models
{
    public sealed class ChildSummaryDto
    {

        [JsonPropertyName("<CountryName>k__BackingField")]
        public string? CountryName { get; set; }

        [JsonPropertyName("<DOB>k__BackingField")]
        public DateTime? DateOfBirth { get; set; }

        [JsonPropertyName("<age>k__BackingField")]
        public int? Age { get; set; }

        [JsonPropertyName("<childnbr>k__BackingField")]
        public long? ChildNumber { get; set; }

        [JsonPropertyName("<shortname>k__BackingField")]
        public string? ShortName { get; set; }

        [JsonPropertyName("<gender>k__BackingField")]
        public string? Gender { get; set; }

        [JsonPropertyName("<cifinfo>k__BackingField")]
        public string? InfoHtml { get; set; }

        [JsonPropertyName("<MonthlySponsorship>k__BackingField")]
        public int? MonthlySponsorship { get; set; }

        [JsonPropertyName("<childphoto>k__BackingField")]
        public int? ChildPhoto { get; set; }
    }
}
