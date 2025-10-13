using ChildFund.Web.Features.Upsell.Models;
using System.Text.Json;

namespace ChildFund.Web.Features.Upsell.Evaluators
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
