using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;

namespace Iserv.Niis.WorkflowServices
{
    public interface IWorkflowServiceRequest
    {
        void Process(RequestWorkFlowRequest requestWorkFlowRequest);
    }
}
