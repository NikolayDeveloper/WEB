using System.Linq;
using Iserv.Niis.BusinessLogic.Documents;
using Iserv.Niis.DI;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowContracts;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using Iserv.Niis.WorkflowServices;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;
using System.Threading.Tasks;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowProtectionDocument;

namespace Iserv.Niis.BusinessLogic.Workflows.Documents
{
    public class ProcessWorkflowByDocumentIdHandler : BaseHandler
    {
        // Коды документов которые не должны двигать ОД по этапам , это рушит процесс имитации параллельности этапов
        private static readonly string[] _specialPatentDocCodes = new string[]
                    {
                          "OP_PAT"              // Полное описание патента ИЗ
                        , "OP_PAT_KZ"           // Полное описание патента ИЗ (kz)
                        , "OP_PAT_PM"           // Полное описание патента ПМ
                        , "OP_PAT_PM_KZ"        // Полное описание патента ПМ (kz)
                        , "OP_PAT_ID"           // Полное описание патента ПО
                        , "OP_PAT_ID_KZ"        // Полное описание патента ПО (kz)
                        , "OP_PAT_SA"           // Полное описание СД
                        , "OP_PAT_SA_KZ"        // Полное описание СД (kz)
                        , "PAT_AVT_CD_ZHIVOD"   // Удостоверение автора СД (животноводство)
                        , "PAT_AVT_CD_RASTENIE" // Удостоверение автора СД (растеневодство)
                        , "PAT_AVT_PM"          // Удостоверение автора ПМ
                        , "PAT_AVT_IZ"          // Удостоверение автора ИЗ
                        , "PAT_AVT_PO"          // Удостоверение автора ПО
                        , "UVO-5"               // Форма УВО-5
                        , "GR_TZ_SVID"          // Свидетельство на ТЗ
                    };
        public async Task ExecuteAsync(int documentId, int? realUserId = null)
        {
            return;

            //var document = await Executor.GetQuery<GetDocumentByIdQuery>().Process(r => r.ExecuteAsync(documentId));

            //if (document != null)
            //{
            //    foreach (var request in document.Requests)
            //    {
            //        NiisWorkflowAmbientContext.Current.RequestWorkflowService.Process(new RequestWorkFlowRequest
            //        {
            //            RequestId = request.RequestId,
            //            NextStageUserId = request.Request.CurrentWorkflow?.CurrentUserId ?? NiisAmbientContext.Current.User.Identity.UserId,
            //            DocumentId = documentId
            //        });
            //    }

            //    foreach (var contract in document.Contracts)
            //    {
            //        NiisWorkflowAmbientContext.Current.ContractWorkflowService.Process(new ContractWorkFlowRequest
            //        {
            //            ContractId = contract.ContractId,
            //            NextStageUserId = contract.Contract.CurrentWorkflow?.CurrentUserId ?? NiisAmbientContext.Current.User.Identity.UserId,
            //            DocumentId = documentId
            //        });
            //    }


            //    if (!_specialPatentDocCodes.Contains(document.Type.Code))
            //        foreach (var protectionDocDocument in document.ProtectionDocs)
            //        {
            //            NiisWorkflowAmbientContext.Current.ProtectionDocumentWorkflowService.Process(new ProtectionDocumentWorkFlowRequest
            //            {
            //                ProtectionDocId = protectionDocDocument.ProtectionDocId,
            //                NextStageUserId = realUserId ?? protectionDocDocument.ProtectionDoc.CurrentWorkflow?.CurrentUserId ?? NiisAmbientContext.Current.User.Identity.UserId,
            //                DocumentId = documentId
            //            }, realUserId);

            //            //if (new[]
            //            //    {
            //            //        DicProtectionDocTypeCodes.RequestTypeNameOfOriginCode,
            //            //        DicProtectionDocTypeCodes.RequestTypeTrademarkCode
            //            //    }.Contains(protectionDocDocument.ProtectionDoc.Type.Code)) && protectionDocDocument.ProtectionDoc.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.OD01_5_1)
            //            //{
            //            //    NiisWorkflowAmbientContext.Current.ProtectionDocumentWorkflowSerivce.Process(
            //            //        new ProtectionDocumentWorkFlowRequest
            //            //        {
            //            //            ProtectionDocId = protectionDocDocument.ProtectionDocId,
            //            //            NextStageUserId = NiisAmbientContext.Current.User.Identity.UserId,
            //            //            DocumentId = documentId
            //            //        });
            //            //}
            //        }
            //}
        }
    }
}
