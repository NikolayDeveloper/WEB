using System;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using NetCoreRules;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesRequest
{
    public class IsTodayIsDayRule : BaseRule<RequestWorkFlowRequest>
    {
        public bool Eval(DayOfWeek dayOfWeek)
        {
            var currentDayOfWeek = DateTimeOffset.Now.DayOfWeek;
            return currentDayOfWeek == dayOfWeek;
        }
    }
}
