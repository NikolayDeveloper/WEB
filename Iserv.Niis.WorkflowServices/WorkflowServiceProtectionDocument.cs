using Iserv.Niis.Common.Codes;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.WorkflowBusinessLogic.ProtectionDocuments;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowProtectionDocument;
using Iserv.Niis.WorkflowBusinessLogic.Workflows;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using NetCoreWorkflow;
using System;
using System.Linq;

namespace Iserv.Niis.WorkflowServices
{
    public class WorkflowServiceProtectionDocument : IWorkflowServiceProtectionDocument
    {
        private readonly IExecutor _executor;
        public WorkflowServiceProtectionDocument(IExecutor executor)
        {
            _executor = executor;
        }

        public void Process(ProtectionDocumentWorkFlowRequest protectionDocumentWorkFlowRequest,
            int? specialUserId = null)
        {
            var current = NiisAmbientContext.Current;
            ProtectionDocWorkflow specWorkf = null;
            if (current != null && current.User != null && current.User.Identity != null)
                specialUserId = NiisAmbientContext.Current.User.Identity.UserId;
            if (specialUserId.HasValue && !_executor
                    .GetQuery<TryGetProtectionDocWorkflowFromParalleByOwnerIdCommand>()
                    .Process(r => r.Execute(protectionDocumentWorkFlowRequest.ProtectionDocId, specialUserId.Value,
                        out specWorkf))
                ||
                specWorkf != null &&
                new string[] {RouteStageCodes.OD03_1, RouteStageCodes.OD01_6}.Contains(specWorkf.CurrentStage.Code)
            )
            {
                if (protectionDocumentWorkFlowRequest.ProtectionDocId != 0)
                {
                    protectionDocumentWorkFlowRequest.CurrentWorkflowObject = _executor
                        .GetQuery<GetProtectionDocByIdForWorkflowServiceQuery>().Process(r =>
                            r.Execute(protectionDocumentWorkFlowRequest.ProtectionDocId, specialUserId));

                    if (protectionDocumentWorkFlowRequest.IsAuto)
                    {
                        protectionDocumentWorkFlowRequest.NextStageUserId =
                            protectionDocumentWorkFlowRequest?.CurrentWorkflowObject?.CurrentWorkflow?.CurrentUserId ??
                            0;
                    }


                    RequestWorkflows(protectionDocumentWorkFlowRequest);
                }
            }
        }

        private void RequestWorkflows(ProtectionDocumentWorkFlowRequest protectionDocumentWorkFlowRequest)
        {
            NetCoreBaseWorkflow<ProtectionDocumentWorkFlowRequest, ProtectionDoc> protectionDocumentWorkFlow;
            
            switch (protectionDocumentWorkFlowRequest.CurrentWorkflowObject.Type.Code)
            {
               case DicProtectionDocTypeCodes.ProtectionDocTypeTrademarkCode:
                case DicProtectionDocTypeCodes.RequestTypeTrademarkCode:
                    protectionDocumentWorkFlow = NiisWorkflowAmbientContext.Current.ProtectionDocumentTrademarkWorkflow;
                    break;

                case DicProtectionDocTypeCodes.ProtectionDocTypeInventionCode:
                case DicProtectionDocTypeCodes.RequestTypeInventionCode:
                    protectionDocumentWorkFlow =
                        NiisWorkflowAmbientContext.Current.ProtectionDocumentInventionsWorkflow;
                    break;

                case DicProtectionDocTypeCodes.ProtectionDocTypeSelectionAchieveCode:
                case DicProtectionDocTypeCodes.RequestTypeSelectionAchieveCode:
                    protectionDocumentWorkFlow = NiisWorkflowAmbientContext.Current
                        .ProtectionDocumentSelectiveAchievementsWorkflow;
                    break;
                    
                case DicProtectionDocTypeCodes.ProtectionDocTypeNameOfOriginCode:
                case DicProtectionDocTypeCodes.RequestTypeNameOfOriginCode:
                    protectionDocumentWorkFlow = NiisWorkflowAmbientContext.Current.ProtectionDocumentAppellationOfOriginWorkflow;
                    break;

                case DicProtectionDocTypeCodes.ProtectionDocTypeIndustrialSampleCode:
                case DicProtectionDocTypeCodes.RequestTypeIndustrialSampleCode:
                    protectionDocumentWorkFlow =
                        NiisWorkflowAmbientContext.Current.ProtectionDocumentIndustrialDesignsWorkflow;
                    break;

                case DicProtectionDocTypeCodes.ProtectionDocTypeUsefulModelCode:
                case DicProtectionDocTypeCodes.RequestTypeUsefulModelCode:
                    protectionDocumentWorkFlow =
                        NiisWorkflowAmbientContext.Current.ProtectionDocumentUsefulModelWorkflow;
                    break;
                    
                default:
                    throw new NotImplementedException();
            }

            if (protectionDocumentWorkFlow is null)
            {
                throw new NotImplementedException();
            }

            protectionDocumentWorkFlow.SetWorkflowRequest(protectionDocumentWorkFlowRequest);

            protectionDocumentWorkFlow.Process();
        }
    }
}
