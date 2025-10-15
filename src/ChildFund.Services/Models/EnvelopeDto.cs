using System.Text.Json.Serialization;

namespace ChildFund.Services.Models;

/// <summary>
/// Envelope containing API response data.
/// </summary>
public class EnvelopeDto : EnvelopeBase
{
    [JsonPropertyName("<AgpOut>k__BackingField")]
    public AgpInfo? AgpOut { get; set; }

    [JsonPropertyName("<AvailableKids>k__BackingField")]
    public WebChildInfoDto[]? AvailableKids { get; set; }

    [JsonPropertyName("<HouseholdMatches>k__BackingField")]
    public ContactInfo[]? HouseholdMatches { get; set; }

    [JsonPropertyName("<TransOut>k__BackingField")]
    public TransactionInfo? TransOut { get; set; }
}

/// <summary>
/// Placeholder for AgpInfo - define properties as needed.
/// </summary>
public class AgpInfo
{
    // Add properties based on actual usage
}

/// <summary>
/// Placeholder for ContactInfo - define properties as needed.
/// </summary>
public class ContactInfo
{
    // Add properties based on actual usage
}

/// <summary>
/// Placeholder for TransactionInfo - define properties as needed.
/// </summary>
public class TransactionInfo
{
    // Add properties based on actual usage
}

