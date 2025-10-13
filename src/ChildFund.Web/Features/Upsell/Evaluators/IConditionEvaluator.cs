using ChildFund.Web.Features.Upsell.Models;
using System.Text.Json;

namespace ChildFund.Web.Features.Upsell.Evaluators
{
    public interface IConditionEvaluator
    {
        /// <summary>True if this evaluator can handle the given JSON property name.</summary>
        bool CanHandle(string operatorName);

        /// <summary>Evaluates a single leaf condition node.</summary>
        bool Evaluate(string operatorName, JsonElement operand, CartContext ctx);
    }
}
