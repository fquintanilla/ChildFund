namespace ChildFund.Web.Features.Common.Pages.Folder;

/// <summary>
/// Describes how the UI should appear for <see cref="FolderPage" /> content.
/// </summary>
[UIDescriptorRegistration]
public class FolderPageUiDescriptor : UIDescriptor<FolderPage>
{
    public FolderPageUiDescriptor()
        : base(ContentTypeCssClassNames.Folder)
    {
    }
}