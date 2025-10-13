namespace ChildFund.Web.Core.Media.Models;

[ContentType(
    DisplayName = "Audio File",
    GUID = "0A6E8691-8291-4F40-B69A-ACAC6F850BBB",
    Description = "Used for audio files.")]
[MediaDescriptor(ExtensionString = "mp3,wav,ogg")]
public class AudioFile : MediaData
{
    [CultureSpecific]
    [Display(GroupName = SystemTabNames.Content, Order = 10)]
    public virtual string Description { get; set; }
}
