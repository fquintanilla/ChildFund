using EPiServer.Shell.ObjectEditing;

namespace ChildFund.Features.CatalogContent.Infrastructure
{
    public class RecurrenceSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new List<ISelectItem>
            {
                new SelectItem { Text = "One-time", Value = "One-time" },
                new SelectItem { Text = "Yearly", Value = "Yearly" },
                new SelectItem { Text = "Monthly", Value = "Monthly" },
                new SelectItem { Text = "Quarterly", Value = "Quarterly" },
                new SelectItem { Text = "Semi-Annually", Value = "Semi-Annually" },
                new SelectItem { Text = "Annually", Value = "Annually" },
            };
        }
    }
}
