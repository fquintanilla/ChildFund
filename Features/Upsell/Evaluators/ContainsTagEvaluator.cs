using System.Text.Json;
using ChildFund.Features.Upsell.Models;

namespace ChildFund.Features.Upsell.Evaluators
{
    public class ContainsTagEvaluator : IConditionEvaluator
    {
        public bool CanHandle(string op) => op.Equals("ContainsTag", StringComparison.OrdinalIgnoreCase);
        public bool Evaluate(string op, JsonElement operand, CartContext ctx)
        {
            // Implement if your CartContext exposes cart tags.
            return false;
        }
    }
}
