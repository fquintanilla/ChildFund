using System.Text.Json;
using ChildFund.Features.Upsell.Models;

namespace ChildFund.Features.Upsell.Evaluators
{
    public class CartTotalGteEvaluator : IConditionEvaluator
    {
        public bool CanHandle(string op) => op.Equals("CartTotalGreaterOrEqual", StringComparison.OrdinalIgnoreCase);
        public bool Evaluate(string op, JsonElement operand, CartContext ctx)
        {
            var threshold = operand.GetDecimal();
            return ctx.CartTotal >= threshold;
        }
    }
}
