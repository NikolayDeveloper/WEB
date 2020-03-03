using Iserv.Niis.WorkflowBusinessLogic.WorkflowDocuments;

namespace Iserv.Niis.WorkflowServices
{
    public interface IWorkflowServiceDocument
    {
        void Process(DocumentWorkFlowRequest requestWorkFlowRequest);
    }
}
