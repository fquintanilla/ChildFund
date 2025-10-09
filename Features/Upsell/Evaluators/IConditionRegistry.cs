namespace ChildFund.Features.Upsell.Evaluators
{
    public interface IConditionRegistry
    {
        bool TryGet(string operatorName, out IConditionEvaluator evaluator);
    }
}
