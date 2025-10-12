using Newtonsoft.Json;

namespace ChildFund.Core.Media.Models;

[ContentType(DisplayName = "Vector Image File",
    GUID = "3bedeaa0-67ba-4f6a-a420-dabf6ad6890b",
    Description = "Used for svg image file type")]
[MediaDescriptor(ExtensionString = "svg")]
public class VectorImageMediaData : ImageMediaData
{
    /// <summary>
    ///     Gets the generated thumbnail for this media.
    /// </summary>
    [JsonIgnore]
    [Searchable(false)]
    public override Blob Thumbnail => BinaryData;

    [Display(Name = "Use Html Tag", GroupName = SystemTabNames.Content, Order = 220)]
    [JsonIgnore]
    [Searchable(false)]
    public virtual bool UseHtmlTag { get; set; }

    public override void SetDefaultValues(ContentType contentType)
    {
        base.SetDefaultValues(contentType);
        UseHtmlTag = true;
    }
}