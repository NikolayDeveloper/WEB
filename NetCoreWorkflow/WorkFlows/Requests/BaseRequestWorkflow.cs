using AutoMapper;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Entities.Workflow;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.WorkflowBusinessLogic.Bibliographics.ColorTzs.ProtectionDoc;
using Iserv.Niis.WorkflowBusinessLogic.Bibliographics.ColorTzs.Request;
using Iserv.Niis.WorkflowBusinessLogic.Bibliographics.ConventionInfos.ProtectionDoc;
using Iserv.Niis.WorkflowBusinessLogic.Bibliographics.ConventionInfos.Request;
using Iserv.Niis.WorkflowBusinessLogic.Bibliographics.EarlyRegs.ProtectionDoc;
using Iserv.Niis.WorkflowBusinessLogic.Bibliographics.EarlyRegs.Request;
using Iserv.Niis.WorkflowBusinessLogic.Bibliographics.Icfem.ProtectionDoc;
using Iserv.Niis.WorkflowBusinessLogic.Bibliographics.Icfem.Request;
using Iserv.Niis.WorkflowBusinessLogic.Bibliographics.Icgs.ProtectionDoc;
using Iserv.Niis.WorkflowBusinessLogic.Bibliographics.Icgs.Request;
using Iserv.Niis.WorkflowBusinessLogic.Bibliographics.Icis.ProtectionDoc;
using Iserv.Niis.WorkflowBusinessLogic.Bibliographics.Icis.Request;
using Iserv.Niis.WorkflowBusinessLogic.Bibliographics.Ipc.ProtectionDoc;
using Iserv.Niis.WorkflowBusinessLogic.Bibliographics.Ipc.Request;
using Iserv.Niis.WorkflowBusinessLogic.Common;
using Iserv.Niis.WorkflowBusinessLogic.Customers;
using Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicProtectionDocStatuses;
using Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicRouteStagePerformers;
using Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicProtectionDocSubTypes;
using Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicProtectionDocTypes;
using Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicRequestStatuses;
using Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicRouteStage;
using Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicSendType;
using Iserv.Niis.WorkflowBusinessLogic.Document;
using Iserv.Niis.WorkflowBusinessLogic.Payment;
using Iserv.Niis.WorkflowBusinessLogic.PaymentInvoices;
using Iserv.Niis.WorkflowBusinessLogic.ProtectionDocuments;
using Iserv.Niis.WorkflowBusinessLogic.Requests;
using Iserv.Niis.WorkflowBusinessLogic.Security;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using Iserv.Niis.WorkflowBusinessLogic.Workflows;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowTaskQueues;
using System;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.WorkflowBusinessLogic.RulesRequest;
using CreateRequestWorkflowCommand = Iserv.Niis.WorkflowBusinessLogic.Workflows.CreateRequestWorkflowCommand;

namespace NetCoreWorkflow.WorkFlows.Requests
{
    public class BaseRequestWorkflow : NetCoreBaseWorkflow<RequestWorkFlowRequest, Request>
    {
        private readonly IIntegrationStatusUpdater _integrationStatusUpdater;
        private readonly IIntegrationDocumentUpdater _integrationDocumentUpdater;
        private readonly IMapper _mapper;

        private readonly string[] _changeStages = 
        {
            RouteStageCodes.TZ_06
        };

        private readonly string[] _registryCodes =
        {
            RouteStageCodes.TZ_03_3_8,
            RouteStageCodes.UM_03_8,
            RouteStageCodes.I_03_3_8
        };

        private readonly string[] _sendRequestCodes =
        {
            RouteStageCodes.TZ_03_3_7_1,
            RouteStageCodes.I_03_3_1_1_1,
            RouteStageCodes.UM_03_2_1,
            RouteStageCodes.NMPT_03_2_1,
            RouteStageCodes.SA_03_3_1,
            RouteStageCodes.PO_03_2_1,
            RouteStageCodes.ITZ_03_3_2_0
        };

        public BaseRequestWorkflow(
            IIntegrationStatusUpdater integrationStatusUpdater,
            IIntegrationDocumentUpdater integrationDocumentUpdater,
            IMapper mapper)
        {
            _integrationStatusUpdater = integrationStatusUpdater;
            _integrationDocumentUpdater = integrationDocumentUpdater;
            _mapper = mapper;
            InitStages();
        }

        private void InitStages()
        {
            WorkflowStage("Ручной переход этапов", "67EE3330-C6EC-4FDB-907B-945C6B316472")
                .UseForAllStages()
                .And<IsRequestExistsNextRouteStageByCodeRule>(r =>
                    r.Eval(WorkflowRequest?.CurrentWorkflowObject?.CurrentWorkflow?.CurrentStage?.Code,
                        WorkflowRequest?.NextStageCode))
                .ThenSendToNextHandStage(SendRequestToNextHandStage());

            WorkflowStage("Ручной возврат этапов", "9E00D2D3-28FF-426D-8577-BC452C18FE52")
                .UseForAllStages()
                .And<IsRequestExistsPreviousRouteStageByCodeRule>(r =>
                    r.Eval(WorkflowRequest?.CurrentWorkflowObject?.CurrentWorkflow?.CurrentStage?.Code,
                        WorkflowRequest?.NextStageCode))
                .Then(ReturnRequestToPreviousStage());

            WorkflowStage("Переход по текущему этапу вручную", "20CD808D-3568-4A14-870F-6F103784EDDD")
                .UseForAllStages()
                .And<IsRequestBeingSentToTheSameStageRule>(r => r.Eval())
                .Then(SendRequestToSameStage());
        }

        protected override string CurrentWorkflowStageCode => WorkflowRequest.CurrentWorkflowObject.CurrentWorkflow?.CurrentStage?.Code;

        protected Action<RequestWorkFlowRequest> SendRequestToNextHandStage()
        {
            return (_workflowRequest) =>
            {
                var request = Executor.GetQuery<GetRequestByIdQuery>()
                    .Process(q => q.Execute(WorkflowRequest.RequestId));

                if (_changeStages.Contains(_workflowRequest.NextStageCode))
                {
                    SendRequestOnChangeScenarioStage(_workflowRequest.NextStageCode)?.Invoke();
                }
                else if (_changeStages.Contains((request.CurrentWorkflow.CurrentStage.Code)))
                {
                    ReturnRequestFromChangeScenario()?.Invoke();
                }
                else
                {
                    SendRequestToNextStage(_workflowRequest.NextStageCode)?.Invoke();
                }
            };
        }

        protected Action ReturnRequestToPreviousStage()
        {
            return () =>
            {
                var request = Executor.GetQuery<GetRequestByIdQuery>()
                    .Process(r => r.Execute(WorkflowRequest.CurrentWorkflowObject.Id));
                var previousStageCode = request.CurrentWorkflow.FromStage.Code;
                SendRequestToNextStage(previousStageCode, request.CurrentWorkflow.FromUserId).Invoke();
            };
        }

        protected Action SendToNextStageAndSetFormalExamNotPaidFlag(string stageCode)
        {
            return () =>
            {
                SendRequestToNextStage(stageCode)?.Invoke();
                var request = Executor.GetQuery<GetRequestByIdQuery>()
                    .Process(q => q.Execute(WorkflowRequest.RequestId));
                request.IsFormalExamFeeNotPaidInTime = true;
                Executor.GetCommand<UpdateRequestCommand>().Process(c => c.Execute(request));
            };
        }

        protected Action SendToNextStageAndCreatePaymentInvoice(string stageCode, string tariffCode, string paymentStatusCode)
        {
            return () =>
            {
                SendRequestToNextStage(stageCode)?.Invoke();
                var systemUser = Executor.GetQuery<GetUserByXinQuery>()
                    .Process(q => q.Execute(UserConstants.SystemUserXin));
                Executor.GetHandler<CreatePaymentInvoiceHandler>().Process(h => h.Execute(Owner.Type.Request, WorkflowRequest.RequestId, tariffCode, paymentStatusCode, systemUser?.Id));
            };
        }

        protected Action SendToNextStageAndCreatePaymentInvoices(string stageCode, string[] tariffCodes, string paymentStatusCode)
        {
            return () =>
            {
                if (stageCode == RouteStageCodes.I_02_2)
                {
                    tariffCodes = GetTariffCodesForStageInputPayment();
                }

                SendRequestToNextStage(stageCode)?.Invoke();
                var systemUser = Executor.GetQuery<GetUserByXinQuery>()
                    .Process(q => q.Execute(UserConstants.SystemUserXin));
                Executor.GetHandler<CreatePaymentInvoicesHandler>().Process(h => h.Execute(Owner.Type.Request, WorkflowRequest.RequestId, tariffCodes, paymentStatusCode, systemUser?.Id));
            };
        }

        protected Action SendRequestToNextStage(string stageCode, int? nextUserId = null)
        {
            return () =>
            {
                //var fromStageId = WorkflowRequest.CurrentWorkflowObject.CurrentWorkflow.CurrentStageId.Value;

                var nextWorkFlow = CreateRequestWorkFlow(stageCode, nextUserId);

                var currentStage = nextWorkFlow.CurrentStage;
                nextWorkFlow.CurrentStage = null;

                Executor.GetCommand<CreateRequestWorkflowCommand>().Process(r => r.Execute(nextWorkFlow));

                _integrationStatusUpdater.Add(nextWorkFlow.Id);
                _integrationDocumentUpdater.Add(nextWorkFlow);

                var request = Executor.GetQuery<GetRequestByIdQuery>().Process(r => r.Execute(WorkflowRequest.CurrentWorkflowObject.Id));
                request.CurrentWorkflow = nextWorkFlow;
                request.CurrentWorkflowId = nextWorkFlow.Id;
                request.IsComplete = nextWorkFlow.IsComplete ?? false;

                var expertise = new[]
                {
                    RouteStageCodes.TZ_03_2_2,
                    RouteStageCodes.SA_032
                };

                if (expertise.Contains(currentStage?.Code))
                {
                    request.PublicDate = DateTimeOffset.Now;
                }

                var nextStage = Executor.GetQuery<GetDicRouteStageByCodeQuery>().Process(q => q.Execute(stageCode));

                request.StatusId = nextStage.RequestStatusId ?? request.StatusId;

                Executor.GetCommand<UpdateRequestCommand>().Process(r => r.Execute(request));
                request = Executor.GetQuery<GetRequestByIdQuery>().Process(r => r.Execute(request.Id));

                Executor.GetCommand<UpdateMarkAsExecutedWorkflowTaskEvenstByRequestIdCommand>().Process(r => r.Execute(request.Id));

                #region генерируем дату подачи заявки

                var formationStageCodes = new[]
                {
                    RouteStageCodes.NMPT_02_1,
                    RouteStageCodes.UM_02_1
                };

                if (formationStageCodes.Contains(stageCode))
                {
                    request.RequestDate = request.DateCreate;
                    Executor.GetCommand<UpdateRequestCommand>().Process(r => r.Execute(request));
                }

                #endregion

                #region Запланируем автоэтап.

                CreateWorkflowAutomaticStageTask();

                #endregion

                var conversionStages = new[]
                {
                    RouteStageCodes.UM_03_2_4,
                    RouteStageCodes.I_03_3_1__1
                };

                if (conversionStages.Contains(stageCode))
                {
                    ConvertRequest(WorkflowRequest.RequestId);
                }

                //Executor.GetHandler<GenerateAutoNotificationHandler>()
                //    .Process(h => h.Execute(WorkflowRequest.CurrentWorkflowObject.Id));

                //Executor.GetHandler<GenerateAutoPaymentHandler>()
                //    .Process(h => h.Execute(WorkflowRequest.CurrentWorkflowObject.Id));

                //Executor.GetHandler<AutoChargeRequestPaymentInvoicesHandler>()
                //    .Process(handler => handler.Execute(WorkflowRequest.CurrentWorkflowObject.Id, fromStageId, nextStage.Id));

                var abortStages = new[]
                {
                    RouteStageCodes.TZ_03_3_9
                };
                if (abortStages.Contains(stageCode))
                {
                    string newStatusCode;
                    var termStageCodes = new[]
                    {
                        //RouteStageCodes.TZTermProlongationFull,
                        //RouteStageCodes.TZTermProlongationChange,
                        //RouteStageCodes.TZTermProlongationConvert,
                        //RouteStageCodes.TZTermProlongationFormal,
                        //RouteStageCodes.TZTermProlongationObjection,
                        //RouteStageCodes.TZTermProlongationSplit,
                        RouteStageCodes.TZ_03_3_7_3,
                        RouteStageCodes.TZ_03_3_7_4
                    };
                    var examAwaitPaymentStageCodes = new[]
                    {
                        RouteStageCodes.TZ_03_2_2_0,
                        RouteStageCodes.TZAwaitingRegistrationTermRestoration
                    };
                    if (termStageCodes.Contains(request.CurrentWorkflow.FromStage.Code))
                    {
                        newStatusCode = DicRequestStatusCodes.RecallByNotAnswered;
                    }
                    else if (examAwaitPaymentStageCodes.Contains(request.CurrentWorkflow.FromStage.Code))
                    {
                        newStatusCode = DicRequestStatusCodes.RecallByNotPaid;
                    }
                    else
                    {
                        newStatusCode = DicRequestStatusCodes.RecalByRequest;
                    }
                    var newStatus = Executor.GetQuery<GetDicRequestStatusByCodeQuery>()
                        .Process(q => q.Execute(newStatusCode));
                    request.StatusId = newStatus.Id;
                    Executor.GetCommand<UpdateRequestCommand>().Process(q => q.Execute(request));
                }

                var generateProtectionDocumentStage = new[]
                {
                    RouteStageCodes.PO_04,
                    RouteStageCodes.SA_04,
                    RouteStageCodes.TZ_04,
                    RouteStageCodes.TZRegistration,
                    RouteStageCodes.UM_04,
                    RouteStageCodes.NMPT_04_1,
                    RouteStageCodes.I_04_1
                };

                //var вecisionSentToApplicant = new[]
                //{
                //    RouteStageCodes.TZ_03_3_7,
                //    RouteStageCodes.PO_03_8,
                //    RouteStageCodes.NMPT_03_7,
                //    RouteStageCodes.I_03_3_7_0,
                //    RouteStageCodes.UM_03_7_0,
                //    RouteStageCodes.SA_03_3_3_8,
                //    RouteStageCodes.I_03_3_2_3,
                //};

                //if (вecisionSentToApplicant.Contains(WorkflowRequest.NextStageCode))
                //{
                //    SendRequestToLk(request);
                //}

                if (generateProtectionDocumentStage.Contains(WorkflowRequest.NextStageCode))
                {
                    //Создание Охранного документа(ОД)
                    GenerateProtectionDocument(WorkflowRequest);
                }

                if (_registryCodes.Contains(WorkflowRequest.NextStageCode))
                {
                    ProcessStateRegistryWorkflow();
                }
            };
        }

        private void SendRequestToLk(Request request)
        {
            
        }

        protected Action SendRequestToSameStage()
        {
            return () =>
            {
                SendRequestToNextStage(WorkflowRequest?.PrevStageCode)?.Invoke();
            };
        }

        protected Action SendRequestToNextStageWithExecutorFromPetition(string stageCode, string[] petitionCodes)
        {
            return () =>
            {
                var petitions = Executor.GetQuery<GetDocumentsByRequestIdAndTypeCodesQuery>()
                    .Process(q => q.Execute(WorkflowRequest.RequestId, petitionCodes));
                var petitionInWork = petitions.FirstOrDefault(p =>
                    p.CurrentWorkflows.Any(cw => cw.CurrentStage.Code == RouteStageCodes.DocumentIncoming_2_2));
                if (petitionInWork != null)
                {
                    var executorId = petitionInWork.CurrentWorkflows
                        .FirstOrDefault(c => c.CurrentStage.Code == RouteStageCodes.DocumentIncoming_2_2)
                        ?.CurrentUserId;
                    SendRequestToNextStage(stageCode, executorId).Invoke();
                }
            };
        }

        protected Action SendRequestOnChangeScenarioStage(string stageCode)
        {
            return () =>
            {
                var request = Executor.GetQuery<GetRequestByIdQuery>().Process(r => r.Execute(WorkflowRequest.CurrentWorkflowObject.Id));
                var requestCurrentWorkflow = request.CurrentWorkflow;

                requestCurrentWorkflow.IsChangeScenarioEntry = true;
                Executor.GetCommand<UpdateRequestWorkflowCommand>().Process(c => c.Execute(requestCurrentWorkflow));

                SendRequestToNextStage(stageCode).Invoke();
            };
        }

        protected Action ReturnRequestFromChangeScenario()
        {
            return () =>
            {
                var workflows = Executor.GetQuery<GetRequestWorkflowsByRequestIdQuery>()
                    .Process(q => q.Execute(WorkflowRequest.CurrentWorkflowObject.Id));
                var lastStageBeforeChangeScenario =
                    workflows.OrderBy(w => w.DateCreate).Last(w => w.IsChangeScenarioEntry == true);

                SendRequestToNextStage(lastStageBeforeChangeScenario?.CurrentStage?.Code).Invoke();
            };
        }

        protected Action ReturnFromSendRequestScenario()
        {

            return () =>
            {
                var workflows = Executor.GetQuery<GetRequestWorkflowsByRequestIdQuery>()
                    .Process(q => q.Execute(WorkflowRequest.CurrentWorkflowObject.Id));
                var lastStageSendRequestScenario =
                    workflows.OrderBy(w => w.DateCreate).Last(w =>_sendRequestCodes.Contains(w.CurrentStage.Code));

                SendRequestToNextStage(lastStageSendRequestScenario?.FromStage?.Code, lastStageSendRequestScenario?.FromUser?.Id).Invoke();
            };
        }

        private void CreateWorkflowAutomaticStageTask()
        {
            var request = Executor.GetQuery<GetRequestByIdQuery>().Process(r => r.Execute(WorkflowRequest.CurrentWorkflowObject.Id));

            var nextStageOrders = Executor.GetQuery<GetNextDicRouteStageOrdersByCodeQuery>()
                .Process(r => r.Execute(request.CurrentWorkflow.CurrentStage.Code));

            var automaticStages = nextStageOrders.Where(r => r.IsAutomatic == true);

            foreach (var autoStageOrder in automaticStages)
            {
                var workflowTaskQueueResolveDate = Executor.GetHandler<CalculateWorkflowTaskQueueResolveDateHandler>()
                    .Process(r => r.Execute(request.Id, Owner.Type.Request, autoStageOrder.NextStage.Code));

                var workflowTaskQueue = new WorkflowTaskQueue
                {
                    RequestId = request.Id,
                    ResolveDate = workflowTaskQueueResolveDate,
                    ResultStageId = autoStageOrder.NextStageId,
                    ConditionStageId = autoStageOrder.CurrentStage.Id,
                };

                Executor.GetCommand<CreateWorkflowTaskQueueCommand>()
                    .Process(r => r.Execute(workflowTaskQueue));
            }
        }

        private void ProcessStateRegistryWorkflow()
        {
            string[] stateServiceRequestCodes = { DicDocumentTypeCodes.StateServiceRequest, DicDocumentTypeCodes.StateServicesRequest };

            List<Document> documents = Executor
                .GetQuery<GetDocumentsByRequestIdAndTypeCodesQuery>()
                .Process(query => query.Execute(WorkflowRequest.RequestId, stateServiceRequestCodes));

            Executor
                .GetHandler<MarkDocumentsAsCompletedHandler>()
                .Process<object>(handler => handler.Execute(documents));
        }

        /// <summary>
        /// Создание данных рабочего процесса для следующего этапа.
        /// </summary>
        /// <param name="nextStageCode">Код следующего этапа маршрута.</param>
        /// <param name="nextStageUserId">Идентификатор пользователя следующего этапа маршрута.</param>
        /// <returns></returns>
        private RequestWorkflow CreateRequestWorkFlow(string nextStageCode, int? nextStageUserId = null)
        {
            var nextStage = Executor.GetQuery<GetDicRouteStageByCodeQuery>().Process(r => r.Execute(nextStageCode));
            int? autoPerformerId = Executor.GetQuery<GetPerformerIdByRouteStageIdQuery>().Process(p => p.Execute(nextStage?.Id ?? 0));
            var autoUser = Executor.GetHandler<GetRequestAutoRouteStageExecutorHandler>().Process(h => h.Execute(nextStage?.Id ?? 0));

            if (autoPerformerId == 0) autoPerformerId = null;

            if (WorkflowRequest.CurrentWorkflowObject.CurrentWorkflow.CurrentStage.Code == nextStageCode)
            {
                autoPerformerId = null;
                autoUser = null;
            }

            return new RequestWorkflow
            {
                CurrentUserId = autoPerformerId ?? autoUser?.Id ?? nextStageUserId ?? WorkflowRequest.NextStageUserId,
                OwnerId = WorkflowRequest.CurrentWorkflowObject.Id,
                CurrentStageId = nextStage.Id,
                CurrentStage = nextStage,
                FromStageId = WorkflowRequest.CurrentWorkflowObject.CurrentWorkflow.CurrentStageId,
                FromUserId = WorkflowRequest.CurrentWorkflowObject.CurrentWorkflow.CurrentUserId,
                RouteId = nextStage.RouteId,
                IsComplete = nextStage.IsLast,
                IsSystem = nextStage.IsSystem,
                IsMain = nextStage.IsMain
            };
        }

        /// <summary>
        /// Создание Охранного документа(ОД)
        /// </summary>
        private void GenerateProtectionDocument(RequestWorkFlowRequest workflowRequest)
        {
            var request = Executor.GetQuery<GetRequestByIdQuery>().Process(r => r.Execute(workflowRequest.RequestId));
            var dicProtectionDocStatus = Executor.GetQuery<GetDicProtectionDocStatusByCodeQuery>().Process(r => r.Execute(DicProtectionDocStatusCodes.D));
            var defaultSendType = Executor.GetQuery<GetSendTypeByCodeQuery>()
                .Process(q => q.Execute(DicSendTypeCodes.ByHand));

            if (dicProtectionDocStatus == null) return;

            //int addYears = 0;
            string protectionDocTypeCode = "";

            switch (request?.ProtectionDocType?.Code)
            {
                case DicProtectionDocTypeCodes.RequestTypeTrademarkCode:
                    //addYears = 10;
                    protectionDocTypeCode = DicProtectionDocTypeCodes.ProtectionDocTypeTrademarkCode;
                    break;
                case DicProtectionDocTypeCodes.RequestTypeNameOfOriginCode:
                    protectionDocTypeCode = DicProtectionDocTypeCodes.ProtectionDocTypeNameOfOriginCode;
                    //addYears = 10;
                    break;
                case DicProtectionDocTypeCodes.RequestTypeSelectionAchieveCode:
                    protectionDocTypeCode = DicProtectionDocTypeCodes.ProtectionDocTypeSelectionAchieveCode;
                    switch (request.SelectionAchieveType.Code)
                    {
                        case DicSelectionAchieveTypeCodes.Agricultural:
                            //addYears = 25;
                            break;
                        case DicSelectionAchieveTypeCodes.AnimalHusbandry:
                            //addYears = 30;
                            break;
                        case DicSelectionAchieveTypeCodes.VarietiesPlant:
                            //addYears = 35;
                            break;
                    }
                    break;
                case DicProtectionDocTypeCodes.RequestTypeUsefulModelCode:
                    protectionDocTypeCode = DicProtectionDocTypeCodes.ProtectionDocTypeUsefulModelCode;
                    //addYears = 5;
                    break;
                case DicProtectionDocTypeCodes.RequestTypeInventionCode:
                    protectionDocTypeCode = DicProtectionDocTypeCodes.ProtectionDocTypeInventionCode;
                    //addYears = 20;
                    break;
                case DicProtectionDocTypeCodes.RequestTypeIndustrialSampleCode:
                    protectionDocTypeCode = DicProtectionDocTypeCodes.ProtectionDocTypeIndustrialSampleCode;
                    //addYears = 15;
                    break;
            }
            //var validDate = request?.RequestDate?.AddYears(addYears);
            var protectionDocType = Executor.GetQuery<GetDicProtectionDocTypebyCodeQuery>()
                .Process(q => q.Execute(protectionDocTypeCode));

            //todo вынести новые коды в класс
            var protectionDocSubtype = Executor.GetQuery<GetDicProtectionDocSubTypeByCodeQuery>()
                .Process(q => q.Execute(request?.RequestType?.Code + "_PD"));

            //Создаем ОД
            var protectionDocument = new ProtectionDoc
            {
                TypeId = protectionDocType?.Id ?? 0,
                SubTypeId = protectionDocSubtype?.Id ?? 0,
                StatusId = dicProtectionDocStatus.Id,
                SelectionAchieveTypeId = request?.SelectionAchieveTypeId,
                ConventionTypeId = request?.ConventionTypeId,
                BeneficiaryTypeId = request?.BeneficiaryTypeId,
                TypeTrademarkId = request?.TypeTrademarkId,
                RegNumber = request?.RequestNum,
                RegDate = request?.RequestDate,
                GosDate = NiisAmbientContext.Current.DateTimeProvider.Now,
                //ЦValidDate = validDate,
                Referat = request?.Referat,
                NameRu = request?.NameRu,
                NameKz = request?.NameKz,
                NameEn = request?.NameEn,
                SelectionFamily = request?.SelectionFamily,
                Image = request?.Image,
                PreviewImage = request?.PreviewImage,
                IsImageFromName = request?.IsImageFromName ?? false,
                DisclaimerRu = request?.DisclaimerRu,
                DisclaimerKz = request?.DisclaimerKz,
                RequestId = workflowRequest.RequestId,
                AddresseeId = request?.AddresseeId,
                SendTypeId = defaultSendType?.Id
            };

            Executor.GetHandler<GenerateBarcodeHandler>().Process<object>(h => h.Execute(protectionDocument));
            Executor.GetCommand<CreateProtectionDocCommand>().Process(r => r.Execute(protectionDocument));

            var requestProtectionDocSimilar = new RequestProtectionDocSimilar
            {
                RequestId = workflowRequest.RequestId,
                ProtectionDocId = protectionDocument.Id,
                DateCreate = DateTimeOffset.Now
            };

            Executor.GetCommand<CreateRequestProtectionDocSimilarCommand>().Process(r => r.Execute(requestProtectionDocSimilar));

            string initialStageCode;

            if (protectionDocType.Code == DicProtectionDocTypeCodes.ProtectionDocTypeNameOfOriginCode)
            {
                initialStageCode = RouteStageCodes.PD_NMPT_AssignmentRegistrationNumber;
            }
            else if (protectionDocType.Code == DicProtectionDocTypeCodes.ProtectionDocTypeTrademarkCode)
            {
                initialStageCode = RouteStageCodes.PD_TM_AssignmentRegistrationNumber;
            }
            else
            {
                initialStageCode = RouteStageCodes.OD01_1;
            }

            var initialStage = Executor.GetQuery<GetDicRouteStageByCodeQuery>().Process(r => r.Execute(initialStageCode));
            var protectionDocWorkflow = new ProtectionDocWorkflow
            {
                CurrentUserId = workflowRequest.NextStageUserId,
                OwnerId = protectionDocument.Id,
                CurrentStageId = initialStage.Id,
                RouteId = initialStage.RouteId,
                IsComplete = initialStage.IsLast,
                IsSystem = initialStage.IsSystem,
                IsMain = initialStage.IsMain,
            };
            //Создали этап
            Executor.GetCommand<CreateProtectionDocWorkflowCommand>().Process(r => r.Execute(protectionDocWorkflow));

            //Добавили и обновили текущий этап у ОД
            protectionDocument.CurrentWorkflowId = protectionDocWorkflow.Id;
            Executor.GetCommand<UpdateProtectionDocCommand>().Process(r => r.Execute(protectionDocument));

            //Платежи заявки теперь относятся и к ОД (новое требование: не наследовать платежи)
            Executor.GetCommand<UpdateRequestPaymentInvoicesByProtectionDocIdCommand>().Process(r => r.Execute(request.Id, protectionDocument.Id));

            #region Перенос биб. данных 

            var mapper = NiisAmbientContext.Current.Mapper;
            var protectionDocId = protectionDocument.Id;
            var requestId = request.Id;

            //TODO: реализовать перенос TODO! тянуть только то, что нужно в зависимости от типа ОД
            var requestConventionInfos = Executor.GetQuery<GetConventionInfosByRequestIdQuery>().Process(q => q.Execute(requestId));
            var protectionDocConventionInfos = mapper.Map<List<RequestConventionInfo>, List<ProtectionDocConventionInfo>>(requestConventionInfos);
            Executor.GetCommand<AddProtectionDocConventionInfosRangeCommand>().Process(c => c.Execute(protectionDocId, protectionDocConventionInfos));

            var requestEarlyRegs = Executor.GetQuery<GetEarlyRegsByRequestIdQuery>().Process(q => q.Execute(requestId));
            var protectionDocEarlyRegs = mapper.Map<List<RequestEarlyReg>, List<ProtectionDocEarlyReg>>(requestEarlyRegs);
            Executor.GetCommand<AddProtectionDocEarlyRegsRangeCommand>().Process(c => c.Execute(protectionDocId, protectionDocEarlyRegs));

            var icgsRequests = Executor.GetQuery<GetIcgsByRequestIdQuery>().Process(q => q.Execute(requestId));
            var icgsProtectionDocs = mapper.Map<List<ICGSRequest>, List<ICGSProtectionDoc>>(icgsRequests);
            Executor.GetCommand<AddIcgsProtectionDocRangeCommand>().Process(c => c.Execute(protectionDocId, icgsProtectionDocs));

            var icisRequests = Executor.GetQuery<GetIcisByRequestIdQuery>().Process(q => q.Execute(requestId));
            var icisProtectionDocs = mapper.Map<List<ICISRequest>, List<ICISProtectionDoc>>(icisRequests);
            Executor.GetCommand<AddIcisProtectionDocRelationsCommand>().Process(c => c.Execute(protectionDocId, icisProtectionDocs.Select(icis => icis.IcisId).ToList()));

            var ipcRequests = Executor.GetQuery<GetIpcByRequestIdQuery>().Process(q => q.Execute(requestId));
            Executor.GetCommand<AddIpcProtectionDocRelationsCommand>().Process(c => c.Execute(protectionDocId, ipcRequests.ToList()));

            var requestColorTzs = Executor.GetQuery<GetColorTzsByRequestIdQuery>().Process(q => q.Execute(requestId));
            var colorTzIds = requestColorTzs.Select(color => color.ColorTzId).ToList();
            Executor.GetCommand<AddColorTzProtectionDocRelationsCommand>().Process(c => c.Execute(protectionDocId, colorTzIds));

            var icfemRequests = Executor.GetQuery<GetIcfemByRequestIdQuery>().Process(q => q.Execute(requestId));
            var icfemProtectionDocs = mapper.Map<List<DicIcfemRequestRelation>, List<DicIcfemProtectionDocRelation>>(icfemRequests);
            Executor.GetCommand<AddIcfemProtectionDocRelationsCommand>().Process(c => c.Execute(protectionDocId, icfemProtectionDocs.Select(icfem => icfem.DicIcfemId).ToList()));

            var protectionDocInfo = mapper.Map<RequestInfo, ProtectionDocInfo>(request.RequestInfo);
            Executor.GetCommand<CreateProtectionDocInfoCommand>().Process(c => c.Execute(protectionDocId, protectionDocInfo));

            #endregion

            var protectionDocCustomers = mapper.Map<List<RequestCustomer>, List<ProtectionDocCustomer>>(request.RequestCustomers.ToList());
            Executor.GetCommand<AddProtectionDocCustomersCommand>().Process(c => c.Execute(protectionDocId, protectionDocCustomers));
        }

        /// <summary>
        /// Преобразование заявки
        /// </summary>
        /// <param name="requestId">Идентификатор преобразуемой заявки</param>
        private void ConvertRequest(int requestId)
        {

            var request = Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.Execute(requestId));
            var workflows = Executor.GetQuery<GetRequestWorkflowsByRequestIdQuery>().Process(q => q.Execute(requestId));

            #region Генерация уведомления

            var notificationCode = string.Empty;
            var hasBeenOnFullExam = workflows.Any(w =>
                w.CurrentStage.Code == RouteStageCodes.TZFirstFullExpertizePerformerChoosing);
            var trademarkTypeCode = request.SpeciesTradeMark.Code;

            //Смерджили два справочника, получилось так, не стал трогать, мало ли прийдется вернуть
            if (hasBeenOnFullExam)
            {
                if (trademarkTypeCode == DicProtectionDocSubtypeCodes.CollectiveTrademark)
                {
                    notificationCode = DicDocumentTypeCodes.OUT_UV_Pred_preobr_zayav_TZ_na_KTZ_v1_19;
                }
                else
                {
                    notificationCode = DicDocumentTypeCodes.OUT_UV_Pred_preobr_zayav_TZ_na_KTZ_v1_19;
                }
            }
            else
            {
                if (trademarkTypeCode == DicProtectionDocSubtypeCodes.CollectiveTrademark)
                {
                    notificationCode = DicDocumentTypeCodes.OUT_UV_Pred_preobr_zayav_TZ_na_KTZ_v1_19;
                }
                else
                {
                    notificationCode = DicDocumentTypeCodes.OUT_UV_Pred_preobr_zayav_TZ_na_KTZ_v1_19;
                }
            }
            var userInputDto = new UserInputDto
            {
                Code = notificationCode,
                Fields = new List<KeyValuePair<string, string>>(),
                OwnerId = requestId,
                OwnerType = Owner.Type.Request
            };
            Executor.GetHandler<CreateDocumentHandler>().Process(h =>
                h.Execute(requestId, Owner.Type.Request, notificationCode, DocumentType.Outgoing, userInputDto));


            #endregion


            #region Создание заявки

            var protectionDocTypeCode = request.ProtectionDocType.Code;
            var protectionDocSubtypeCode = request.RequestType.Code;
            DicProtectionDocType newProtectionDocType = null;
            DicProtectionDocSubType newProtectionDocSubType = null;
            string initialStageCode = null;
            switch (protectionDocTypeCode)
            {
                //todo перенести коды в классы
                case DicProtectionDocTypeCodes.RequestTypeUsefulModelCode:
                    newProtectionDocType = Executor.GetQuery<GetDicProtectionDocTypebyCodeQuery>().Process(q =>
                        q.Execute(DicProtectionDocTypeCodes.RequestTypeInventionCode));
                    initialStageCode = RouteStageCodes.I_02_1;
                    switch (protectionDocSubtypeCode)
                    {
                        case DicProtectionDocSubtypeCodes.NationalUsefulModel:
                            newProtectionDocSubType = Executor.GetQuery<GetDicProtectionDocSubTypeByCodeQuery>()
                                .Process(q => q.Execute(DicProtectionDocSubtypeCodes.NationalInvention));
                            break;
                        case "03_UsefulModel":
                            newProtectionDocSubType = Executor.GetQuery<GetDicProtectionDocSubTypeByCodeQuery>()
                                .Process(q => q.Execute("03"));
                            break;
                        case "04_UsefulModel":
                            newProtectionDocSubType = Executor.GetQuery<GetDicProtectionDocSubTypeByCodeQuery>()
                                .Process(q => q.Execute("04"));
                            break;
                    }
                    break;
                case DicProtectionDocTypeCodes.RequestTypeInventionCode:
                    newProtectionDocType = Executor.GetQuery<GetDicProtectionDocTypebyCodeQuery>().Process(q =>
                        q.Execute(DicProtectionDocTypeCodes.RequestTypeUsefulModelCode));
                    initialStageCode = RouteStageCodes.UM_02_1;
                    switch (protectionDocSubtypeCode)
                    {
                        case DicProtectionDocSubtypeCodes.NationalInvention:
                            newProtectionDocSubType = Executor.GetQuery<GetDicProtectionDocSubTypeByCodeQuery>()
                                .Process(q => q.Execute(DicProtectionDocSubtypeCodes.NationalUsefulModel));
                            break;
                        case "03":
                            newProtectionDocSubType = Executor.GetQuery<GetDicProtectionDocSubTypeByCodeQuery>()
                                .Process(q => q.Execute("03_UsefulModel"));
                            break;
                        case "04":
                            newProtectionDocSubType = Executor.GetQuery<GetDicProtectionDocSubTypeByCodeQuery>()
                                .Process(q => q.Execute("04_UsefulModel"));
                            break;
                    }
                    break;
                case DicProtectionDocTypeCodes.RequestTypeTrademarkCode:
                    if (request.SpeciesTradeMark.Code == DicProtectionDocSubtypeCodes.CollectiveTrademark)
                    {
                        newProtectionDocSubType = Executor.GetQuery<GetDicProtectionDocSubTypeByCodeQuery>()
                            .Process(q => q.Execute(DicProtectionDocSubtypeCodes.RegularTradeMark));
                    }
                    else
                    {
                        newProtectionDocSubType = Executor.GetQuery<GetDicProtectionDocSubTypeByCodeQuery>()
                            .Process(q => q.Execute(DicProtectionDocSubtypeCodes.CollectiveTrademark));
                    }

                    request.SpeciesTradeMark = newProtectionDocSubType;
                    request.SpeciesTradeMarkId = newProtectionDocSubType.Id;

                    Executor.GetCommand<UpdateRequestCommand>().Process(c => c.Execute(request));
                    return;
            }
            if (initialStageCode == null)
            {
                return;
            }
            var newRequest = _mapper.Map<Request>(request);
            newRequest.ProtectionDocTypeId = newProtectionDocType?.Id ?? request.ProtectionDocTypeId;
            newRequest.RequestTypeId = newProtectionDocSubType?.Id ?? request.RequestTypeId;
            newRequest.DateCreate = DateTimeOffset.Now;
            Executor.GetHandler<GenerateRequestNumberHandler>().Process(h => h.Execute(newRequest));
            var newRequestId = Executor.GetCommand<CreateRequestCommand>().Process(c => c.Execute(newRequest));

            var initialStage = Executor.GetQuery<GetDicRouteStageByCodeQuery>()
                .Process(q => q.Execute(initialStageCode));

            var initialWorkflow = new RequestWorkflow
            {
                CurrentUserId = WorkflowRequest.NextStageUserId,
                OwnerId = newRequestId,
                CurrentStageId = initialStage.Id,
                RouteId = initialStage.RouteId,
                IsComplete = initialStage.IsLast,
                IsSystem = initialStage.IsSystem,
                IsMain = initialStage.IsMain,
            };
            Executor.GetCommand<CreateRequestWorkflowCommand>()
                .Process(c => c.Execute(initialWorkflow));

            newRequest.CurrentWorkflowId = initialWorkflow.Id;
            newRequest.StatusId = initialStage.RequestStatusId;
            Executor.GetCommand<UpdateRequestCommand>().Process(c => c.Execute(newRequest));

            #endregion

            #region Биб. данные

            var newConventionInfos = _mapper.Map<RequestConventionInfo[]>(request.RequestConventionInfos);
            foreach (var conventionInfo in newConventionInfos)
            {
                conventionInfo.RequestId = newRequestId;
                Executor.GetCommand<CreateConventionInfoCommand>().Process(c => c.Execute(conventionInfo));
            }

            var newEarlyRegs = _mapper.Map<RequestEarlyReg[]>(request.EarlyRegs);
            foreach (var earlyReg in newEarlyRegs)
            {
                earlyReg.RequestId = newRequestId;
                Executor.GetCommand<CreateRequestEarlyRegCommand>().Process(c => c.Execute(earlyReg));
            }

            var newIcgsRequests = _mapper.Map<ICGSRequest[]>(request.ICGSRequests);
            foreach (var icgsRequest in newIcgsRequests)
            {
                icgsRequest.RequestId = newRequestId;
                Executor.GetCommand<CreateRequestIcgsCommand>().Process(c => c.Execute(icgsRequest));
            }

            var newIpcRequests = _mapper.Map<IPCRequest[]>(request.IPCRequests);
            foreach (var ipcRequest in newIpcRequests)
            {
                ipcRequest.RequestId = newRequestId;
                Executor.GetCommand<CreateIpcRequestCommand>().Process(c => c.Execute(ipcRequest));
            }

            var newIcisRequests = _mapper.Map<ICISRequest[]>(request.ICISRequests);
            foreach (var icisRequest in newIcisRequests)
            {
                icisRequest.RequestId = newRequestId;
                Executor.GetCommand<CreateRequestIcisCommand>().Process(c => c.Execute(icisRequest));
            }

            foreach (var colorTz in request.ColorTzs)
            {
                var newColorTz = new DicColorTZRequestRelation
                {
                    ColorTzId = colorTz.ColorTzId,
                    RequestId = newRequestId
                };
                Executor.GetCommand<CreateRequestColorTzCommand>().Process(c => c.Execute(newColorTz));
            }

            foreach (var icfem in request.Icfems)
            {
                var newIcfem = new DicIcfemRequestRelation
                {
                    DicIcfemId = icfem.DicIcfemId,
                    RequestId = newRequestId
                };
                Executor.GetCommand<CreateRequestIcfemCommand>().Process(c => c.Execute(newIcfem));
            }

            #endregion

            #region Контрагенты

            var newCustomers = _mapper.Map<RequestCustomer[]>(request.RequestCustomers);
            foreach (var customer in newCustomers)
            {
                customer.RequestId = newRequestId;
                Executor.GetCommand<CreateRequestCustomerCommand>().Process(c => c.Execute(customer));
            }

            #endregion
        }

        private string[] GetTariffCodesForStageInputPayment()
        {
            var request = Executor.GetQuery<GetRequestByIdQuery>().Process(r => r.Execute(WorkflowRequest.CurrentWorkflowObject.Id));

            var tariffCodesForI_02_2 = new List<string>();


            var beneficiaryType = request.RequestCustomers
                .Where(c => c.CustomerRole.Code == DicCustomerRoleCodes.Declarant)
                .Select(c => c.Customer.BeneficiaryType)
                .FirstOrDefault();

            var isNotResident = request.RequestCustomers
                .Where(c => c.CustomerRole.Code == DicCustomerRoleCodes.Declarant)
                .Select(c => c.Customer.IsNotResident)
                .FirstOrDefault();

            if (request.EarlyRegs != null && request.EarlyRegs.Any()
                && beneficiaryType != null && beneficiaryType.Code == DicBeneficiaryTypeCodes.SMB
                && isNotResident == false)
            {
                tariffCodesForI_02_2.Add(DicTariff.Codes.AcceptanceApplicationsConventionalPriorityAafterDeadline);
            }

            else if (request.ReceiveType != null && request.ReceiveType.Code == DicReceiveTypeCodes.Courier)
            {
                if (beneficiaryType != null && beneficiaryType.Code == DicBeneficiaryTypeCodes.SMB)
                {
                    tariffCodesForI_02_2.Add(DicTariff.Codes.InventionFormalExaminationOnPurpose);
                }
                else if (beneficiaryType != null && beneficiaryType.Code == DicBeneficiaryTypeCodes.VET)
                {
                    tariffCodesForI_02_2.Add(DicTariff.Codes.InventionAcceleratedFormalExaminationOnPurpose);
                }
            }
            else if (request.ReceiveType != null && request.ReceiveType.Code == DicReceiveTypeCodes.ElectronicFeed
                                                || request.ReceiveType.Code == DicReceiveTypeCodes.ElectronicFeedEgov)
            {
                if (beneficiaryType != null && beneficiaryType.Code == DicBeneficiaryTypeCodes.SMB)
                {
                    tariffCodesForI_02_2.Add(DicTariff.Codes.InventionFormalExaminationEmail);
                }
                else if (beneficiaryType != null && beneficiaryType.Code == DicBeneficiaryTypeCodes.VET)
                {
                    tariffCodesForI_02_2.Add(DicTariff.Codes.InventionAcceleratedFormalExaminationEmail);
                }
            }


            if (tariffCodesForI_02_2.Any() == false)
            {
                tariffCodesForI_02_2.Add(DicTariff.Codes.InventionFormalExaminationOnPurpose);
            }

            return tariffCodesForI_02_2.ToArray();
        }
    }
}