using System.Text.Json.Serialization;

namespace ChildFund.Services.Models
{
    public class AvailableKidsForWebResponseDto
    {
        [JsonPropertyName("<AgpOut>k__BackingField")]
        public string? AgpOut { get; set; }

        [JsonPropertyName("<AvailableKids>k__BackingField")]
        public List<AvailableKidDto>? AvailableKids { get; set; }
    }
}
