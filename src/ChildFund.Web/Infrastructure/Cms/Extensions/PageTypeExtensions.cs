namespace ChildFund.Web.Infrastructure.Cms.Extensions;

public static class PageTypeExtensions
{
    private static readonly Lazy<IContentTypeRepository<PageType>> _pageTypeRepository =
        new(() =>
            ServiceLocator.Current.GetInstance<IContentTypeRepository<PageType>>());

    public static PageType GetPageType(this Type pageType) => _pageTypeRepository.Value.Load(pageType);
}