using ChildFund.Features.Shared.Blocks;
using ChildFund.Features.Shared.Interfaces;
using EPiServer.Framework.Cache;
using EPiServer.Personalization.VisitorGroups;

namespace ChildFund.Features.Shared.Components;

public abstract class CachedBlockComponent<TBlockData> : PartialContentComponent<TBlockData>
    where TBlockData : FoundationBlockData
{
    protected T GetModel<T>(ContentReference reference, string key, Func<T> viewModelGenerationMethod)
        where T : class, ICachedBlockViewModel<TBlockData>
    {
        if (ContentReference.IsNullOrEmpty(reference))
        {
            return viewModelGenerationMethod.Invoke();
        }

        var groups = GetVisitorGroupIdsByCurrentUser(HttpContext);
        var repository = ServiceLocator.Current.GetInstance<IContentVersionRepository>();
        var lastReferenceId = repository.List(reference).First().ContentLink.WorkID;

        var version = reference.ID + "_" + lastReferenceId + "_" + groups;
        var cacheKey = $"block_viewmodel_{key}_{version}";

        var model = (T)CacheManager.Get(cacheKey);
        if (model != null)
        {
            return model;
        }

        model = viewModelGenerationMethod.Invoke();
        model.Version = version;

        var eviction = new CacheEvictionPolicy(TimeSpan.FromMinutes(60), CacheTimeoutType.Absolute);
        CacheManager.Insert(cacheKey, model, eviction);

        return model;
    }

    private string GetVisitorGroupIdsByCurrentUser(HttpContext httpContext)
    {
        var visitorGroupRepository = ServiceLocator.Current.GetInstance<IVisitorGroupRepository>();
        var visitorGroupRoleRepository = ServiceLocator.Current.GetInstance<IVisitorGroupRoleRepository>();

        var visitorGroupId = new List<string>();
        var user = httpContext.User;
        var visitorGroups = visitorGroupRepository.List();

        foreach (var visitorGroup in visitorGroups)
        {
            if (!visitorGroupRoleRepository.TryGetRole(visitorGroup.Name, out var virtualRoleObject))
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
}