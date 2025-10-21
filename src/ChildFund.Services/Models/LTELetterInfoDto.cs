using System.Text.Json.Serialization;

namespace ChildFund.Services.Models;

/// <summary>
/// LTE letter information DTO - matches WCF auto-generated model.
/// </summary>
public partial class LTELetterInfoDto
{
    [JsonPropertyName("AttachmentList")]
    public LTEAttachmentInfoDto[]? AttachmentList { get; set; }

    [JsonPropertyName("Author")]
    public AuthorTypes Author { get; set; }

    [JsonPropertyName("ChildId")]
    public int ChildId { get; set; }

    [JsonPropertyName("ChildName")]
    public string? ChildName { get; set; }

    [JsonPropertyName("ContactId")]
    public int ContactId { get; set; }

    [JsonPropertyName("IsELetter")]
    public bool IsELetter { get; set; }

    [JsonPropertyName("IsRead")]
    public bool IsRead { get; set; }

    [JsonPropertyName("LastUpdate")]
    public DateTime LastUpdate { get; set; }

    [JsonPropertyName("LetterHTML")]
    public string? LetterHTML { get; set; }

    [JsonPropertyName("LetterId")]
    public int LetterId { get; set; }

    [JsonPropertyName("LetterName")]
    public string? LetterName { get; set; }

    [JsonPropertyName("LetterPDF")]
    public byte[]? LetterPDF { get; set; }

    [JsonPropertyName("LetterStatus")]
    public LetterStatuses LetterStatus { get; set; }

    [JsonPropertyName("LetterSubject")]
    public string? LetterSubject { get; set; }

    [JsonPropertyName("LetterText")]
    public string? LetterText { get; set; }

    [JsonPropertyName("LetterThumbnail")]
    public byte[]? LetterThumbnail { get; set; }

    [JsonPropertyName("LetterType")]
    public string? LetterType { get; set; }

    [JsonPropertyName("MCSSlipId")]
    public int MCSSlipId { get; set; }

    [JsonPropertyName("NationalOfficeId")]
    public int NationalOfficeId { get; set; }

    [JsonPropertyName("ProjectId")]
    public int ProjectId { get; set; }

    [JsonPropertyName("SharedLink")]
    public string? SharedLink { get; set; }

    [JsonPropertyName("SponsorName")]
    public string? SponsorName { get; set; }

    [JsonPropertyName("StationeryName")]
    public string? StationeryName { get; set; }

    /// <summary>
    /// Author types enumeration.
    /// </summary>
    public enum AuthorTypes
    {
        Sponsor = 0,
        Child = 1,
        NA = 2,
        DND = 3
    }

    /// <summary>
    /// Letter statuses enumeration.
    /// </summary>
    public enum LetterStatuses
    {
        Pending = 0,
        InProcess = 1,
        Rejected = 2,
        Delivered = 3,
        NA = 4
    }
}
