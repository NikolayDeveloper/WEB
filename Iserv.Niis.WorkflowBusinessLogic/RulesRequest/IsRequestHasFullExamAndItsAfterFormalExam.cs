using System;
using System.Linq;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.WorkflowBusinessLogic.RequestWorkflows;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using NetCoreRules;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesRequest
{
    public class IsRequestHasFullExamAndItsAfterFormalExam : BaseRule<RequestWorkFlowRequest>
    {
        public bool Eval()
        {
            var lastFullExamWorkflow = Executor.GetQuery<GetLastRequestWorkflowByRequestIdStageCodeQuery>()
                .Process(r => r.Execute(WorkflowRequest.RequestId, RouteStageCodes.TZ_03_3_2));

            if (lastFullExamWorkflow == null)
                return false;

            var lastFormalExamWorkflow = Executor.GetQuery<GetLastRequestWorkflowByRequestIdStageCodeQuery>()
                .Process(r => r.Execute(WorkflowRequest.RequestId, RouteStageCodes.TZ_03_2_2));

            var fullExamDate = lastFullExamWorkflow.DateCreate;
            var formalExamDate = lastFormalExamWorkflow?.DateCreate ?? DateTimeOffset.MinValue;

            return  fullExamDate > formalExamDate;
        }
    }
}