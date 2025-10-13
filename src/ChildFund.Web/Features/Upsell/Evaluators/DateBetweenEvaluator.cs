using ChildFund.Web.Features.Upsell.Models;
using System.Text.Json;

namespace ChildFund.Web.Features.Upsell.Evaluators
{
    public class DateBetweenEvaluator : IConditionEvaluator
    {
        public bool CanHandle(string op) => op.Equals("DateBetween", StringComparison.OrdinalIgnoreCase);
        public bool Evaluate(string op, JsonElement operand, CartContext ctx)
        {
            if (operand.ValueKind != JsonValueKind.Array || operand.GetArrayLength() != 2) return false;
            var start = DateTime.Parse(operand[0].GetString(), CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
            var end = DateTime.Parse(operand[1].GetString(), CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
            var now = ctx.UtcNow;
            return now >= start && now <= end;
        }
    }
}
