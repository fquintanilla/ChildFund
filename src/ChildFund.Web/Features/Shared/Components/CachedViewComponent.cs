using ChildFund.Web.Infrastructure.Cms.Services;
using EPiServer.Framework.Cache;

namespace ChildFund.Web.Features.Shared.Components;

public abstract class CachedViewComponent : ViewComponent
{
    private readonly IDataService _dataService;

    protected CachedViewComponent(IDataService dataService) => _dataService = dataService;

    protected T GetModel<T>(ContentReference reference, string key, Func<T> viewModelGenerationMethod)
        where T : ViewComponent
    {
        if (ContentReference.IsNullOrEmpty(reference))
        {
            return viewModelGenerationMethod.Invoke();
        }

        var groups = _dataService.GetVisitorGroupIdsByCurrentUser(HttpContext);
        var repository = ServiceLocator.Current.GetInstance<IContentVersionRepository>();
        var lastReferenceId = repository.List(reference).First().ContentLink.WorkID;

        var version = reference.ID + "_" + lastReferenceId + "_" + groups;
        var cacheKey = $"component_viewmodel_{key}_{version}";

        var model = (T)CacheManager.Get(cacheKey);
        if (model != null)
        {
            return model;
        }

        model = viewModelGenerationMethod.Invoke();

        var eviction = new CacheEvictionPolicy(TimeSpan.FromMinutes(60), CacheTimeoutType.Absolute);
        CacheManager.Insert(cacheKey, model, eviction);

        return model;
    }
}