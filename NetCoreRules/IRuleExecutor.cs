using System;

namespace NetCoreRules
{
    public interface IRuleExecutor
    {
        IRuleExecutor<TRule> GetRule<TRule>();
    }

    public interface IRuleExecutor<out TRule>
    {
        bool Process(Func<TRule, bool> ruleFunc);
        void InitRule(Action<TRule> ruleFunc);
    }
}