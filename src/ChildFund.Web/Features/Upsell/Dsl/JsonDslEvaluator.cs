using ChildFund.Web.Features.Upsell.Evaluators;
using ChildFund.Web.Features.Upsell.Models;
using System.Text.Json;

namespace ChildFund.Web.Features.Upsell.Dsl
{
    public class JsonDslEvaluator(IConditionRegistry registry, ILogger<JsonDslEvaluator> logger) : IDslEvaluator
    {
        public bool Evaluate(string conditionsJson, CartContext ctx)
        {
            if (string.IsNullOrWhiteSpace(conditionsJson)) return true; // treat empty as match
            using var doc = JsonDocument.Parse(conditionsJson);
            return EvalNode(doc.RootElement, ctx);
        }

        private bool EvalNode(JsonElement node, CartContext ctx)
        {
            // Group operators
            if (node.TryGetProperty("All", out var all))
                return EvalAll(all, ctx);
            if (node.TryGetProperty("Any", out var any))
                return EvalAny(any, ctx);
            if (node.TryGetProperty("Not", out var not))
                return !EvalNode(not, ctx);

            // Leaf operator: { "OperatorName": operand }
            foreach (var prop in node.EnumerateObject())
            {
                var op = prop.Name;
                var operand = prop.Value;
                if (registry.TryGet(op, out var evaluator))
                    return evaluator.Evaluate(op, operand, ctx);

                // Unknown operator => treat as false for safety
                logger.LogError("Unknown DSL operator: {Operator}", op);
                return false;
            }

            // Empty object => true (e.g., { "All": [] })
            return true;
        }

        private bool EvalAll(JsonElement array, CartContext ctx)
        {
            if (array.ValueKind != JsonValueKind.Array) return false;
            foreach (var item in array.EnumerateArray())
                if (!EvalNode(item, ctx)) return false;
            return true;
        }

        private bool EvalAny(JsonElement array, CartContext ctx)
        {
            if (array.ValueKind != JsonValueKind.Array) return false;
            foreach (var item in array.EnumerateArray())
                if (EvalNode(item, ctx)) return true;
            return false;
        }
    }
}
