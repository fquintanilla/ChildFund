using ChildFund.Features.Upsell.Models;
using System.Text.Json;

namespace ChildFund.Features.Upsell.Evaluators
{
    public class CustomerSegmentInEvaluator : IConditionEvaluator
    {
        public bool CanHandle(string op) => op.Equals("CustomerSegmentIn", StringComparison.OrdinalIgnoreCase);
        public bool Evaluate(string op, JsonElement operand, CartContext ctx)
        {
            if (operand.ValueKind != JsonValueKind.Array) return false;
            foreach (var el in operand.EnumerateArray())
            {
                var seg = el.GetString();
                if (!string.IsNullOrWhiteSpace(seg) && ctx.CustomerSegments.Contains(seg))
                    return true;
            }
            return false;
        }
    }
}
