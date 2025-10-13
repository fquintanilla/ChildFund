using ChildFund.Core.Media.Models;

namespace ChildFund.Features.Shared.ViewModels;

public class ImageViewModel
{
    public ImageViewModel(ImageMediaData input)
    {
        ID = input.ContentLink.ID;
        ContentLink = input.ContentLink;
        Url = UrlResolver.Current.GetUrl(input.ContentLink);
        AltText = input.AltText;
    }

    public int ID { get; set; }
    public ContentReference ContentLink { get; set; }
    public string Url { get; set; }
    public string AltText { get; set; }
}