using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;

namespace ChildFund.Web.Infrastructure.Cms.EditorDescriptors;

[EditorDescriptorRegistration(TargetType = typeof(ContentArea), UIHint = UIHint)]
public class OnlyDndContentAreaEditorDescriptor : ContentAreaEditorDescriptor
{
    public const string UIHint = "OnlyDndContentArea";

    public OnlyDndContentAreaEditorDescriptor() => ClientEditingClass = "contentAreaWithDndOnly/contentAreaEditor";
}