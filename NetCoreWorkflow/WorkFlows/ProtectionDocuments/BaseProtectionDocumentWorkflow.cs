using System;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Workflow;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Exceptions;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicRouteStage;
using Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicTariffs;
using Iserv.Niis.WorkflowBusinessLogic.Document;
using Iserv.Niis.WorkflowBusinessLogic.PaymentInvoices;
using Iserv.Niis.WorkflowBusinessLogic.ProtectionDocuments;
using Iserv.Niis.WorkflowBusinessLogic.RulesProtectionDocument;
using Iserv.Niis.WorkflowBusinessLogic.Security;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowProtectionDocument;
using Iserv.Niis.WorkflowBusinessLogic.Workflows;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowTaskQueues;

namespace NetCoreWorkflow.WorkFlows.ProtectionDocuments
{
    public class BaseProtectionDocumentWorkflow : NetCoreBaseWorkflow<ProtectionDocumentWorkFlowRequest, ProtectionDoc>
    {
        public BaseProtectionDocumentWorkflow()
        {
            InitStages();
        }

        private void InitStages()
        {
            WorkflowStage("Ручной переход этапов", "CD782C2A-427D-42FC-A8F8-6C64A8D85CF9")
                .UseForAllStages()
                .And<IsProtectionDocumentExistsNextRouteStageByCodeRule>(r => r.Eval(WorkflowRequest?.CurrentWorkflowObject?.CurrentWorkflow?.CurrentStage?.Code, WorkflowRequest?.NextStageCode))
                .ThenSendToNextHandStage(SendProtectionDocumentToNextHandStage());

            WorkflowStage("Ручной возврат этапов", "9B3A496D-8085-4EBF-AD99-F982EAFE0385")
                .UseForAllStages()
                .And<IsProtectionDocumentExistsPreviousRouteStageByCodeRule>(r => r.Eval(WorkflowRequest?.CurrentWorkflowObject?.CurrentWorkflow?.CurrentStage?.Code, WorkflowRequest?.NextStageCode))
                .ThenSendToNextHandStage(SendProtectionDocumentToNextHandStage());
        }

        protected override string CurrentWorkflowStageCode => WorkflowRequest.CurrentWorkflowObject.CurrentWorkflow?.CurrentStage?.Code;

        protected Action<ProtectionDocumentWorkFlowRequest> SendProtectionDocumentToNextHandStage()
        {
            return (_workflowRequest) =>
            {
                SendProtectionDocumentToNextStage(_workflowRequest.NextStageCode).Invoke();
            };
        }

        protected Action SendProtectionDocumentToNextStage(string stageCode)
        {
            return () =>
            {

                List<DicRouteStage> nextStages = new List<DicRouteStage>();
                var protectionDoc = Executor.GetQuery<GetProtectionDocByIdQuery>().Process(r => r.Execute(WorkflowRequest.CurrentWorkflowObject.Id));
                List<ProtectionDocWorkflow> nextWorkFlows = new List<ProtectionDocWorkflow>();
                if (stageCode == RouteStageCodes.ODParallel)
                {
                    WorkflowRequest.SpecificNextStageUserIds[RouteStageCodes.ODParallel] = WorkflowRequest
                    .CurrentWorkflowObject
                    .CurrentWorkflow
                    .CurrentUserId
                    .HasValue ? WorkflowRequest.CurrentWorkflowObject.CurrentWorkflow.CurrentUserId.Value 
                    : WorkflowRequest.SpecificNextStageUserIds.Last().Value;
                    List<string> stages;
                    if (WorkflowRequest.SpecificNextStageUserIds.Any(x => x.Key == RouteStageCodes.OD01_6))
                        stages = new List<string> { RouteStageCodes.OD01_3, RouteStageCodes.OD01_2_2, RouteStageCodes.OD01_6 }; 
                    else
                        stages = new List<string> { RouteStageCodes.OD01_3, RouteStageCodes.OD01_2_2, RouteStageCodes.OD03_1 };


                    foreach (var stage_code in stages)
                    {
                        if (!protectionDoc.Workflows.Any(x => x.CurrentStage.Code == stage_code || (x.FromStage != null && x.FromStage.Code == stage_code)))
                        {
                            nextStages.Add(Executor.GetQuery<GetDicRouteStageByCodeQuery>().Process(q => q.Execute(stage_code)));
                            nextWorkFlows.Add(CreateProtectionDocWorkFlow(stage_code));
                        }
                    };
                    nextWorkFlows.Add(CreateProtectionDocWorkFlow(RouteStageCodes.ODParallel));
                }
                else
                {
                    nextStages.Add( Executor.GetQuery<GetDicRouteStageByCodeQuery>().Process(q => q.Execute(stageCode)) );
                    nextWorkFlows.Add(CreateProtectionDocWorkFlow(stageCode));
                }

                nextWorkFlows.ForEach(x => Executor.GetCommand<CreateProtectionDocWorkflowCommand>().Process(r => r.Execute(x)));

                for (int i = 0; i < nextWorkFlows.Count - 1; i++)
                    Executor.GetCommand<CreateProtectionDocParallelWorkflowCommand>().Process(r => r.Execute(nextWorkFlows[i]));

                protectionDoc.CurrentWorkflow = nextWorkFlows.Last();
                protectionDoc.CurrentWorkflowId = nextWorkFlows.Last().Id;
                protectionDoc.StatusId = nextStages.Last()?.ProtectionDocStatusId ?? protectionDoc.StatusId;
                
                Executor.GetCommand<UpdateProtectionDocCommand>().Process(r => r.Execute(protectionDoc));
                protectionDoc = Executor.GetQuery<GetProtectionDocByIdQuery>().Process(r => r.Execute(protectionDoc.Id));
                
                Executor.GetCommand<UpdateMarkAsExecutedWorkflowTaskEvenstByRequestIdCommand>().Process(r => r.Execute(protectionDoc.Id));

                #region Запланируем автоэтап.

                var nextStageOrders = Executor.GetQuery<GetNextDicRouteStageOrdersByCodeQuery>().Process(r => r.Execute(protectionDoc.CurrentWorkflow.CurrentStage.Code));

                var automaticStages = nextStageOrders.Where(r => r.IsAutomatic == true);

                foreach (var autoStageOrder in automaticStages)
                {
                    var workflowTaskQueueResolveDate = Executor.GetHandler<CalculateWorkflowTaskQueueResolveDateHandler>().Process(r => r.Execute(protectionDoc.Id, Owner.Type.ProtectionDoc, autoStageOrder.NextStage.Code));

                    var workflowTaskQueue = new WorkflowTaskQueue
                    {
                        ProtectionDocId = protectionDoc.Id,
                        ResolveDate = workflowTaskQueueResolveDate,
                        ResultStageId = autoStageOrder.NextStageId,
                        ConditionStageId = autoStageOrder.CurrentStage.Id,
                    };

                    Executor.GetCommand<CreateWorkflowTaskQueueCommand>().Process(r => r.Execute(workflowTaskQueue));
                }

                #endregion

                #region Создание патента/сертификата

                if (stageCode == RouteStageCodes.OD04_5)
                {
                    CreatePatentOrCertificate(WorkflowRequest.CurrentWorkflowObject.Id);
                }

                #endregion

                #region Внесение ОД в реестр на отправку в МЮ

                if (stageCode == RouteStageCodes.OD01_3)
                {
                    CreateProtectionDocRegister(WorkflowRequest.CurrentWorkflowObject.Id);
                }

                #endregion

                #region Создание удостоверений авторов

                //if (stageCode == RouteStageCodes.OD01_2_1)
                //{
                //    CreateAuthorsCertificate(WorkflowRequest.CurrentWorkflowObject.Id);
                //}

                #endregion

                #region Создание тарифов на поддержку ОД

                //if (stageCode == RouteStageCodes.OD01_5)
                //{
                //    CreateProtectionDocSupportTariffs(WorkflowRequest.CurrentWorkflowObject.Id);
                //}

                #endregion
            };
        }

        private ProtectionDocWorkflow CreateProtectionDocWorkFlow(string nextStageCode)
        {
            var nextStage = Executor.GetQuery<GetDicRouteStageByCodeQuery>().Process(r => r.Execute(nextStageCode));
            
            return new ProtectionDocWorkflow
            {
                CurrentUserId = WorkflowRequest.SpecificNextStageUserIds.Any(x => x.Key == nextStageCode) ? WorkflowRequest.SpecificNextStageUserIds[nextStageCode] : WorkflowRequest.NextStageUserId,
                SecondaryCurrentUserId = WorkflowRequest.NextStageSecondaryUserId,
                OwnerId = WorkflowRequest.CurrentWorkflowObject.Id,
                CurrentStageId = nextStage.Id,
                FromStageId = WorkflowRequest.CurrentWorkflowObject.CurrentWorkflow.CurrentStageId,
                FromUserId = WorkflowRequest.CurrentWorkflowObject.CurrentWorkflow.CurrentUserId,
                RouteId = nextStage.RouteId,
                IsComplete = nextStage.IsLast,
                IsSystem = nextStage.IsSystem,
                IsMain = nextStage.IsMain
            };
        }

        private void CreatePatentOrCertificate(int protectionDocid)
        {
            var protectionDoc = Executor.GetQuery<GetProtectionDocByIdQuery>().Process(q => q.Execute(protectionDocid));
            if (protectionDoc == null)
                throw new DataNotFoundException(nameof(ProtectionDoc), DataNotFoundException.OperationType.Read, 0);
            string patentCode = null;
            switch (protectionDoc?.Type?.Code)
            {
                case DicProtectionDocTypeCodes.ProtectionDocTypeTrademarkCode:
                    patentCode = DicDocumentTypeCodes.TrademarkCertificate;
                    break;
                case DicProtectionDocTypeCodes.ProtectionDocTypeNameOfOriginCode:
                    patentCode = DicDocumentTypeCodes.NmptCertificate;
                    break;
                case DicProtectionDocTypeCodes.ProtectionDocTypeInventionCode:
                    patentCode = DicDocumentTypeCodes.InventionPatent;
                    break;
                case DicProtectionDocTypeCodes.ProtectionDocTypeSelectionAchieveCode:
                    switch (protectionDoc?.SelectionAchieveType?.Code)
                    {
                        case DicSelectionAchieveTypeCodes.AnimalHusbandry:
                            patentCode = DicDocumentTypeCodes.SelectiveAchievementsAnimalHusbandryPatent;
                            break;
                        case DicSelectionAchieveTypeCodes.Agricultural:
                        case DicSelectionAchieveTypeCodes.VarietiesPlant:
                            patentCode = DicDocumentTypeCodes.SelectiveAchievementsAgriculturalPatent;
                            break;
                    }
                    break;
                case DicProtectionDocTypeCodes.ProtectionDocTypeUsefulModelCode:
                    patentCode = DicDocumentTypeCodes.UsefulModelPatent;
                    break;
                case DicProtectionDocTypeCodes.ProtectionDocTypeIndustrialSampleCode:
                    patentCode = DicDocumentTypeCodes.IndustrialDesignsPatent;
                    break;
            }
            var userInputDto = new UserInputDto
            {
                Code = patentCode,
                Fields = new List<KeyValuePair<string, string>>(),
                OwnerId = protectionDocid,
                OwnerType = Owner.Type.ProtectionDoc
            };
            Executor.GetHandler<CreateDocumentHandler>().Process<Document>(h =>
                h.Execute(protectionDocid, Owner.Type.ProtectionDoc, patentCode, DocumentType.Outgoing, userInputDto));
        }

        private void CreateAuthorsCertificate(int protectionDocId)
        {
            var protectionDoc = Executor.GetQuery<GetProtectionDocByIdQuery>().Process(q => q.Execute(protectionDocId));
            string authorCertificateCode = null;
            switch (protectionDoc.Type?.Code)
            {
                case DicProtectionDocTypeCodes.ProtectionDocTypeInventionCode:
                    authorCertificateCode = DicDocumentTypeCodes.InventionAuthorCertificate;
                    break;
                case DicProtectionDocTypeCodes.ProtectionDocTypeSelectionAchieveCode:
                    switch (protectionDoc.SelectionAchieveType?.Code)
                    {
                        case DicSelectionAchieveTypeCodes.AnimalHusbandry:
                            authorCertificateCode = DicDocumentTypeCodes.AnimalHusbandrySelectiveAchievementAuthorCertificate;
                            break;
                        case DicSelectionAchieveTypeCodes.Agricultural:
                        case DicSelectionAchieveTypeCodes.VarietiesPlant:
                            authorCertificateCode = DicDocumentTypeCodes.AgriculturalSelectiveAchievementAuthorCertificate;
                            break;
                    }
                    break;
                case DicProtectionDocTypeCodes.ProtectionDocTypeUsefulModelCode:
                    authorCertificateCode = DicDocumentTypeCodes.UsefulModelAuthorCertificate;
                    break;
                case DicProtectionDocTypeCodes.ProtectionDocTypeIndustrialSampleCode:
                    authorCertificateCode = DicDocumentTypeCodes.IndustrialDesignAuthorCertificate;
                    break;
            }
            if (authorCertificateCode != null)
            {
                var authors =
                    protectionDoc.ProtectionDocCustomers.Where(pc =>
                        pc.CustomerRole.Code == DicCustomerRoleCodes.Author);
                for (int i = 0; i < authors.Count(); i++)
                {
                    var userInputDto = new UserInputDto
                    {
                        Code = authorCertificateCode,
                        Fields = new List<KeyValuePair<string, string>>(),
                        OwnerId = protectionDocId,
                        OwnerType = Owner.Type.ProtectionDoc,
                        Index = i +1
                    };
                    Executor.GetHandler<CreateDocumentHandler>().Process<Document>(h =>
                        h.Execute(protectionDocId, Owner.Type.ProtectionDoc, authorCertificateCode, DocumentType.Internal, userInputDto));
                }
            }
        }

        private void CreateProtectionDocRegister(int protectionDocId)
        {
            var protectionDoc = Executor.GetQuery<GetProtectionDocByIdQuery>().Process(q => q.Execute(protectionDocId));
            var bulletinId = protectionDoc.Bulletins.FirstOrDefault(b => b.IsPublish)?.Bulletin?.Id ?? 0;
            var typeId = protectionDoc.TypeId;
            /// удаленно (старый док)
            var register = Executor.GetQuery<GetProtectionDocRegisterByBulletinIdAndProtectionDocTypeIdQuery>()
                .Process(q => q.Execute(bulletinId, typeId));
            //if (register == null)
            //{
            //    var registerCode = DicDocumentTypeCodes.Reestr_006_014_3;
            //    var userInputDto = new UserInputDto
            //    {
            //        Code = registerCode,
            //        Fields = new List<KeyValuePair<string, string>>(),
            //        OwnerId = protectionDocId,
            //        OwnerType = Owner.Type.ProtectionDoc
            //    };
            //    var documentId = Executor.GetHandler<CreateDocumentHandler>().Process(h =>
            //        h.Execute(protectionDocId, Owner.Type.ProtectionDoc, registerCode, DocumentType.Outgoing, userInputDto, bulletinId, typeId));
            //    Executor.GetHandler<GenerateProtectionDocRegisterNumberHandler>().Process<object>(h => h.Execute(documentId));
            //}
            //else
            //{
            var protectionDocDocument = new ProtectionDocDocument
                {
                    DocumentId = register.Id,
                    ProtectionDocId = protectionDocId
                };
                Executor.GetCommand<CreateProtectionDocDocumentCommand>()
                    .Process(c => c.Execute(protectionDocDocument));
            //}
        }

        private void CreateProtectionDocSupportTariffs(int protectionDocId)
        {
            var protectionDoc = Executor.GetQuery<GetProtectionDocByIdQuery>().Process(q => q.Execute(protectionDocId));
            var tariffs = Executor.GetQuery<GetProtectionDocSupportTariffsBySupportYearAndExpiryQuery>().Process(q =>
                q.Execute((DateTimeOffset.Now.Year - protectionDoc.DateCreate.Year) + 1, false, protectionDoc.TypeId));
            var systemUser = Executor.GetQuery<GetUserByXinQuery>()
                .Process(q => q.Execute(UserConstants.SystemUserXin));
            foreach (var tariff in tariffs)
            {
                Executor.GetHandler<CreatePaymentInvoiceHandler>().Process(h =>
                    h.Execute(Owner.Type.ProtectionDoc, protectionDocId, tariff.Code, DicPaymentStatusCodes.Notpaid, systemUser?.Id));
            }
        }
    }
}