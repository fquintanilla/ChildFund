using EPiServer.PlugIn;
using EPiServer.Scheduler;

namespace ChildFund.Infrastructure.Jobs;

[ScheduledPlugIn(
    DisplayName = "[ChildFund] Remove unused properties on content types",
    Description = "",
    SortIndex = 220,
    GUID = Guid)]
public class RemoveUnusedContentPropertiesJob : ScheduledJobBase
{
    public const string Guid = "A194D4A3-30AA-46EB-9D9B-302E3C141E8A";
    private readonly ContentTypeModelRepository _contentTypeModelRepository;
    private readonly IContentTypeRepository _contentTypeRepository;
    private readonly IPropertyDefinitionRepository _propertyDefinitionRepository;

    public RemoveUnusedContentPropertiesJob(
        IContentTypeRepository contentTypeRepository,
        IPropertyDefinitionRepository propertyDefinitionRepository,
        ContentTypeModelRepository contentTypeModelRepository)
    {
        _contentTypeRepository = contentTypeRepository;
        _propertyDefinitionRepository = propertyDefinitionRepository;
        _contentTypeModelRepository = contentTypeModelRepository;
        IsStoppable = false;
    }

    public override string Execute()
    {
        OnStatusChanged("Starting job");

        var propertiesRemoved = 0;

        var contentTypes = _contentTypeRepository.List();

        var messages = new List<string>();
        // Filter out Episerver internal properties
        contentTypes = contentTypes.Where(ct => !string.IsNullOrEmpty(ct.ModelTypeString));

        var excludedContentTypes = new List<string>
        {
            "SiteUrlPredefinedHiddenElementBlock",
            "TextboxElementBlock",
            "TextareaElementBlock",
            "NumberElementBlock",
            "RangeElementBlock",
            "UrlElementBlock",
            "SelectionElementBlock",
            "ChoiceElementBlock",
            "ImageChoiceElementBlock",
            "PredefinedHiddenElementBlock",
            "VisitorDataHiddenElementBlock",
            "FormContainerBlock"
        };

        foreach (var contentType in contentTypes)
        {
            //for some reason epi form fields show up in results

            if (excludedContentTypes.Contains(contentType.Name) || contentType.Name.Contains("ElementBlock"))
            {
                continue;
            }

            // Delete all properties that do not exist in code (probably due to renaming or removing them)
            var propertiesToRemove = contentType.PropertyDefinitions.Where(IsMissingModelProperty);

            foreach (var property in propertiesToRemove)
            {
                _propertyDefinitionRepository.Delete(property);
                ++propertiesRemoved;
                var type = _contentTypeRepository.Load(property.ContentTypeID);
                var typeName = "undefined";
                if (type != null)
                {
                    typeName = type.Name;
                }

                messages.Add($" {property.Name} in {typeName}");
            }

            OnStatusChanged($"{propertiesRemoved} properties removed");
        }

        return $"{propertiesRemoved} properties removed: {string.Join(",", messages)}";
    }

    private bool IsMissingModelProperty(PropertyDefinition propertyDefinition)
    {
        if (propertyDefinition == null)
        {
            return false;
        }

        if (!propertyDefinition.ExistsOnModel)
        {
            return true;
        }

        return _contentTypeModelRepository.GetPropertyModel(propertyDefinition.ContentTypeID, propertyDefinition) ==
               null;
    }
}