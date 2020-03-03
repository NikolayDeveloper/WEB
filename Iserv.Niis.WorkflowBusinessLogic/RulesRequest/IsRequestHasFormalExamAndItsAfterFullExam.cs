using System;
using System.Linq;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.WorkflowBusinessLogic.RequestWorkflows;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using NetCoreRules;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesRequest
{
    public class IsRequestHasFormalExamAndItsAfterFullExam : BaseRule<RequestWorkFlowRequest>
    {
        public bool Eval()
        {
            var lastFormalExamWorkflow = Executor.GetQuery<GetLastRequestWorkflowByRequestIdStageCodeQuery>()
                .Process(r => r.Execute(WorkflowRequest.RequestId, RouteStageCodes.TZ_03_2_2));

            if (lastFormalExamWorkflow == null)
                return false;

            var lastFullExamWorkflow = Executor.GetQuery<GetLastRequestWorkflowByRequestIdStageCodeQuery>()
                .Process(r => r.Execute(WorkflowRequest.RequestId, RouteStageCodes.TZ_03_3_2));

            var formalExamDate = lastFormalExamWorkflow.DateCreate;
            var fullExamDate = lastFullExamWorkflow?.DateCreate ?? DateTimeOffset.MinValue;

            return formalExamDate > fullExamDate;
        }
    }
}