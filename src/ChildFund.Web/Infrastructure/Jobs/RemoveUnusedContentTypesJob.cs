using EPiServer.Security;

namespace ChildFund.Web.Infrastructure.Jobs;

[ScheduledPlugIn(
    DisplayName = "[ChildFund] Remove unused content types and instances aggressively",
    Description = "Aggressively deletes content types with no backing model and any content based off it",
    SortIndex = 210,
    GUID = Guid)]
public class RemoveUnusedContentTypesJob : ScheduledJobBase
{
    public const string Guid = "3A048CE8-A263-4E4E-8729-75152372D9E3";
    private const int EpiContentModelIdMax = 10;
    private readonly IContentModelUsage _contentModelUsage;
    private readonly IContentRepository _contentRepository;
    private readonly IContentTypeRepository _contentTypeRepository;

    public RemoveUnusedContentTypesJob(
        IContentTypeRepository contentTypeRepository,
        IContentModelUsage contentModelUsage,
        IContentRepository contentRepository)
    {
        _contentTypeRepository = contentTypeRepository;
        _contentModelUsage = contentModelUsage;
        _contentRepository = contentRepository;
        IsStoppable = false;
    }

    public override string Execute()
    {
        OnStatusChanged("Starting job");

        var contentTypes = _contentTypeRepository.List();

        var contentTypesWithoutBackingModel = contentTypes
            .Where(x => x.ModelType == null && x.ID > EpiContentModelIdMax)
            .ToList();

        var contentTypesToRemove = contentTypesWithoutBackingModel.Count;
        var removedContentTypes = 0;

        if (contentTypesToRemove == 0)
        {
            return "Did not find any types to remove.";
        }

        OnStatusChanged($"Found {contentTypesToRemove} content types without a backing model");

        //EPiServer.Security.PrincipalInfo = new GenericPrincipal(
        //new GenericIdentity("xyz"),
        //new[] { "Administrators" });
        var errorList = new List<string>();


        foreach (var contentType in contentTypesWithoutBackingModel)
        {
            var usages = _contentModelUsage.ListContentOfContentType(contentType);

            if (usages.Any())
            {
                foreach (var contentUsage in usages)
                {
                    try
                    {
                        _contentRepository.DeleteChildren(contentUsage.ContentLink, true, AccessLevel.NoAccess);
                        _contentRepository.Delete(contentUsage.ContentLink, true, AccessLevel.NoAccess);
                    }
                    catch (Exception e)
                    {
                        errorList.Add(e.Message);
                        // ignored because the DeleteChildren will most likely
                        // already have deleted an type later in the loop
                    }
                }
            }

            OnStatusChanged($"Removed unused content type {++removedContentTypes}/{contentTypesToRemove}");

            try
            {
                _contentTypeRepository.Delete(contentType);
            }
            catch (Exception e)
            {
                errorList.Add(e.Message);
                OnStatusChanged($"{e.Message}");
            }
        }

        var removedTypes = contentTypesWithoutBackingModel.Select(x => x.Name).ToList();

        return
            $"Removed {contentTypesToRemove} unused content types: {string.Join(", ", removedTypes)}. Messages: {string.Join(", ", errorList)}";
    }
}