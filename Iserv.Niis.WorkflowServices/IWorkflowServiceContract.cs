using Iserv.Niis.WorkflowBusinessLogic.WorkflowContracts;

namespace Iserv.Niis.WorkflowServices
{
    public interface IWorkflowServiceContract
    {
        void Process(ContractWorkFlowRequest requestWorkFlowRequest);
    }
}
