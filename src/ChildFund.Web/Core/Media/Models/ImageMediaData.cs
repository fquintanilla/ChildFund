using Newtonsoft.Json;

namespace ChildFund.Web.Core.Media.Models;

[ContentType(DisplayName = "Image File",
    GUID = "20644be7-3ca1-4f84-b893-ee021b73ce6c",
    Description = "Used for image file types such as jpg, jpeg, jpe, ico, gif, bmp, png")]
[MediaDescriptor(ExtensionString = "jpg,jpeg,jpe,ico,gif,bmp,png")]
public class ImageMediaData : ImageData
{
    [Editable(false)]
    [JsonIgnore]
    [Searchable(false)]
    public override Blob Thumbnail => BinaryData;

    [CultureSpecific]
    [Display(Description = "Description of the image", GroupName = SystemTabNames.Content, Order = 160)]
    public virtual string Description { get; set; }

    [CultureSpecific]
    [Display(Name = "Alternate text", GroupName = SystemTabNames.Content, Order = 170)]
    public virtual string AltText { get; set; }

    [Editable(false)]
    [JsonIgnore]
    [Searchable(false)]
    public virtual int Height { get; set; }

    [Editable(false)]
    [JsonIgnore]
    [Searchable(false)]
    public virtual int Width { get; set; }
}