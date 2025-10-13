using Newtonsoft.Json;

namespace ChildFund.Core.Media.Models;

[ContentType(DisplayName = "Video File",
    GUID = "8a9d9d4b-cd4b-40e8-a777-414cfbda7770",
    Description = "Used for video file types such as mp4, flv, webm")]
[MediaDescriptor(ExtensionString = "mp4,flv,webm")]
public class VideoFile : VideoData
{
    [UIHint(UIHint.Textarea)]
    [Display(GroupName = SystemTabNames.Content, Order = 20)]
    [JsonIgnore]
    [Searchable(false)]
    public virtual string Copyright { get; set; }

    [Display(Name = "Display controls", GroupName = SystemTabNames.Content, Order = 30)]
    [JsonIgnore]
    [Searchable(false)]
    public virtual bool DisplayControls { get; set; }

    [Display(GroupName = SystemTabNames.Content, Order = 40)]
    [JsonIgnore]
    [Searchable(false)]
    public virtual bool Autoplay { get; set; }

    public override void SetDefaultValues(ContentType contentType)
    {
        base.SetDefaultValues(contentType);

        Autoplay = false;
        DisplayControls = true;
    }
}