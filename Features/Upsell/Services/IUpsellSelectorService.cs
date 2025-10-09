using ChildFund.Features.Upsell.Models;

namespace ChildFund.Features.Upsell.Services
{
    public interface IUpsellSelectorService
    {
        IEnumerable<VariantView> GetUpsells(
            IEnumerable<UpsellRule> rulesInPriorityOrder,
            CartContext ctx,
            int max);
    }
}
