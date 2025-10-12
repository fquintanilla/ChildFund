using ChildFund.Infrastructure.Cms.Constants;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;

namespace ChildFund.Infrastructure.Cms.EditorDescriptors;

[EditorDescriptorRegistration(TargetType = typeof(DateTime?), UIHint = CmsUiHints.Calendar)]
[EditorDescriptorRegistration(TargetType = typeof(DateTime), UIHint = CmsUiHints.Calendar)]
[EditorDescriptorRegistration(TargetType = typeof(string), UIHint = CmsUiHints.Calendar)]
public class CalendarEditorDescriptor : EditorDescriptor
{
    public override void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
    {
        ClientEditingClass = "dijit/form/DateTextBox";
        base.ModifyMetadata(metadata, attributes);
    }
}