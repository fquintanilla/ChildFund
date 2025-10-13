using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;

namespace ChildFund.Infrastructure.Cms.EditorDescriptors;

[EditorDescriptorRegistration(TargetType = typeof(ContentArea), UIHint = "HideContentAreaActionsContainer")]
public class HideContentAreaActionsContainer : ContentAreaEditorDescriptor
{
    public override void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
    {
        base.ModifyMetadata(metadata, attributes);
        metadata.OverlayConfiguration["className"] = "epi-hide-actionscontainer";
    }
}