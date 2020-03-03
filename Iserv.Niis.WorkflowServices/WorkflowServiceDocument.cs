using Iserv.Niis.WorkflowBusinessLogic.Document;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowDocuments;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using NetCoreWorkflow;
using System;
using System.Linq;
using Iserv.Niis.Domain.Entities.Document;

namespace Iserv.Niis.WorkflowServices
{
    public class WorkflowServiceDocument : IWorkflowServiceDocument
    {
        private readonly IExecutor _executor;
        public WorkflowServiceDocument(IExecutor executor)
        {
            _executor = executor;
        }

        public void Process(DocumentWorkFlowRequest contractWorkFlowRequest)
        {
            if (contractWorkFlowRequest.DocumentId != 0)
            {
                contractWorkFlowRequest.CurrentWorkflowObject = _executor.GetQuery<GetDocumentByIdForWorkflowServiceQuery>().Process(r => r.Execute(contractWorkFlowRequest.DocumentId));
                if(contractWorkFlowRequest.IsAuto)
                {
                    var currentUserId = 0;

                    var curentWorkflow = contractWorkFlowRequest.CurrentWorkflowObject.CurrentWorkflows.FirstOrDefault(d => d.CurrentStage.Code == contractWorkFlowRequest.PrevStageCode);

                    if (curentWorkflow != null)
                        currentUserId = curentWorkflow.CurrentUserId.GetValueOrDefault(0);
                    else if (contractWorkFlowRequest.CurrentWorkflowObject.CurrentWorkflows.Count >= 1)
                        currentUserId = contractWorkFlowRequest.CurrentWorkflowObject.CurrentWorkflows.First().CurrentUserId.GetValueOrDefault(0);

                    contractWorkFlowRequest.NextStageUserId = currentUserId;
                }

                RequestWorkflows(contractWorkFlowRequest);
            }
        }

        private void RequestWorkflows(DocumentWorkFlowRequest contractWorkFlowRequest)
        {
            NetCoreBaseWorkflow<DocumentWorkFlowRequest, Document> contractWorkFlow = NiisWorkflowAmbientContext.Current.CommonDocumentWorkflow;

            if (contractWorkFlow == null)
            {
                throw new NotImplementedException();
            }

            contractWorkFlow.SetWorkflowRequest(contractWorkFlowRequest);

            contractWorkFlow.Process();
        }
    }
}
