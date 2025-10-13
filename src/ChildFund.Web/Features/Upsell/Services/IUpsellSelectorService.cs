using ChildFund.Web.Features.Upsell.Models;

namespace ChildFund.Web.Features.Upsell.Services
{
    public interface IUpsellSelectorService
    {
        IEnumerable<VariantView> GetUpsells(
            IEnumerable<UpsellRule> rulesInPriorityOrder,
            CartContext ctx,
            int max);
    }
}
