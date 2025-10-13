using ChildFund.Web.Core.Media.Interfaces;
using ChildFund.Web.Infrastructure.Display;

namespace ChildFund.Web.Core.Media.Services;

[ServiceConfiguration(ServiceType = typeof(IImageMediaService))]
public class ImageMediaService : IImageMediaService
{
    public string BuildUrlsFromCutoffs(string imageUrl, List<PictureSize> cutoffs, bool isWebp)
    {
        var last = cutoffs.Last();
        var urlList = "";
        foreach (var cutoff in cutoffs)
        {
            urlList += BuildUrlsFromCutoffs(imageUrl, cutoff, isWebp);
            if (cutoff != last)
            {
                urlList += ", ";
            }
        }
        return urlList;
    }

    public string BuildUrlsFromCutoffs(string imageUrl, PictureSize cutoff, bool isWebp)
    {
        return $"{imageUrl}?width={cutoff.Width}&height={cutoff.Height}{(isWebp ? "&format=webp" : "")} {cutoff.Zoom}x";

    }
}