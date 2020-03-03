using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;
using System;
using System.Linq;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.WorkflowBusinessLogic.ProtectionDocuments;
using Iserv.Niis.WorkflowBusinessLogic.RequestDocuments;
using Iserv.Niis.WorkflowBusinessLogic.Requests;

namespace Iserv.Niis.WorkflowBusinessLogic.WorkflowTaskQueues
{
    /// <summary>
    /// Класс, который представляет запрос, который расчитывает время выполнения запланированной задачи по идентификатору сущности, его типу и следующему этапу рабочего процесса.
    /// <para></para>
    /// Не могу комментировать методы, слишком много бизнес-логики, которую я не понимаю.
    /// </summary>
    public class CalculateWorkflowTaskQueueResolveDateHandler : BaseHandler
    {
        /// <summary>
        /// Выполенение запроса.
        /// </summary>
        /// <param name="ownerId">Идентификатор родительской сущности.</param>
        /// <param name="ownerType">Тип родительской сущности.</param>
        /// <param name="nextStageCode">Код следующего этапа рабочего процесса.</param>
        /// <returns></returns>
        public DateTimeOffset Execute(int ownerId, Owner.Type ownerType, string nextStageCode)
        {
            if (Properties.Settings.Default.IsAutoWorkflowInTestMode == true)
            {
                return DateTimeOffset.Now + Properties.Settings.Default.AutoWorkflowTestDelayTime;
            }
            DateTimeOffset resolveDate;
            switch (ownerType)
            {
                case Owner.Type.Request:
                    var request = Executor.GetQuery<GetRequestByIdQuery>().Process(r => r.Execute(ownerId));
                    resolveDate = GetResolveDate(request, nextStageCode);
                    break;
                case Owner.Type.ProtectionDoc:
                    var protectionDoc = Executor.GetQuery<GetProtectionDocByIdQuery>().Process(q => q.Execute(ownerId));
                    resolveDate = GetExecutionDate(protectionDoc, DateTimeOffset.Now, nextStageCode);
                    break;
                case Owner.Type.Contract:
                default:
                    throw new NotImplementedException();
            }

            return resolveDate;
        }

        /// <summary>
        /// Возвращает время выполнения задачи для заявки по коду следующего этапа рабочего процесса.
        /// </summary>
        /// <param name="request">Заявка.</param>
        /// <param name="nextStageCode">Код рабочего процесса.</param>
        /// <returns>Время выполнения запланированной задачи.</returns>
        private DateTimeOffset GetResolveDate(Request request, string nextStageCode)
        {
            var startDate = GetStartDate(request);
            var executionDate = GetExecutionDate(request, startDate, nextStageCode);

            return executionDate;
        }
        
        private DateTimeOffset GetExecutionDate(Request request, DateTimeOffset startDate, string nextStageCode)
        {
            var calendarProvider = NiisAmbientContext.Current.CalendarProvider;

            var currentStage = request.CurrentWorkflow.CurrentStage;
            //if (currentStage.ExpirationValue == null) throw new Exception("");

            var executionDate = calendarProvider.GetExecutionDate(startDate, currentStage.ExpirationType, currentStage.ExpirationValue ?? 0);

            //Этапы передачи в Госреестр
            if (new[] {RouteStageCodes.TZ_03_3_8}.Contains(currentStage.Code))
            {
                executionDate = new DateTimeOffset(NiisAmbientContext.Current.DateTimeProvider.Now.Year,
                    NiisAmbientContext.Current.DateTimeProvider.Now.Month,
                    NiisAmbientContext.Current.DateTimeProvider.Now.Day, 20, 0, 0,
                    TimeZoneInfo.Local.GetUtcOffset(NiisAmbientContext.Current.DateTimeProvider.Now));
            }

            // Этапы, которые имеют накопительные даты (5, 15, 25)
            if (new[] { RouteStageCodes.TZ_03_2_2_1, RouteStageCodes.TZ_03_3_2 }.Contains(currentStage.Code))
            {
                executionDate = calendarProvider.GetFullExaminationDate(startDate);
            }

            // Этапы, которые имеют накопительные даты (1, 10, 20)
            if (new[] { RouteStageCodes.I_02_2_1, RouteStageCodes.I_03_2_1_1, RouteStageCodes.I_02_2_1_0_0, RouteStageCodes.I_03_2_1__1 }.Contains(currentStage.Code))
            {
                executionDate = calendarProvider.GetFormalExaminationDate(startDate);
            }

            // Этапы, которые имеют накопительные даты (15, 30)
            if (new[] { RouteStageCodes.TZ_03_3_8 }.Contains(currentStage.Code))
            {
                executionDate = calendarProvider.GetTransferToGosreestrDate(startDate);
            }

            // Продление срока специфичное (3 месяца)
            if (currentStage.Code.Equals(RouteStageCodes.I_03_3_1_1_0) && request.CurrentWorkflow.FromStage.Code.Equals(RouteStageCodes.I_03_3_4_1))
            {
                executionDate = calendarProvider.GetExecutionDate(startDate, ExpirationType.CalendarMonth, 3);
            }

            if (nextStageCode.Equals(RouteStageCodes.NMPT_03_2))
            {
                executionDate = calendarProvider.GetExecutionDate(DateTimeOffset.Now, ExpirationType.CalendarMonth, 2);
            }

            if (currentStage.Code.Equals(RouteStageCodes.ITZ_03_3_4_5_1))
            {
                var cnt = request.PaymentInvoices.Where(pi => pi.Tariff.Code.Equals(DicTariff.Codes.ExpertiseConclusionObjectionTermExtensionMonthly)).Sum(pi => pi.TariffCount);
                cnt = cnt > 6 ? 6 : cnt;
                executionDate = calendarProvider.GetExecutionDate(startDate, ExpirationType.CalendarMonth, (short)(cnt ?? 1));
            }

            if (new[] {RouteStageCodes.I_03_2_1, RouteStageCodes.I_03_2_4,RouteStageCodes.I_03_9, RouteStageCodes.I_03_3_1__1}.Contains(currentStage.Code))
            {
                var cnt = request.PaymentInvoices.Where(pi => pi.Tariff.Code.Equals(DicTariffCodes.BProlongationRequestDocumentsTerm)).Sum(pi => pi.TariffCount);
                cnt = cnt > 6 ? 6 : cnt;
                executionDate = calendarProvider.GetExecutionDate(startDate, ExpirationType.CalendarMonth, (short)(cnt ?? 1));
            }

            if (currentStage.Code.Equals(RouteStageCodes.TZ_03_3_7_3))
            {
                var cnt = request.PaymentInvoices.Where(pi => pi.Tariff.Code.Equals(DicTariff.Codes.ExpertiseConclusionObjectionTermExtensionMonthly)).Sum(pi => pi.TariffCount);
                cnt = cnt > 6 ? 6 : cnt;
                executionDate = calendarProvider.GetExecutionDate(startDate, ExpirationType.CalendarMonth, (short)(cnt ?? 1));
            }
            if (currentStage.Code.Equals(RouteStageCodes.TZ_03_3_7_4))
            {
                executionDate = calendarProvider.GetExecutionDate(DateTimeOffset.Now, ExpirationType.CalendarMonth, 2);
            }

            if (currentStage.Code.Equals(RouteStageCodes.UM_02_2_6))
            {
                var queue = Executor.GetQuery<GetLastWorkflowTaskQueuesByRequestIdAndCodeQuery>().Process(r => r.Execute(request.Id, RouteStageCodes.UM_03_2_1));
                if (queue == null)
                {
                    queue = Executor.GetQuery<GetLastWorkflowTaskQueuesByRequestIdAndCodeQuery>().Process(r => r.Execute(request.Id));
                }

                var invoice = request.PaymentInvoices.LastOrDefault(pi => pi.Tariff.Code.Equals(DicTariff.Codes.ExtensionAndRestorationTimesForAnswerToExaminationRequest));
                var cnt = invoice != null ? invoice.TariffCount ?? 0 : 0;
                cnt = cnt > 3 ? 3 : cnt;

                executionDate = queue.ResolveDate.AddMonths(cnt);
            }

            if (currentStage.Code.Equals(RouteStageCodes.UM_02_2_7))
            {
                var doc = request.Documents.Any(a => a.Document.Type.Code == DicDocumentTypeCodes.PetitionForExtendTime)
                    && request.PaymentInvoices.Any(pi => pi.Tariff.Code.Equals(DicTariff.Codes.InventionExaminationOnUsefulModelOnPaper) /*&& pi.Status.Code != "notpaid"*/);
                executionDate = calendarProvider.GetExecutionDate(startDate, ExpirationType.CalendarMonth, 2);
            }

            if (currentStage.Code.Equals(RouteStageCodes.UM_03_8))
            {
                var doc = request.Documents.Any(a => a.Document.Type.Code == DicDocumentTypeCodes.PetitionForExtendTime)
                    && request.PaymentInvoices.Any(pi => pi.Tariff.Code.Equals(DicTariff.Codes.InventionExaminationOnUsefulModelOnPaper) /*&& pi.Status.Code != "notpaid"*/);
                executionDate = calendarProvider.GetExecutionDate(request.DateCreate, ExpirationType.CalendarMonth, 12);
            }

            if (currentStage.Code.Equals(RouteStageCodes.NMPT_03_7))
            {
                var doc = request.Documents.FirstOrDefault(rd => rd.Document.Type.Code == DicDocumentTypeCodes.NotificationOfRegistrationDecision);
                if (doc != null && doc.Document.SendingDate.HasValue)
                {
                    executionDate =
                        calendarProvider.GetExecutionDate(doc.Document.SendingDate.Value, ExpirationType.CalendarMonth, 3);
                }
            }

            if (currentStage.Code.Equals(RouteStageCodes.UM_03_7_0))
            {
                var doc = request.Documents.FirstOrDefault(rd => rd.Document.Type.Code == DicDocumentTypeCodes.UV_KPM);
                if (doc != null && doc.Document.SendingDate.HasValue)
                {
                    executionDate =
                        calendarProvider.GetExecutionDate(doc.Document.SendingDate.Value, ExpirationType.CalendarMonth, 3);
                }
            }

            if (currentStage.Code.Equals(RouteStageCodes.TZ_02_2))
            {
                executionDate = calendarProvider.GetExecutionDate(startDate, ExpirationType.CalendarDay, 2);
            }

               
            return executionDate;
        }

        private DateTimeOffset GetStartDate(Request request)
        {
            var currentStageCode = request.CurrentWorkflow.CurrentStage.Code;
            var workflows = Executor.GetQuery<GetRequestWorkflowsByRequestIdQuery>()
                .Process(q => q.Execute(request.Id));
            var startDate = workflows.Where(w => w.CurrentStage.Code == currentStageCode).OrderBy(w => w.DateCreate)
                            .FirstOrDefault()?.DateCreate ?? DateTimeOffset.Now;
            var fromStageCode = request.CurrentWorkflow.FromStage?.Code;

            // Этапы, которые начинают отсчет с даты подачи заявки
            if (new[] { RouteStageCodes.TZ_03_2_2, RouteStageCodes.TZ_03_3_2, RouteStageCodes.UM_02_2_7, RouteStageCodes.NMPT_02_2_0 }.Contains(currentStageCode))
            {
                startDate = request.DateCreate;
            }

            // Этапы, которые начинают отсчет с даты направления запроса
            if (new[] { RouteStageCodes.TZ_03_3_7_1, RouteStageCodes.I_03_3_1_1 }.Contains(currentStageCode))
            {
                if (fromStageCode != null && fromStageCode.Equals
                        (new string[]
                            {
                                RouteStageCodes.TZ_03_2_2,
                                RouteStageCodes.TZ_03_3_2_2,
                                RouteStageCodes.TZConvert,
                                RouteStageCodes.TZ_06
                            }
                        )
                    )
                {
                    startDate = GetLastDocument(request.Id, DicDocumentTypeCodes.OUT_Zap_Pol_dover_v1_19).DateCreate;
                }

                if (fromStageCode != null && fromStageCode.Equals(RouteStageCodes.TZ_03_3_2))
                {
                    startDate = request.DateCreate;
                }

                if (fromStageCode != null && fromStageCode.Equals(RouteStageCodes.I_03_2_1))
                {
                    startDate = GetLastDocument(request.Id, DicDocumentTypeCodes.RequestForFormalExamForInvention).DateCreate;
                }

                if (fromStageCode != null && fromStageCode.Equals(RouteStageCodes.I_03_2_4))
                {
                    startDate = GetLastDocument(request.Id, DicDocumentTypeCodes.RequestForExaminationOfInventionPatentRequest, DicDocumentTypeCodes.RequestForSubstantiveExamination).DateCreate;
                }
            }

            // Этапы, которые начинают отсчет с даты направления уведомления
            if (new[] { RouteStageCodes.TZ_03_3_7_4, RouteStageCodes.I_02_2_0, RouteStageCodes.I_03_2_1_0 }.Contains(currentStageCode))
            {
                if (fromStageCode != null && fromStageCode.Equals(RouteStageCodes.TZ_03_3_7_1))
                {
                    startDate = GetLastDocument(request.Id, DicDocumentTypeCodes.NotificationOfAnswerTimeExpiration).DateCreate;
                }

                if (fromStageCode != null && fromStageCode.Equals(RouteStageCodes.TZ_03_2_2_0))
                {
                    startDate = GetLastDocument(request.Id, DicDocumentTypeCodes.NotificationOfRegistrationExaminationTimeExpiration).DateCreate;
                }

                if (fromStageCode != null && (fromStageCode.Equals(RouteStageCodes.I_03_2_1) || fromStageCode.Equals(RouteStageCodes.I_03_2_1_1)))
                {
                    startDate = GetLastDocument(request.Id, DicDocumentTypeCodes.NotificationForPozitiveFormalExamination, DicDocumentTypeCodes.NotificationForPozitiveFormalExaminationKz).DateCreate;
                }

                if (fromStageCode != null && (fromStageCode.Equals(RouteStageCodes.I_03_2_1) || fromStageCode.Equals(RouteStageCodes.I_03_2_1_1)))
                {
                    startDate = GetLastDocument(request.Id, DicDocumentTypeCodes.NotificationForPozitiveFormalExamination, DicDocumentTypeCodes.NotificationForPozitiveFormalExaminationKz).DateCreate;
                }
            }

            if (new[] { RouteStageCodes.TZ_03_2_2_0 }.Contains(currentStageCode))
            {
                startDate = GetLastDocument(request.Id, DicDocumentTypeCodes.NotificationOfTmRequestReviewingAcceptance, DicDocumentTypeCodes.NotificationOfTmRequestReviewingAcceptance).DateCreate;
            }

            if (new[] { RouteStageCodes.TZ_03_3_7 }.Contains(currentStageCode))
            {
                if (fromStageCode != null && fromStageCode.Equals(RouteStageCodes.TZ_03_3_6))
                {
                    startDate = GetLastDocument(request.Id, DicDocumentTypeCodes.NotificationOfRegistrationDecision).DateCreate;
                }

                if (fromStageCode != null && fromStageCode.Equals(RouteStageCodes.TZ_03_3_9_2_0))
                {
                    startDate = GetLastDocument(request.Id, DicDocumentTypeCodes.NotificationOfTmRegistration, DicDocumentTypeCodes.OUT_UV_Pol_Reg_TZ_AP_sov_v1_19).DateCreate;
                }
            }

            if (new[] { RouteStageCodes.TZ_03_3_9_1 }.Contains(currentStageCode))
            {
                startDate = GetLastDocument(request.Id, DicDocumentTypeCodes.ExpertRefusalOpinionFinal).DateCreate;
            }

            // Этапы, которые начинают отсчет с даты направления заключения
            if (new[] { RouteStageCodes.TZ_03_3_4_4 }.Contains(currentStageCode))
            {
                startDate = GetLastDocument(request.Id, DicDocumentTypeCodes.ExpertTmRegistrationOpinionWithDisclaimer).DateCreate;
            }

            if (new[] { RouteStageCodes.TZ_03_3_4_4_0 }.Contains(currentStageCode))
            {
                startDate = GetLastDocument(request.Id, DicDocumentTypeCodes.ExpertTmRegisterRefusalOpinion).DateCreate;
            }

            if (currentStageCode.Equals(RouteStageCodes.I_03_3_4_1) && fromStageCode != null && fromStageCode.Equals(RouteStageCodes.I_03_3_3))
            {
                startDate = GetLastDocument(request.Id, DicDocumentTypeCodes.ConclusionOfInventionPatentGrantRefuse).DateCreate;
            }

            // Этапы, которые начинают отсчет с даты получения возражения
            if (new[] { RouteStageCodes.TZ_03_3_2_0, RouteStageCodes.TZ_03_3_9_2, RouteStageCodes.I_03_3_4_1_0 }.Contains(currentStageCode))
            {
                startDate = GetLastDocument(request.Id, DicDocumentTypeCodes.Objection).DateCreate;
            }

            return startDate;
        }

        private DateTimeOffset GetExecutionDate(ProtectionDoc protectionDoc, DateTimeOffset startDate, string nextStageCode)
        {
            var calendarProvider = NiisAmbientContext.Current.CalendarProvider;

            var currentStage = protectionDoc.CurrentWorkflow.CurrentStage;

            var executionDate = calendarProvider.GetExecutionDate(startDate, currentStage.ExpirationType, currentStage.ExpirationValue ?? 0);

            if (nextStageCode == RouteStageCodes.OD03_3)
            {
                switch (protectionDoc.Type.Code)
                {
                    case DicProtectionDocTypeCodes.ProtectionDocTypeTrademarkCode:
                    case DicProtectionDocTypeCodes.ProtectionDocTypeNameOfOriginCode:
                        executionDate = protectionDoc.ValidDate?.AddYears(10) ?? DateTimeOffset.Now.AddYears(20);
                        break;
                    case DicProtectionDocTypeCodes.ProtectionDocTypeInventionCode:
                        executionDate = protectionDoc.ValidDate?.AddYears(5) ?? DateTimeOffset.Now.AddYears(20);
                        break;
                    case DicProtectionDocTypeCodes.ProtectionDocTypeIndustrialSampleCode:
                        executionDate = protectionDoc.ValidDate?.AddYears(5) ?? DateTimeOffset.Now.AddYears(20);
                        break;
                    case DicProtectionDocTypeCodes.ProtectionDocTypeUsefulModelCode:
                        executionDate = protectionDoc.ValidDate?.AddYears(3) ?? DateTimeOffset.Now.AddYears(20);
                        break;
                    case DicProtectionDocTypeCodes.ProtectionDocTypeSelectionAchieveCode:
                        executionDate = protectionDoc.ValidDate?.AddYears(10) ?? DateTimeOffset.Now.AddYears(20);
                        break;
                }
            }
            if (nextStageCode == RouteStageCodes.OD05_02)
            {
                executionDate = protectionDoc?.MaintainDate ?? DateTimeOffset.Now.AddYears(1);
            }

            return executionDate;
        }

        private Domain.Entities.Document.Document GetLastDocument(int requestId, params string[] typeCode)
        {
            var document = Executor.GetQuery<GetLastRequestsDocumentQuery>().Process(r => r.Execute(requestId, typeCode))?.Document;

            if (document == null)
            {
                throw new Exception($"Document with type \"{typeCode}\" not found in request {requestId}!");
            }

            return document;
        }
    }
}
