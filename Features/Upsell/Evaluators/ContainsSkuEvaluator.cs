using System.Text.Json;
using ChildFund.Features.Upsell.Models;

namespace ChildFund.Features.Upsell.Evaluators
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
