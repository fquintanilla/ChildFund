using EPiServer.Shell.ObjectEditing;

namespace ChildFund.Infrastructure.Cms.SelectionFactories;

public class LinkTargetSelectionFactory : ISelectionFactory
{
    public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata) =>
        new ISelectItem[]
        {
            new SelectItem { Text = "", Value = null },
            new SelectItem { Text = "Open the link in a new window", Value = LinkTargetOptions.NewWindow },
            new SelectItem { Text = "Open the link in the whole window", Value = LinkTargetOptions.TopWindow }
        };

    public static class LinkTargetOptions
    {
        public const string NewWindow = "_blank";
        public const string TopWindow = "_top";
    }
}