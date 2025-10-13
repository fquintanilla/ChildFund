using ChildFund.Web.Core.Media.Models;
using ChildFund.Web.Features.Common.Pages.Folder;
using EPiServer.Framework.Cache;
using EPiServer.Globalization;
using Geta.Optimizely.Categories;
using HtmlAgilityPack;

namespace ChildFund.Web.Infrastructure.Cms.Extensions;

public static class ContentReferenceExtensions
{
    private static readonly Lazy<IContentLoader> _contentLoader =
        new(() => ServiceLocator.Current.GetInstance<IContentLoader>());

    private static readonly Lazy<IContentRepository> _contentRepository =
        new(() => ServiceLocator.Current.GetInstance<IContentRepository>());

    private static readonly Lazy<IContentProviderManager> _providerManager =
        new(() => ServiceLocator.Current.GetInstance<IContentProviderManager>());

    private static readonly Lazy<IPageCriteriaQueryService> _pageCriteriaQueryService =
        new(() => ServiceLocator.Current.GetInstance<IPageCriteriaQueryService>());

    private static readonly Lazy<IUrlResolver> _urlResolver =
        new(() => ServiceLocator.Current.GetInstance<IUrlResolver>());

    private static readonly Lazy<ISiteDefinitionResolver> _siteDefinitionResolver =
        new(() => ServiceLocator.Current.GetInstance<ISiteDefinitionResolver>());

    public static bool IsNullOrEmpty(this ContentReference contentReference) =>
        ContentReference.IsNullOrEmpty(contentReference);

    public static IContent Get<TContent>(this ContentReference contentLink) where TContent : IContent =>
        _contentLoader.Value.Get<TContent>(contentLink);

    public static bool TryGet<TContent>(this ContentReference contentLink, out IContent content) where TContent : IContent
    {
        content = null;

        if (!_contentLoader.Value.TryGet<TContent>(contentLink, out var tryContent))
        {
            return false;
        }

        content = tryContent;
        return true;
    }


    public static IContent GetByLanguage<TContent>(this ContentReference contentLink, string language)
        where TContent : IContent =>
        _contentLoader.Value.Get<TContent>(contentLink, CultureInfo.GetCultureInfo(language));

    public static TContent GetBlock<TContent>(this ContentReference contentLink) where TContent : BlockData =>
        contentLink != null ? _contentLoader.Value.Get<TContent>(contentLink) : null;

    public static IEnumerable<T> GetAllRecursively<T>(this ContentReference rootLink) where T : PageData
    {
        foreach (var child in _contentLoader.Value.GetChildren<T>(rootLink))
        {
            yield return child;

            foreach (var descendant in GetAllRecursively<T>(child.ContentLink))
            {
                yield return descendant;
            }
        }
    }

    public static IEnumerable<T> FindPagesRecursively<T>(this IContentLoader contentLoader, PageReference pageLink)
        where T : PageData
    {
        foreach (var child in contentLoader.GetChildren<T>(pageLink))
        {
            yield return child;
        }

        foreach (var folder in contentLoader.GetChildren<FolderPage>(pageLink))
        {
            foreach (var nestedChild in contentLoader.FindPagesRecursively<T>(folder.PageLink))
            {
                yield return nestedChild;
            }
        }
    }

    public static IEnumerable<T> GetAllBlocksFromContentAssetsFolder<T>(this ContentReference rootLink)
        where T : BlockData => _contentLoader.Value.GetChildren<T>(rootLink);

    public static IEnumerable<T> GetDescendants<T>(this ContentReference rootLink) where T : IContent
    {
        foreach (var child in _contentLoader.Value.GetChildren<IContent>(rootLink))
        {
            if (child is T content)
            {
                yield return content;
            }

            foreach (var descendant in GetDescendants<T>(child.ContentLink))
            {
                if (descendant is { } contentChild)
                {
                    yield return contentChild;
                }
            }
        }
    }

    public static T GetContent<T>(ContentReference contentReference) where T : IContentData
    {
        if (contentReference == null)
        {
            return default;
        }

        return _contentRepository.Value.TryGet(contentReference, out T content) ? content : default;
    }

    public static IEnumerable<T> GetAncestorsOrSelfContent<T>(this ContentReference rootLink) where T : PageData
    {
        if (rootLink.Get<IContent>() is T currentContent)
        {
            yield return currentContent;
        }

        foreach (var parent in _contentLoader.Value.GetAncestors(rootLink).OfType<T>())
        {
            yield return parent;
        }
    }

    public static IEnumerable<T> GetAncestorsContent<T>(this ContentReference rootLink) where T : PageData
    {
        foreach (var parent in _contentLoader.Value.GetAncestors(rootLink).OfType<T>())
        {
            yield return parent;
        }
    }

    public static IEnumerable<T> GetAll<T>(this ContentReference rootLink) where T : PageData
    {
        var children = _contentLoader.Value.GetChildren<PageData>(rootLink);
        foreach (var child in children)
        {
            if (child is T childOfRequestedTyped)
            {
                yield return childOfRequestedTyped;
            }

            foreach (var descendant in GetAll<T>(child.ContentLink))
            {
                yield return descendant;
            }
        }
    }

    public static IEnumerable<T> GetChildren<T>(this ContentReference rootLink) where T : PageData
    {
        var children = _contentLoader.Value.GetChildren<PageData>(rootLink);
        foreach (var child in children)
        {
            if (child is T childOfRequestedTyped)
            {
                yield return childOfRequestedTyped;
            }
        }
    }

    public static IEnumerable<PageData> FindPagesByPageType(this ContentReference pageLink, bool recursive,
        int pageTypeId)
    {
        if (ContentReference.IsNullOrEmpty(pageLink))
        {
            throw new ArgumentNullException(nameof(pageLink), "No page link specified, unable to find pages");
        }

        return recursive
            ? FindPagesByPageTypeRecursively(pageLink, pageTypeId)
            : _contentLoader.Value.GetChildren<PageData>(pageLink);
    }

    private static IEnumerable<PageData> FindPagesByPageTypeRecursively(ContentReference pageLink, int pageTypeId)
    {
        var criteria = new PropertyCriteriaCollection
        {
            new()
            {
                Name = "PageTypeID",
                Type = PropertyDataType.PageType,
                Condition = CompareCondition.Equal,
                Value = pageTypeId.ToString(CultureInfo.InvariantCulture)
            }
        };

        if (!_providerManager.Value.ProviderMap.CustomProvidersExist)
        {
            return _pageCriteriaQueryService.Value.FindPagesWithCriteria(pageLink.ToPageReference(), criteria);
        }

        var contentProvider = _providerManager.Value.ProviderMap.GetProvider(pageLink);
        if (contentProvider.HasCapability(ContentProviderCapabilities.Search))
        {
            criteria.Add(new PropertyCriteria { Name = "EPI:MultipleSearch", Value = contentProvider.ProviderKey });
        }

        return _pageCriteriaQueryService.Value.FindPagesWithCriteria(pageLink.ToPageReference(), criteria);
    }

    /// <summary>
    ///     Helper method to get a URL string for a content reference using the PreferredCulture
    /// </summary>
    /// <param name="contentRef">The content reference of a routable content item to get the URL for.</param>
    /// <param name="isAbsolute">Whether the full URL including protocol and host should be returned.</param>
    public static Uri GetUri(this ContentReference contentRef, bool isAbsolute = false) =>
        contentRef.GetUri(ContentLanguage.PreferredCulture.Name, isAbsolute);

    /// <summary>
    ///     Helper method to get a URL string for a content reference using the provided culture code
    /// </summary>
    /// <param name="contentRef">The content reference of a routable content item to get the URL for.</param>
    /// <param name="lang">The language code to use when retrieving the URL.</param>
    /// <param name="isAbsolute">Whether the full URL including protocol and host should be returned.</param>
    public static Uri GetUri(this ContentReference contentRef, string lang, bool isAbsolute = false)
    {
        var cacheName = $"GetUri{contentRef.ID}|{lang}|{isAbsolute}";

        if (CacheManager.Get(cacheName) is Uri viewModel)
        {
            return viewModel;
        }

        var urlString = _urlResolver.Value.GetUrl(contentRef, lang, new UrlResolverArguments { ForceCanonical = true });
        if (string.IsNullOrEmpty(urlString))
        {
            return null;
        }

        var eviction = new CacheEvictionPolicy(TimeSpan.FromMinutes(30), CacheTimeoutType.Absolute);

        //if we're not getting an absolute URL, we don't need to work out the correct host name so exit here
        var uri = new Uri(urlString, UriKind.RelativeOrAbsolute);
        if (uri.IsAbsoluteUri || !isAbsolute)
        {
            CacheManager.Insert(cacheName, uri, eviction);
            return uri;
        }

        //Work out the correct domain to use from the hosts defined in the site definition
        var siteDefinition = _siteDefinitionResolver.Value.GetByContent(contentRef, true, true);
        var host = siteDefinition.Hosts.FirstOrDefault(h => h.Type == HostDefinitionType.Primary) ??
                   siteDefinition.Hosts.FirstOrDefault(h => h.Type == HostDefinitionType.Undefined);
        if (host == null)
        {
            return null;
        }

        var baseUrl = (host.Name ?? "*").Equals("*")
            ? siteDefinition.SiteUrl
            : new Uri($"http{(host.UseSecureConnection ?? false ? "s" : string.Empty)}://{host.Name}");

        uri = new Uri(baseUrl, urlString);
        CacheManager.Insert(cacheName, uri, eviction);

        return uri;
    }

    public static string GetSvgImage(this VectorImageMediaData file)
    {
        if (file == null)
        {
            return null;
        }

        try
        {
            string data;
            using (var reader = file.BinaryData.OpenRead())
            {
                data = new StreamReader(reader).ReadToEnd();
            }

            var doc = new HtmlDocument();
            doc.LoadHtml(data);

            var svg = doc.DocumentNode.Descendants("svg").FirstOrDefault();
            if (svg is { HasChildNodes: true } &&
                svg.ChildNodes.All(x => !x.Name.Equals("title", StringComparison.InvariantCultureIgnoreCase)))
            {
                var title = HtmlNode.CreateNode($"<title>{file.AltText}</title>");
                svg.PrependChild(title);
                return svg.OuterHtml;
            }

            return data;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static string GetCategoryTitle(this ContentReference contentRef)
    {
        if (contentRef == null)
        {
            return string.Empty;
        }

        var page = _contentLoader.Value.Get<CategoryData>(contentRef);
        return page == null ? string.Empty : page.Name;
    }

    public static List<T> GetCategoryDataChildren<T>(this ContentReference rootLink) where T : CategoryData
    {
        var lstCategories = new List<T>();
        if (rootLink == null)
        {
            return lstCategories;
        }

        var children = _contentLoader.Value.GetChildren<CategoryData>(rootLink);
        foreach (var child in children)
        {
            if (child is T childOfRequestedTyped)
            {
                lstCategories.Add(childOfRequestedTyped);
            }
        }

        return lstCategories;
    }

    public static List<T> GetCategoryDataChildrenByLanguage<T>(this ContentReference rootLink, string language)
        where T : CategoryData
    {
        var lstCategories = new List<T>();
        if (rootLink == null)
        {
            return lstCategories;
        }

        var children = !string.IsNullOrEmpty(language)
            ? _contentLoader.Value.GetChildren<CategoryData>(rootLink, new CultureInfo(language))
            : _contentLoader.Value.GetChildren<CategoryData>(rootLink);

        foreach (var child in children)
        {
            if (child is T childOfRequestedTyped)
            {
                lstCategories.Add(childOfRequestedTyped);
            }
        }

        return lstCategories;
    }
}