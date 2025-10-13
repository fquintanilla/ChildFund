using ChildFund.Infrastructure.Cms.Helpers;

//using ImageProcessor.Web.Episerver;

namespace ChildFund.Infrastructure.Cms.Extensions;

public static class ImageUrlExtensions
{
    /// <summary>
    ///     Render a resized image URL with webp support to change format if supported
    /// </summary>
    /// <param name="urlHelper">The url helper</param>
    /// <param name="contentLink">The content link</param>
    /// <param name="width">Resize the width.</param>
    /// <param name="height">Resize the height.</param>
    /// <returns>The url with resizing and webp support</returns>
    public static string WebPFallbackImageUrl(this IUrlHelper urlHelper, ContentReference contentLink,
        int? width = null, int? height = null) =>
        WebPFallbackImageUrl(urlHelper, urlHelper.ContentUrl(contentLink), width, height);

    /// <summary>
    ///     Render a resized image URL with webp support to change format if supported
    /// </summary>
    /// <param name="urlHelper">The url helper</param>
    /// <param name="url">The url</param>
    /// <param name="width">Resize the width.</param>
    /// <param name="height">Resize the height.</param>
    /// <returns>The url with resizing and webp support</returns>
    public static string WebPFallbackImageUrl(this IUrlHelper urlHelper, string url, int? width = null,
        int? height = null)
    {
        var imageUrl = new UrlBuilder(ResizeImageUrl(urlHelper, url, width, height));
        if (WebPHelper.SupportsWebP(urlHelper.ActionContext.HttpContext.Request))
        {
            imageUrl.QueryCollection.Add("format", "webp");
        }

        return imageUrl.ToString();
    }

    /// <summary>
    ///     Render a resized image URL
    /// </summary>
    /// <param name="urlHelper">The url helper</param>
    /// <param name="url">The url</param>
    /// <param name="width">Resize the width.</param>
    /// <param name="height">Resize the height.</param>
    /// <returns>The url with resizing</returns>
    public static string ResizeImageUrl(this IUrlHelper urlHelper, string url, int? width = null, int? height = null)
    {
        var imageUrl = new UrlBuilder(url);
        if (width.HasValue)
        {
            imageUrl.QueryCollection.Add("width", width.ToString());
        }

        if (height.HasValue)
        {
            imageUrl.QueryCollection.Add("height", height.ToString());
        }

        return imageUrl.ToString();
    }
}