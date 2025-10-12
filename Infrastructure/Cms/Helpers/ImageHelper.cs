using ChildFund.Core.Media.Models;
using ChildFund.Infrastructure.Display;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ChildFund.Infrastructure.Cms.Helpers
{
	public class ImageHelper
    {
        public static PictureSize GetDimensions(Blob imageBlob)
        {
            var imageDimensions = new PictureSize();

            try
            {
                using (Image<Rgba32> image = Image.Load<Rgba32>(imageBlob.ReadAllBytes()))
                {
                    imageDimensions.Width = image.Width;
                    imageDimensions.Height = image.Height;

                    image.Dispose();
                }
            }catch{} //Empty on purpose

            return imageDimensions;
        }

        public static PictureSizeList CreateDefaultPictureSizeList(ImageMediaData image)
        {
            var cutoffs = new List<PictureSize>()
            {
                new PictureSize() { Height = image.Height, Width = image.Width, Zoom = 1},
                new PictureSize() { Height = image.Height * 2, Width = image.Width * 2, Zoom = 2}
            };
            var pictureSizeList = new PictureSizeList()
            {
                ImageHeight = image.Height,
                ImageWidth = image.Width,
                ImageCutoffs = cutoffs,
                WebpCutoffs = cutoffs,
                SourceSetDimensions = new List<ImageDimensions>()
            };
            return pictureSizeList;
        }
    }
}
