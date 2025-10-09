using EPiServer.Shell.ObjectEditing;

namespace ChildFund.Features.Upsell.Infrastructure
{
    public class SortModeSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new List<ISelectItem>
            {
                new SelectItem { Text = "Sequence Ascending", Value = "SequenceAsc" },
                new SelectItem { Text = "Price Ascending", Value = "PriceAsc" },
                new SelectItem { Text = "Price Descending", Value = "PriceDesc" },
                new SelectItem { Text = "Random (within top N)", Value = "RandomWithinTopN" }
            };
        }
    }
}
