using ChildFund.Features.Upsell.Models;

namespace ChildFund.Features.Upsell.Dsl
{
    public interface IDslEvaluator
    {
        bool Evaluate(string conditionsJson, CartContext ctx);
    }

}
