using ChildFund.Web.Infrastructure.Cms.Constants;
using ChildFund.Web.Infrastructure.Cms.Extensions;
using ChildFund.Web.Infrastructure.Cms.Helpers;
using ChildFund.Web.Infrastructure.Cms.Services;
using EPiServer.Globalization;
using Microsoft.AspNetCore.Mvc.Routing;

namespace ChildFund.Web.Infrastructure.Cms.Extensions;

public static class UrlExtensions
{
    private const string _fileDateTicksCacheKeyFormat = "FileDateTicks_{0}";
    private const string _editQueryString = "?epieditmode=";

    private static readonly Lazy<IUrlResolver> _urlResolver =
        new(() => ServiceLocator.Current.GetInstance<IUrlResolver>());

    private static readonly Lazy<IContentLoader> _contentLoader =
        new(() => ServiceLocator.Current.GetInstance<IContentLoader>());

    private static readonly Lazy<IHttpContextAccessor> _httpContextAccessor =
        new(() => ServiceLocator.Current.GetInstance<IHttpContextAccessor>());

    private static readonly Lazy<ICacheService> _cacheService =
        new(() => ServiceLocator.Current.GetInstance<ICacheService>());

    private static readonly Lazy<IWebHostingEnvironment> _hostingEnvironment =
        new(() => ServiceLocator.Current.GetInstance<IWebHostingEnvironment>());

    public static RouteValueDictionary ContentRoute(this IUrlHelper urlHelper,
        ContentReference contentLink,
        object routeValues = null)
    {
        var first = new RouteValueDictionary(routeValues);

        var values = first.Union(urlHelper.ActionContext.RouteData.Values);

        values[RoutingConstants.ActionKey] = "index";
        values[RoutingConstants.ContentLinkKey] = contentLink;
        return values;
    }

    /// <summary>
    ///     Returns the target URL for a PageReference. Respects the page's shortcut setting
    ///     so if the page is set as a shortcut to another page or an external URL that URL
    ///     will be returned.
    /// </summary>
    public static IHtmlContent PageLinkUrl(this IUrlHelper urlHelper,
        ContentReference pageLink)
    {
        if (ContentReference.IsNullOrEmpty(pageLink))
        {
            return HtmlString.Empty;
        }

        var page = _contentLoader.Value.Get<PageData>(pageLink);

        return urlHelper.PageLinkUrl(page);
    }

    /// <summary>
    ///     Returns the target URL for a page. Respects the page's shortcut setting
    ///     so if the page is set as a shortcut to another page or an external URL that URL
    ///     will be returned.
    /// </summary>
    public static IHtmlContent PageLinkUrl(this IUrlHelper urlHelper,
        PageData page)
    {
        switch (page.LinkType)
        {
            case PageShortcutType.Normal:
            case PageShortcutType.FetchData:
                return new HtmlString(_urlResolver.Value.GetUrl(page.PageLink));

            case PageShortcutType.Shortcut:
                if (page.Property["PageShortcutLink"] is PropertyPageReference shortcutProperty &&
                    !ContentReference.IsNullOrEmpty(shortcutProperty.PageLink))
                {
                    return urlHelper.PageLinkUrl(shortcutProperty.PageLink);
                }

                break;

            case PageShortcutType.External:
                return new HtmlString(page.LinkURL);
        }

        return HtmlString.Empty;
    }

    public static IHtmlContent GetSegmentedUrl(this IUrlHelper urlHelper,
        PageData currentPage,
        params string[] segments)
    {
        var url = urlHelper.PageLinkUrl(currentPage).ToString();

        if (url != null && !url.EndsWith("/"))
        {
            url += '/';
        }

        url += string.Join("/", segments);

        return new HtmlString(url);
    }

    public static string GetUrlNoHost(this ContentReference content) => UrlResolver.Current.GetUrl(content);

    public static string GetUrlNoHost(this Url url) => UrlResolver.Current.GetUrl(url.ToString());

    public static string GetUrl(this ContentReference content)
    {
        var siteUrl = SiteDefinitionHelper.GetPrimaryHostUri();
        var url = UrlResolver.Current.GetUrl(content);
        if (siteUrl == null)
            return url;
        return new Uri(siteUrl, url).ToString();
    }

    public static string GetUrl(this UrlResolver urlResolver, HttpRequest request, ContentReference contentLink,
        string language)
    {
        if (!ContentReference.IsNullOrEmpty(contentLink))
        {
            return urlResolver.GetUrl(contentLink, language);
        }

        return string.IsNullOrEmpty(request.Headers["Referer"]) ? "/" : request.Headers["Referer"];
    }

    public static IHtmlContent ImageExternalUrl(this IUrlHelper urlHelper,
        ImageData image) =>
        new HtmlString(_urlResolver.Value.GetUrl(image.ContentLink));

    public static IHtmlContent ImageExternalUrl(this IUrlHelper urlHelper,
        ImageData image,
        string variant) => urlHelper.ImageExternalUrl(image.ContentLink, variant);

    public static IHtmlContent ImageExternalUrl(this UrlHelper urlHelper,
        Uri imageUri,
        string variant) =>
        new HtmlString(
            string.IsNullOrWhiteSpace(variant) ? imageUri.ToString() : imageUri + "/" + variant);

    public static IHtmlContent ImageExternalUrl(this IUrlHelper urlHelper,
        ContentReference imageReference,
        string variant)
    {
        if (ContentReference.IsNullOrEmpty(imageReference))
        {
            return HtmlString.Empty;
        }

        var url = _urlResolver.Value.GetUrl(imageReference);
        //Inject variant
        if (!string.IsNullOrEmpty(variant))
        {
            if (url.Contains("?"))
            {
                url = url.Insert(url.IndexOf('?'), "/" + variant);
            }
            else
            {
                url = url + "/" + variant;
            }
        }

        return new HtmlString(url);
    }

    public static bool IsExternalUrl(this string urlInternal, HttpRequest request)
    {
        if (string.IsNullOrEmpty(urlInternal))
            return false;

        var externalUrl = UriSupport.AbsoluteUrlBySettings(urlInternal);
        return IsUrlInSiteHosts(externalUrl, request);
    }

    public static bool IsExternalUrl(this Url urlInternal, HttpRequest request)
    {
        if (urlInternal == null)
        {
            return false;
        }

        var url = new UrlBuilder(urlInternal.Uri.ToString());

        var externalUrl = UriSupport.AbsoluteUrlBySettings(url.ToString());
        return IsUrlInSiteHosts(externalUrl, request);
    }

    public static bool IsUrlInSiteHosts(string url, HttpRequest request)
    {
        var currentSite = SiteDefinition.Current;
        if (currentSite != null)
        {
            return !currentSite.Hosts.Any(host => url.Contains(host?.Url?.ToString() ?? string.Empty));
        }

        return !url.Contains(request?.Host.Host ?? string.Empty);

    }

    public static bool IsDownloadFile(this string url)
    {
        //Getting the real url
        var urlHelper = ServiceLocator.Current.GetInstance<UrlResolver>();
        url = urlHelper.GetUrl(url);

        //Remove ExtraUrlTrail for edit mode
        if (url.Contains(_editQueryString))
            url = RemoveExtraUrlTrail(url);

        var fileExtension = GetFileExtensionFromUrl(url);
        return SiteConstants.DownloadFileExtensions.Contains(fileExtension);
    }

    public static string GetFileExtensionFromUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return "";
        }

        url = url.Split('?')[0];
        url = url.Split('/').Last();
        return url.Contains('.') ? url.Substring(url.LastIndexOf('.')) : string.Empty;
    }

    //Created this method because edit mode gives resolved like this: /EPiServer/CMS/Content/globalassets/123media/bkci_8qcdvq.png,,76?epieditmode=true
    private static string RemoveExtraUrlTrail(string url)
    {
        // Define the pattern to match
        string pattern = @",,.*?\?epieditmode=(true|false)";

        // Use Regex.Replace to remove the matched portion
        string result = Regex.Replace(url, pattern, "");

        return result;
    }

    public static IHtmlContent CampaignUrl(this IUrlHelper urlHelper,
        HtmlString url,
        string campaign)
    {
        var s = url.ToString();
        if (s.Contains("?"))
        {
            return new HtmlString(s + "&utm_campaign=" + WebUtility.UrlEncode(campaign));
        }

        return new HtmlString(s + "?utm_campaign=" + WebUtility.UrlEncode(campaign));
    }

    public static IHtmlContent GetFriendlyUrl(this IUrlHelper urlHelper, string url) =>
        new HtmlString(_urlResolver.Value.GetUrl(url) ?? url);

    public static IHtmlContent WriteShortenedUrl(string root, string segment)
    {
        var fullUrlPath = $"{root}{segment.ToLower().Replace(" ", "-")}/";

        return new HtmlString(fullUrlPath);
    }

    public static string GetBaseUrl(bool includeLanguage = false)
    {
        var request = _httpContextAccessor.Value.HttpContext?.Request;

        if (request == null)
        {
            return string.Empty;
        }

        var currentLanguage = ContentLanguage.PreferredCulture;
        var currentSiteStartPage = SiteDefinition.Current?.StartPage?.Get<PageData>();

        if (currentSiteStartPage != null && includeLanguage && !currentSiteStartPage.IsMasterLanguageBranch())
        {
            return $"{request.Scheme}://{request.Host}/{currentLanguage.Name.ToLower()}";
        }

        return $"{request.Scheme}://{request.Host}";
    }

    public static string ExternalApiUrl(this string actionUrl, bool includeLanguage = false)
    {
        var baseUrl = GetBaseUrl(includeLanguage);

        if (baseUrl.EndsWith("/"))
        {
            baseUrl = baseUrl.TrimEnd('/');
        }

        // Added / at the end because FEE toolkit generates a 301 redirect when it is not there
        var fullUrl = string.Concat(baseUrl, actionUrl, "/");
        return fullUrl;
    }

    public static string ExternalUrlByLanguage(this ContentReference contentReference, string language,
        string parameters = "")
    {
        var baseUrl = GetBaseUrl();

        if (baseUrl.EndsWith("/"))
        {
            baseUrl = baseUrl.TrimEnd('/');
        }

        var contentPath = !string.IsNullOrEmpty(language)
            ? ServiceLocator.Current.GetInstance<UrlResolver>().GetUrl(contentReference, language)
            : ServiceLocator.Current.GetInstance<UrlResolver>().GetUrl(contentReference);

        if (!contentPath.StartsWith("/"))
        {
            contentPath += '/';
        }

        var contentUrl = string.Concat(baseUrl, contentPath);
        var fullUrl = $"{contentUrl}{parameters}";
        return fullUrl;
    }

    public static string ExternalUrl(this ContentReference contentReference, string parameters = "")
    {
        var baseUrl = GetBaseUrl();

        if (baseUrl.EndsWith("/"))
        {
            baseUrl = baseUrl.TrimEnd('/');
        }

        var contentPath = ServiceLocator.Current.GetInstance<UrlResolver>().GetUrl(contentReference);

        if (!contentPath.StartsWith("/"))
        {
            contentPath += '/';
        }

        var contentUrl = string.Concat(baseUrl, contentPath);
        var fullUrl = $"{contentUrl}{parameters}";
        return fullUrl;
    }

    private static RouteValueDictionary Union(this RouteValueDictionary first,
        RouteValueDictionary second)
    {
        var dictionary = new RouteValueDictionary(second);
        foreach (var pair in first)
        {
            if (pair.Value != null)
            {
                dictionary[pair.Key] = pair.Value;
            }
        }

        return dictionary;
    }

    private static long GetFileDateTicks(this string filename)
    {
        var cacheKey = string.Format(_fileDateTicksCacheKeyFormat, filename);

        // Check if we already cached the ticks in the cache.
        if (_cacheService.Value.Exists(cacheKey))
        {
            return _cacheService.Value.Get<long>(cacheKey);
        }

        var physicalPath = Path.Combine(_hostingEnvironment.Value.WebRootPath, filename);
        var fileInfo = new FileInfo(physicalPath);

        // If file exists, read number of ticks from last write date. Or fall back to 0.
        var ticks = fileInfo.Exists ? fileInfo.LastWriteTime.Ticks : 0;

        // Add the number of ticks to cache for 12 hours.
        // The cache dependency will remove the entry if file is changed or deleted.
        _cacheService.Value.Add(cacheKey, ticks, TimeSpan.FromHours(12));

        return ticks;
    }

    public static string ContentVersioned(this UrlHelper urlHelper, string contentPath)
    {
        var url = urlHelper.Content(contentPath);

        var fileTicks = url.GetFileDateTicks();

        return $"{url}?v={fileTicks}";
    }
}