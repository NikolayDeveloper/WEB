using Iserv.Niis.DataBridge.Implementations;
using System;
//using NetCoreDI;

namespace NetCoreRules
{
    public class RuleExecutor : IRuleExecutor
    {
        public IRuleExecutor<TRule> GetRule<TRule>()
        {
            var rule = AmbientContext.Current.Resolver.ResolveObject<TRule>();
            return new RuleExecutor<TRule>(rule);
        }
    }

    public class RuleExecutor<TRule> : IRuleExecutor<TRule>
    {
        private readonly TRule _rule;

        public RuleExecutor(TRule rule)
        {
            _rule = rule;
        }

        public bool Process(Func<TRule, bool> ruleFunc)
        {
            return ruleFunc(_rule);
        }

        public void InitRule(Action<TRule> ruleFunc)
        {
            ruleFunc(_rule);
        }
    }
}
