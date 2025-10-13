using ChildFund.Infrastructure.Display;

namespace ChildFund.Core.Media.Interfaces;

public interface IImageMediaService
{
    public string BuildUrlsFromCutoffs(string imageUrl, List<PictureSize> cutoffs, bool isWebp);
    public string BuildUrlsFromCutoffs(string imageUrl, PictureSize cutoff, bool isWebp);
}
