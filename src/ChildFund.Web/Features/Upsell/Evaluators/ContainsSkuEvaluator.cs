using ChildFund.Web.Features.Upsell.Models;
using System.Text.Json;

namespace ChildFund.Web.Features.Upsell.Evaluators
{
    public class ContainsSkuEvaluator : IConditionEvaluator
    {
        public bool CanHandle(string op) => op.Equals("ContainsSku", StringComparison.OrdinalIgnoreCase);
        public bool Evaluate(string op, JsonElement operand, CartContext ctx)
        {
            var sku = operand.GetString();
            return !string.IsNullOrWhiteSpace(sku) && ctx.CartSkus.Contains(sku);
        }
    }
}
