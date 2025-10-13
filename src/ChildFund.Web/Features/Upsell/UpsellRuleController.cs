using ChildFund.Web.Features.Upsell.Infrastructure;
using ChildFund.Web.Features.Upsell.Models;
using ChildFund.Web.Features.Upsell.Services;

namespace ChildFund.Web.Features.Upsell
{
    public class UpsellRuleController(
        IUpsellSelectorService selector,
        ICartContextProvider cartContextProvider)
        : BlockComponent<UpsellRuleBlock>
    {
        protected override IViewComponentResult InvokeComponent(UpsellRuleBlock currentBlock)
        {
            if (!currentBlock.Enabled)
            {
                return Content(string.Empty);
            }

            var ctx = cartContextProvider.Get();
            var rule = currentBlock.ToUpsellRule();

            var variants = selector
                .GetUpsells([rule], ctx, rule.MaxSuggestions)
                .Take(rule.MaxSuggestions)
                .ToList();

            if (!variants.Any())
            {
                return Content(string.Empty);
            }

            var vm = new UpsellRuleViewModel
            {
                RuleName = currentBlock.RuleName,
                Variants = variants.ToList()
            };

            return View("~/Features/Upsell/UpsellRuleBlock.cshtml", vm);
        }
    }
}
