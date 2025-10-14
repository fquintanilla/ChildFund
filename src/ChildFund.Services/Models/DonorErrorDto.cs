using System.Text.Json.Serialization;

namespace ChildFund.Services.Models
{
    public class DonorErrorDto
    {
        [JsonPropertyName("<AgpOut>k__BackingField")]
        public object? AgpOut { get; set; }

        [JsonPropertyName("<AvailableKids>k__BackingField")]
        public object? AvailableKids { get; set; }

        [JsonPropertyName("<HouseholdMatches>k__BackingField")]
        public object? HouseholdMatches { get; set; }

        [JsonPropertyName("<TransOut>k__BackingField")]
        public object? TransOut { get; set; }

        public List<string>? ErrorList { get; set; }

        public string? ErrorMessage { get; set; }

        public long? NewID { get; set; }

        public string? NewOutput { get; set; }

        public string? NewValue { get; set; }

        public int? NumberOfRecordsAffected { get; set; }

        public int? ReturnCode { get; set; }

        public int? ReturnMessage { get; set; }

        public string? StackTrace { get; set; }
    }
}
