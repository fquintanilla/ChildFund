namespace ChildFund.Web.Infrastructure.Display;

public enum PictureDimensionBreakpoints
{
    //All of these numbers are (Breakpoint - 1)
    Mobile = 359,
    MobileLarge = 479,
    Tablet = 767,
    TabletLandscape = 1023,
    Desktop = 1280,
    Wide = 1439,
    DesktopFullHd = 1920
}

public static class PictureDimensions
{
    //Add here public static readonly PictureSizeList ModuleImages = new (), add values depending on the markup
}

public class PictureSize
{
    public PictureSize()
    { }

    public PictureSize(int zoom, int width, int height)
    {
        Zoom = zoom;
        Width = width;
        Height = height;
    }

    public int Height { get; set; }
    public int Width { get; set; }
    public int Zoom { get; set; }
}

public class PictureSizeList
{
    public PictureSizeList()
    { }

    public PictureSizeList(List<ImageDimensions> sourceSetDimensions, List<PictureSize> webpCutoffs, List<PictureSize> imageCutoffs,
        int imageWidth, int imageHeight)
    {
        SourceSetDimensions = sourceSetDimensions;
        WebpCutoffs = webpCutoffs;
        ImageCutoffs = imageCutoffs;
        ImageWidth = imageWidth;
        ImageHeight = imageHeight;
    }

    public List<PictureSize> ImageCutoffs { get; set; }
    public int ImageHeight { get; set; }
    public int ImageWidth { get; set; }
    public List<ImageDimensions> SourceSetDimensions { get; set; }
    public List<PictureSize> WebpCutoffs { get; set; }
}
public class ImageDimensions
{
    public ImageDimensions(int maxwidth, List<PictureSize> cutoffs)
    {
        MaxWidth = maxwidth;
        Cutoffs = cutoffs;
    }

    public ImageDimensions(PictureDimensionBreakpoints maxwidth, List<PictureSize> cutoffs)
    {
        MaxWidth = (int)maxwidth;
        Cutoffs = cutoffs;
    }

    public List<PictureSize> Cutoffs { get; set; }
    public int MaxWidth { get; set; }
}