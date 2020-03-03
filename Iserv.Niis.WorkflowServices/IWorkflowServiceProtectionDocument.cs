using Iserv.Niis.WorkflowBusinessLogic.WorkflowProtectionDocument;

namespace Iserv.Niis.WorkflowServices
{
    public interface IWorkflowServiceProtectionDocument
    {
        void Process(ProtectionDocumentWorkFlowRequest requestWorkFlowRequest, int? specialUserId = null);
    }
}
