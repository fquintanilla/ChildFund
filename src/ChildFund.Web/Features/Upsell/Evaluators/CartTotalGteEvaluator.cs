using ChildFund.Web.Features.Upsell.Models;
using System.Text.Json;

namespace ChildFund.Web.Features.Upsell.Evaluators
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
