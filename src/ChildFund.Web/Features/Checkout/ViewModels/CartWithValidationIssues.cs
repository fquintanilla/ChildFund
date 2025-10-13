namespace ChildFund.Web.Features.Checkout.ViewModels
{
    public class CartWithValidationIssues
    {
        public virtual ICart? Cart { get; set; }

        public virtual Dictionary<ILineItem, List<ValidationIssue>> ValidationIssues { get; set; }
    }
}
