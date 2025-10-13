using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;

namespace ChildFund.Infrastructure.Cms.EditorDescriptors;

[EditorDescriptorRegistration(TargetType = typeof(ContentData))]
public class MoveCategoryEditorDescriptor : EditorDescriptor
{
    public override void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
    {
        foreach (var property in metadata.Properties)
        {
            if (property.PropertyName == "icategorizable_category")
            {
                property.GroupName = SystemTabNames.PageHeader;
                property.Order = 9000;
            }
        }
    }
}