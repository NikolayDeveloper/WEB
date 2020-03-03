using System;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowTaskQueues;
using NetCoreRules;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesRequest
{
    public class IsRequestExpiredOnCurrentStageRule : BaseRule<RequestWorkFlowRequest>
    {
        private readonly ICalendarProvider _calendarProvider;

        public IsRequestExpiredOnCurrentStageRule(ICalendarProvider calendarProvider)
        {
            _calendarProvider = calendarProvider;
        }

        public bool Eval(string nextStageCode)
        {
            DateTimeOffset expireDate;
            bool isExpired;
            if (Properties.Settings.Default.IsAutoWorkflowInTestMode == true)
            {
                var currentStageDate = WorkflowRequest.CurrentWorkflowObject.CurrentWorkflow.DateCreate;
                expireDate = currentStageDate + Properties.Settings.Default.AutoWorkflowTestDelayTime;
                isExpired = DateTimeOffset.Now > expireDate;
                return isExpired;
            }
            expireDate = Executor.GetHandler<CalculateWorkflowTaskQueueResolveDateHandler>().Process(h => h.Execute(WorkflowRequest.RequestId, Owner.Type.Request, nextStageCode));
            isExpired = DateTimeOffset.Now > expireDate;

            return isExpired;
        }
    }
}