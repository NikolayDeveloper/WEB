using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.WorkflowBusinessLogic.Contracts;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowContracts;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using NetCoreWorkflow;
using System;

namespace Iserv.Niis.WorkflowServices
{
    public class WorkflowServiceContract : IWorkflowServiceContract
    {
        private readonly IExecutor _executor;
        public WorkflowServiceContract(IExecutor executor)
        {
            _executor = executor;
        }

        public void Process(ContractWorkFlowRequest contractWorkFlowRequest)
        {
            if (contractWorkFlowRequest.ContractId != 0)
            {
                //contractWorkFlowRequest.CurrentWorkflowObject = _executor.GetQuery<GetContractByIdQuery>().Process(r => r.Execute(contractWorkFlowRequest.ContractId));
                contractWorkFlowRequest.CurrentWorkflowObject = _executor.GetQuery<GetContractByIdForWorkflowServiceQuery>().Process(r => r.Execute(contractWorkFlowRequest.ContractId));
                if(contractWorkFlowRequest.IsAuto)
                {
                    contractWorkFlowRequest.NextStageUserId = contractWorkFlowRequest.CurrentWorkflowObject.CurrentWorkflow.CurrentUserId ?? 0;
                }

                RequestWorkflows(contractWorkFlowRequest);
            }
        }

        private void RequestWorkflows(ContractWorkFlowRequest contractWorkFlowRequest)
        {
            NetCoreBaseWorkflow<ContractWorkFlowRequest, Contract> contractWorkFlow = NiisWorkflowAmbientContext.Current.ContractWorkflow;
            
            if (contractWorkFlow == null)
            {
                throw new NotImplementedException();
            }

            contractWorkFlow.SetWorkflowRequest(contractWorkFlowRequest);

            contractWorkFlow.Process();
        }
    }
}
