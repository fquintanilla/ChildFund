using ChildFund.Infrastructure.Display;

namespace ChildFund.Core.Media.ViewModels;

public class ImageMediaDataViewModel
{
    public string Name { get; set; }
    public string ImageLink { get; set; }
    public string AltText { get; set; }
    public string Description { get; set; }
    public PictureSizeList DefaultPictureSizes { get; set; }
}