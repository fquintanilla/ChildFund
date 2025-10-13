using ChildFund.Web.Infrastructure.Display;

namespace ChildFund.Web.Core.Media.Interfaces;

public interface IImageMediaService
{
    public string BuildUrlsFromCutoffs(string imageUrl, List<PictureSize> cutoffs, bool isWebp);
    public string BuildUrlsFromCutoffs(string imageUrl, PictureSize cutoff, bool isWebp);
}
