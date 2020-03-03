using System;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowProtectionDocument;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowTaskQueues;
using NetCoreRules;

namespace Iserv.Niis.WorkflowBusinessLogic.ProtectionDocuments
{
    public class IsProtectionDocExpiredOnCurrentStageRule: BaseRule<ProtectionDocumentWorkFlowRequest>
    {
        public bool Eval(string nextStageCode)
        {
            var expireDate = Executor.GetHandler<CalculateWorkflowTaskQueueResolveDateHandler>().Process(h => h.Execute(WorkflowRequest.ProtectionDocId, Owner.Type.ProtectionDoc, nextStageCode));
            var isExpired = DateTimeOffset.Now > expireDate;

            return isExpired;
        }
    }
}
