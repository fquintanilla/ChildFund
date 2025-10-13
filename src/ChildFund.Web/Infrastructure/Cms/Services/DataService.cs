using ChildFund.Web.Features.Common.Pages.Folder;
using ChildFund.Web.Features.Shared.Pages;
using EPiServer.Personalization.VisitorGroups;

namespace ChildFund.Web.Infrastructure.Cms.Services;

public interface IDataService
{
    string GetUrl(IContent content);
    List<T> GetContentFromArea<T>(ContentArea area) where T : IContentData;
    FoundationPageData GetCurrentPage();
    FoundationPageData GetFolderPageRenerableParent(FolderPage folderPage);
    FoundationPageData GetParent(IContent currentPage = null);
    string GetVisitorGroupIdsByCurrentUser(HttpContext httpContext);
    IEnumerable<T> GetChildren<T>(IContent page) where T : IContent;
    IEnumerable<T> GetChildren<T>(ContentReference reference) where T : IContent;
}

public class DataService : IDataService
{
    private readonly IContentLoader _contentLoader;
    private readonly IContentRepository _contentRepository;
    private readonly IContentRouteHelper _contentRouteHelper;
    private readonly UrlResolver _urlResolver;
    private readonly IVisitorGroupRepository _visitorGroupRepository;
    private readonly IVisitorGroupRoleRepository _visitorGroupRoleRepository;
    private readonly ICacheService _cacheService;

    private readonly int _breadcrumbsCacheDuration = 30; //minutes

    public DataService(IContentRepository contentRepository, IContentLoader contentLoader,
        IContentRouteHelper contentRouteHelper, IVisitorGroupRepository visitorGroupRepository,
        IVisitorGroupRoleRepository visitorGroupRoleRepository,
        UrlResolver urlResolver, ICacheService cacheService)
    {
        _contentRepository = contentRepository;
        _contentLoader = contentLoader;
        _contentRouteHelper = contentRouteHelper;
        _visitorGroupRepository = visitorGroupRepository;
        _visitorGroupRoleRepository = visitorGroupRoleRepository;
        _urlResolver = urlResolver;
        _cacheService = cacheService;
    }

    public string GetUrl(IContent content) => _urlResolver.GetUrl(content.ContentLink);

    public List<T> GetContentFromArea<T>(ContentArea area) where T : IContentData
    {
        var results = new List<T>();

        if (area?.FilteredItems == null)
        {
            return results;
        }

        foreach (var item in area.FilteredItems)
        {
            if (!_contentLoader.TryGet(item.ContentLink, out IContentData block))
            {
                continue;
            }

            if (block is T typeBlock)
            {
                results.Add(typeBlock);
            }
        }

        return results;
    }

    public FoundationPageData GetCurrentPage() => (FoundationPageData)_contentRouteHelper.Content;

    public FoundationPageData GetFolderPageRenerableParent(FolderPage folderPage)
    {
        if (folderPage == null)
        {
            return null;
        }

        var filter = new FilterContentForVisitor();

        var parent = _contentLoader.GetAncestors(folderPage.ContentLink)
            .OfType<FoundationPageData>()
            .FirstOrDefault(page => !filter.ShouldFilter(page));

        return parent;
    }

    public FoundationPageData GetParent(IContent currentPage = null)
    {
        if (currentPage == null)
        {
            currentPage = GetCurrentPage();
        }

        var parent = _contentLoader.Get<FoundationPageData>(currentPage.ParentLink);
        return parent;
    }

    public string GetVisitorGroupIdsByCurrentUser(HttpContext httpContext)
    {
        var visitorGroupId = new List<string>();
        var user = httpContext.User;
        var visitorGroups = _visitorGroupRepository.List();

        foreach (var visitorGroup in visitorGroups)
        {
            if (!_visitorGroupRoleRepository.TryGetRole(visitorGroup.Name, out var virtualRoleObject))
            {
                continue;
            }

            if (virtualRoleObject.IsMatch(user, httpContext))
            {
                visitorGroupId.Add(visitorGroup.Id.ToString());
            }
        }

        return string.Join('|', visitorGroupId);
    }

    public IEnumerable<T> GetChildren<T>(IContent page) where T : IContent
    {
        var children = _contentRepository.GetChildren<T>(page.ContentLink);
        return children;
    }

    public IEnumerable<T> GetChildren<T>(ContentReference reference) where T : IContent
    {
        var children = _contentRepository.GetChildren<T>(reference);
        return children;
    }
}