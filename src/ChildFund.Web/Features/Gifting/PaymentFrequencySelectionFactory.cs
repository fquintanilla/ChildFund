namespace ChildFund.Web.Features.Gifting
{
    public class PaymentFrequencySelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            yield return new SelectItem { Text = "One-Time", Value = "OneTime" };
            yield return new SelectItem { Text = "Monthly", Value = "Monthly" };
            yield return new SelectItem { Text = "Yearly", Value = "Yearly" };
        }
    }
}
