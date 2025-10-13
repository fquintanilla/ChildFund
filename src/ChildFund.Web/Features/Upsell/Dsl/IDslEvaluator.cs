using ChildFund.Web.Features.Upsell.Models;

namespace ChildFund.Web.Features.Upsell.Dsl
{
    public interface IDslEvaluator
    {
        bool Evaluate(string conditionsJson, CartContext ctx);
    }

}
