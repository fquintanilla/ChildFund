using ChildFund.Features.Home;
using ChildFund.Features.Shared.Pages;
using ChildFund.Infrastructure.Cms.Services;
using EPiServer.Security;
using Microsoft.Net.Http.Headers;

namespace ChildFund.Infrastructure.Cms.Extensions;

public static class ContentExtensions
{
    private static readonly Regex _mobileRegex1 = new(
        @"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino",
        RegexOptions.IgnoreCase | RegexOptions.Multiline);

    private static readonly Regex _mobileRegex2 = new(
        @"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-",
        RegexOptions.IgnoreCase | RegexOptions.Multiline);

    private static readonly Lazy<IUrlResolver> _urlResolver =
        new(() => ServiceLocator.Current.GetInstance<IUrlResolver>());

    private static readonly Lazy<ICookieService> _cookieService =
        new(() => ServiceLocator.Current.GetInstance<ICookieService>());

    private static readonly Lazy<IContentLoader> _contentLoader =
        new(() => ServiceLocator.Current.GetInstance<IContentLoader>());

    public static IEnumerable<PageData> GetSiblings(this PageData pageData) =>
        GetSiblings(pageData, _contentLoader.Value);

    public static IEnumerable<PageData> GetSiblings(this PageData pageData, IContentLoader contentLoader)
    {
        var filter = new FilterContentForVisitor();
        return contentLoader.GetChildren<PageData>(pageData.ParentLink).Where(page => !filter.ShouldFilter(page));
    }

    public static IEnumerable<T> GetSiblings<T>(this PageData pageData, IContentLoader contentLoader) where T : PageData
    {
        var filter = new FilterContentForVisitor();
        return contentLoader.GetChildren<T>(pageData.ParentLink).Where(page => !filter.ShouldFilter(page));
    }

    public static IEnumerable<T> FilterForDisplay<T>(this IEnumerable<T> contents, bool requirePageTemplate = false,
        bool requireVisibleInMenu = false)
        where T : IContent
    {
        var accessFilter = new FilterAccess();
        var publishedFilter = new FilterPublished();
        contents = contents.Where(x => !publishedFilter.ShouldFilter(x) && !accessFilter.ShouldFilter(x));
        if (requirePageTemplate)
        {
            var templateFilter = ServiceLocator.Current.GetInstance<FilterTemplate>();
            templateFilter.TemplateTypeCategories = TemplateTypeCategories.Request;
            contents = contents.Where(x => !templateFilter.ShouldFilter(x));
        }

        if (requireVisibleInMenu)
        {
            contents = contents.Where(x => VisibleInMenu(x));
        }

        return contents;
    }

    private static bool VisibleInMenu(IContent content) => content is not PageData page || page.VisibleInMenu;

    public static IEnumerable<T> GetItems<T>(this IList<ContentReference> references)
    {
        if (references == null || !references.Any())
        {
            return new List<T>();
        }

        var items = _contentLoader.Value.GetItems(references, CultureInfo.InvariantCulture).OfType<T>();
        return items;
    }

    public static IEnumerable<T> GetItems<T>(this IList<ContentReference> references, CultureInfo language)
    {
        if (references == null || !references.Any())
        {
            return new List<T>();
        }

        var items = _contentLoader.Value.GetItems(references, language).OfType<T>();
        return items;
    }

    public static IList<FoundationPageData> GetAncestors(this IContent reference)
    {
        var ancestors = _contentLoader.Value.GetAncestors(reference.ContentLink).OfType<FoundationPageData>().Reverse()
            .Filter(new FilterAccess(AccessLevel.Read))
            .Filter(new FilterPublished())
            .Filter(new FilterTemplate()).Select(x => x as FoundationPageData).ToList();

        return ancestors;
    }

    public static IEnumerable<PageData> GetAncestorsOrSelf(this IContent reference)
    {
        var ancestors = _contentLoader.Value.GetAncestors(reference.ContentLink).OfType<PageData>()
            .Filter(new FilterAccess(AccessLevel.Read))
            .Filter(new FilterPublished())
            .Filter(new FilterTemplate()).ToList();

        ancestors.Add(reference as PageData);

        return ancestors;
    }

    public static IEnumerable<PageData> Filter(this IEnumerable<PageData> pages, ContentFilterBase filter)
    {
        EPiServer.Framework.Validator.ThrowIfNull(nameof(pages), pages);
        EPiServer.Framework.Validator.ThrowIfNull(nameof(filter), filter);
        return pages.Where(p => !filter.ShouldFilter(p));
    }

    public static ContentReference GetRelativeStartPage(this IContent content)
    {
        if (content is HomePage)
        {
            return content.ContentLink;
        }

        var ancestors = _contentLoader.Value.GetAncestors(content.ContentLink);
        return ancestors.FirstOrDefault(x => x is HomePage) is not HomePage startPage
            ? ContentReference.StartPage
            : startPage.ContentLink;
    }

    public static string GetPublicUrl(this ContentReference contentLink, string language = null)
    {
        if (ContentReference.IsNullOrEmpty(contentLink))
        {
            return string.Empty;
        }

        var content = language != null ? contentLink.GetByLanguage<IContent>(language) : contentLink.Get<IContent>();
        if (content == null || !PublishedStateAssessor.IsPublished(content))
        {
            return string.Empty;
        }

        return _urlResolver.Value.GetUrl(contentLink, language);
    }

    public static string GetPublicUrl(this Guid contentGuid, string language)
    {
        var contentLink = PermanentLinkUtility.FindContentReference(contentGuid);
        return GetPublicUrl(contentLink, language);
    }

    public static bool IsMobile(this IHeaderDictionary headers)
    {
        if (headers == null)
        {
            return false;
        }

        if (!headers.ContainsKey(HeaderNames.UserAgent))
        {
            return false;
        }

        return _mobileRegex1.IsMatch(headers[HeaderNames.UserAgent].ToString()) ||
               _mobileRegex2.IsMatch(headers[HeaderNames.UserAgent].ToString().Substring(0, 4));
    }

    /// <summary>
    ///     Helper method to get a URL string for an IContent
    /// </summary>
    /// <param name="content">The routable content item to get the URL for.</param>
    /// <param name="isAbsolute">Whether the full URL including protocol and host should be returned.</param>
    public static string GetUrl<T>(this T content, bool isAbsolute = false) where T : IContent, ILocale, IRoutable =>
        content.GetUri(isAbsolute)?.ToString();

    /// <summary>
    ///     Helper method to get a Uri for an IContent
    /// </summary>
    /// <param name="content">The routable content item to get the URL for.</param>
    /// <param name="isAbsolute">Whether the full URL including protocol and host should be returned.</param>
    public static Uri GetUri<T>(this T content, bool isAbsolute = false) where T : IContent, ILocale, IRoutable =>
        content.ContentLink.GetUri(content.Language.Name, isAbsolute);
}