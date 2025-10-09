namespace ChildFund.Features.Upsell.Evaluators
{
    public class ConditionRegistry : IConditionRegistry
    {
        private readonly Dictionary<string, IConditionEvaluator> _map;

        public ConditionRegistry(IEnumerable<IConditionEvaluator> evaluators)
        {
            var conditionEvaluators = evaluators.ToList();

            _map = conditionEvaluators
                .SelectMany(GetSupported)
                .ToDictionary(k => k, k => conditionEvaluators.First(e => e.CanHandle(k)),
                    StringComparer.OrdinalIgnoreCase);
        }

        private static IEnumerable<string> GetSupported(IConditionEvaluator ev)
        {
            // Ask evaluator what it supports by probing common names.
            // Alternatively, expose IAdvertiseOperators with a Supported set.
            var known = new[]
            {
                "CartTotalGreaterOrEqual", 
                "CartTotalLessOrEqual", 
                "ContainsSku",
                "ContainsTag", 
                "DateBetween", 
                "CustomerSegmentIn"
            };

            return known.Where(ev.CanHandle);
        }

        public bool TryGet(string operatorName, out IConditionEvaluator evaluator)
            => _map.TryGetValue(operatorName, out evaluator);
    }
}
