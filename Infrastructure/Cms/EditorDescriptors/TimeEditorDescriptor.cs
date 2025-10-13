using ChildFund.Infrastructure.Cms.Constants;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;

namespace ChildFund.Infrastructure.Cms.EditorDescriptors;

[EditorDescriptorRegistration(TargetType = typeof(DateTime?), UIHint = CmsUiHints.Time)]
[EditorDescriptorRegistration(TargetType = typeof(DateTime), UIHint = CmsUiHints.Time)]
[EditorDescriptorRegistration(TargetType = typeof(string), UIHint = CmsUiHints.Time)]
public class TimeEditorDescriptor : EditorDescriptor
{
    public override void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
    {
        ClientEditingClass = "dijit/form/TimeTextBox";

        base.ModifyMetadata(metadata, attributes);
    }
}