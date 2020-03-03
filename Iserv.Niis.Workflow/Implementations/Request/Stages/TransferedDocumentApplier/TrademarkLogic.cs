using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Workflow.Abstract;

namespace Iserv.Niis.Workflow.Implementations.Request.Stages.TransferedDocumentApplier
{
    public class TrademarkLogic : BaseLogic
    {
        private readonly Dictionary<string, Func<Domain.Entities.Document.Document, Domain.Entities.Request.Request, Expression<Func<DicRouteStage, bool>>>> _logicMap;

        public TrademarkLogic(
            NiisWebContext context,
            IWorkflowApplier<Domain.Entities.Request.Request> workflowApplier)
            : base(workflowApplier, context)
        {
            _logicMap = new Dictionary<string, Func<Domain.Entities.Document.Document, Domain.Entities.Request.Request, Expression<Func<DicRouteStage, bool>>>>
            {
                { DicDocumentType.Codes.ExpertTmRegisterRefusalOpinion, RefusalConclusionLogic },
                { DicDocumentType.Codes.ExpertTmRegisterOpinion, RegistrationConclusionLogic },
                { DicDocumentType.Codes.ExpertTmRegistrationOpinionWithDisclaimer, RefusalConclusionLogic },
                { DicDocumentType.Codes.ExpertTmRegisterFinalOpinion, FinalRegistrationConclusionLogic },
                { DicDocumentType.Codes.ExpertTmRegistrationFinalOpinionWithApplicantConsent, FinalRegistrationConclusionLogic },
                { DicDocumentType.Codes.ExpertTmRegistrationFinalOpinionWithoutApplicantConsent, FinalRegistrationConclusionLogic },
                { DicDocumentType.Codes.NotificationOfTrademarkRequestRegistation, ReadyToFullExaminationForPred1Logic },
                { DicDocumentType.Codes.NotificationOfProvidelessOfficeWorkTermination, RequestlessWorkStopLogic },
                { DicDocumentType.Codes.RequestForPreExamination, SentRequestLogic },
                { DicDocumentType.Codes.ExpertRefusalOpinionFinal, FinalRejectionLogic },
                { DicDocumentType.Codes.NotificationOfPaymentlessOfficeWorkTermination, WaitStageExpiredWorkStopLogic },
                { DicDocumentType.Codes.NotificationOfRefusal, WaitStageExpiredWorkStopLogic },
                { DicDocumentType.Codes.NotificationOfAnswerlessOfficeWorkTermination, AnswerlessWorkStopLogic },
                { DicDocumentType.Codes.RequestForFullExamination, FullExaminationRequestLogic },
                { DicDocumentType.Codes.NotificationOfTmRegistration, PreparationToGosRegisterTransferLogic },
                { DicDocumentType.Codes.NotificationOfRegistrationExaminationTimeExpiration, RestorationWait88Logic },
                { DicDocumentType.Codes.NotificationOfAnswerTimeExpiration, RestorationWait89Logic },
                { DicDocumentType.Codes.NotificationOfTmRequestReviewingAcceptance, FullExaminationPaymentExpectationLogic },
                { DicDocumentType.Codes.NotificationOfFormalExaminationTimeExpiration, PreExaminationPaymentExpectationLogic },
                { DicDocumentType.Codes.NotificationOfRegistrationDecision, ReturnFromMinistryWhenLogic }
            };
        }

        public override async Task ApplyAsync(RequestDocument requestDocument)
        {
            await ApplyStageAsync(GetStagePredicate(requestDocument), requestDocument.Request);
        }

        /// <summary>
        /// Логика обработки этапов при исходящем документе "7.12_Уведомление о принятии решения о регистрации"
        /// </summary>
        /// <param name="document">Документ</param>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Подготовка для передачи в Госреестр"</returns>
        private Expression<Func<DicRouteStage, bool>> ReturnFromMinistryWhenLogic(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(document, "OUT03.1"))
            {
                // Возвращено с МЮ РК
                if (CurrentStageContains(request, "TM03.3.6"))
                {
                    // Подготовка для передачи в Госреестр
                    return s => s.Code.Equals("TM03.3.7");
                }
            }
            
            return null;
        }

        /// <summary>
        /// Логика обработки этапов при исходящем документе "89_1_Уведомление (промежуточное, об истечении сроков предоставления оплаты за прием и проведение форм.экспертизы) ТЗ"
        /// </summary>
        /// <param name="document">Документ</param>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Ожидаемые оплату за прием и проведение предварительной экспертизы"</returns>
        private Expression<Func<DicRouteStage, bool>> PreExaminationPaymentExpectationLogic(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(document, "OUT03.1"))
            {
                if (CurrentStageContains(request, "TM02.2"))
                {
                    return s => s.Code.Equals("TM02.2.2");
                }
            }

            return null;
        }

        /// <summary>
        /// Логика обработки этапов при исходящем документе "Уведомление о принятии к рассмотрению заявки на регистрацию ТЗ_знака обслуживания (без оплаты за полную экспертизу)"
        /// </summary>
        /// <param name="document">Документ</param>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Ожидаемые оплату за полную экспертизу"</returns>
        private Expression<Func<DicRouteStage, bool>> FullExaminationPaymentExpectationLogic(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(document, "OUT03.1"))
            {
                if (CurrentStageContains(request, "TM03.2.2") && !HasPaidInvoices(request, DicTariff.Codes.TmRequestExamination, DicTariff.Codes.TmRequestExaminationWithThreeOrMoreClass))
                {
                    return s => s.Code.Equals("TM03.2.2.0");
                }
            }

            return null;
        }

        /// <summary>
        /// Логика обработки этапов при исходящем документе "89_Уведомление (промежуточное, об истечении сроков предоставления ответа на запрос экспертизы)"
        /// </summary>
        /// <param name="document">Документ</param>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Ожидание восстановления пропущенного срока"</returns>
        private Expression<Func<DicRouteStage, bool>> RestorationWait89Logic(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(document, "OUT03.1"))
            {
                if (CurrentStageContains(request, "X01")
                    && FromStageContains(request, "TM03.3.7.1"))
                {
                    return s => s.Code.Equals("TM03.3.7.0");
                }
            }

            return null;
        }

        /// <summary>
        /// Логика обработки этапов при исходящем документе "88_Уведомление (промежуточное, об истечении сроков предоставления оплаты за проведение экспертизы на регистрацию товарного знака)"
        /// </summary>
        /// <param name="document">Документ</param>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Ожидание восстановления пропущенного срока"</returns>
        private Expression<Func<DicRouteStage, bool>> RestorationWait88Logic(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(document, "OUT03.1"))
            {
                if (CurrentStageContains(request, "X01") && FromStageContains(request, "TM03.2.2.0"))
                {
                    return s => s.Code.Equals("TM03.3.7.0");
                }
            }

            return null;
        }

        /// <summary>
        /// Логика обработки этапов при входящем документе "УВЕДОМЛЕНИЕ о регистрации товарного знака"
        /// </summary>
        /// <param name="document">Документ</param>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Подготовка для передачи в Госреестр"</returns>
        private Expression<Func<DicRouteStage, bool>> PreparationToGosRegisterTransferLogic(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(document, "OUT03.1"))
            {
                // Решение Апелляционного Совета
                if (CurrentStageContains(request, "TM03.3.9.2.0")
                    && AnyDocuments(request, DicDocumentType.Codes.DecisionOfAppealsBoard))
                {
                    // Подготовка для передачи в Госреестр
                    return s => s.Code.Equals("TM03.3.7");
                }
            }

            return null;
        }

        /// <summary>
        /// Логика обработки этапов при исходящем документе "Запрос полной экспертизы"
        /// </summary>
        /// <param name="document">Документ</param>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Направлен запрос"</returns>
        private Expression<Func<DicRouteStage, bool>> FullExaminationRequestLogic(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(document, "OUT03.1"))
            {
                if (CurrentStageContains(request, "TM03.3.2"))
                {
                    return s => s.Code.Equals("TM03.3.7.1");
                }
            }

            return null;
        }

        /// <summary>
        /// Логика обработки этапов при исходящем документе "Уведомление о прекращении делопроизводства (в связи с отстутствием ответа на запрос) (полная экспертиза)"
        /// </summary>
        /// <param name="document">Документ</param>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Делопроизводство прекращено"</returns>
        private Expression<Func<DicRouteStage, bool>> AnswerlessWorkStopLogic(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(document, "OUT03.1"))
            {
                if (CurrentStageContains(request, "TM03.3.7.0") &&
                    FromStageContains(request, "TM03.3.7.3"))
                {
                    return s => s.Code.Equals("TM03.3.9");
                }
            }
            
            return null;
        }

        /// <summary>
        /// Логика обработки этапов при исходящих документах "85_Уведомление о прекращении делопроизводства (в связи с неуплатой за полную экспертизу)" 
        /// или "Уведомление об отказе"
        /// </summary>
        /// <param name="document">Документ</param>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Делопроизводство прекращено"</returns>
        private Expression<Func<DicRouteStage, bool>> WaitStageExpiredWorkStopLogic(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(document, "OUT03.1"))
            {
                if (CurrentStageContains(request, "X01") && FromStageContains(request, "TM03.2.2.0"))
                {
                    return s => s.Code.Equals("TM03.3.9");
                }
            }
            
            return null;
        }

        /// <summary>
        /// Логика обработки этапов при входящем документе "Экспертное заключение об отказе (окончательное)"
        /// </summary>
        /// <param name="document">Документ</param>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Направлено заявителю заключение об окончательном отказе"</returns>
        private Expression<Func<DicRouteStage, bool>> FinalRejectionLogic(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(document, "OUT02.1"))
            {
                if (CurrentStageContains(request, "TM03.3.2.0", "TM03.3.2.0.0"))
                {
                    // Окончательное экспертное заключение
                    return s => s.Code.Equals("TM03.3.3.0");
                }
            }

            if (CurrentStageContains(document, "OUT02.3"))
            {
                // Окончательное экспертное заключение
                if (CurrentStageContains(request, "TM03.3.3.0"))
                {
                    // На утверждение директору
                    return s => s.Code.Equals("TM03.3.3.1");
                }
            }

            if (CurrentStageContains(document, "OUT02.3"))
            {
                // Окончательное экспертное заключение
                if (CurrentStageContains(request, "TM03.3.3.0"))
                {
                    // На утверждение директору
                    return s => s.Code.Equals("TM03.3.3.1");
                }
            }

            if (CurrentStageContains(document, "OUT03.1"))
            {
                // Утверждено директором
                if (CurrentStageContains(request, "TM03.3.4"))
                {
                    // Передано в МЮ РК
                    return s => s.Code.Equals("TM03.3.5");
                }
                
                // Возвращено с МЮ РК
                if (CurrentStageContains(request, "TM03.3.6"))
                {
                    // Направлено заявителю заключение об окончательном отказе
                    return s => s.Code.Equals("TM03.3.9.1");
                }

            }

            return null;
        }

        /// <summary>
        /// Логика обработки этапов при исходящем документе "Запрос предварительной экспертизы"
        /// </summary>
        /// <param name="document">Документ</param>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Направлен запрос"</returns>
        private Expression<Func<DicRouteStage, bool>> SentRequestLogic(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(document, "OUT03.1"))
            {
                if (CurrentStageContains(request, "TM03.2.2"))
                {
                    return s => s.Code.Equals("TM03.3.7.1");
                }
            }

            return null;
        }

        /// <summary>
        /// Логика обработки этапов при исходящем документе "Уведомление о прекращении делопроизводства (не предоставление материалов)"
        /// </summary>
        /// <param name="document">Документ</param>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Делопроизводство прекращено"</returns>
        private Expression<Func<DicRouteStage, bool>> RequestlessWorkStopLogic(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(document, "OUT03.1"))
            {
                if (CurrentStageContains(request, "TM03.3.7.0") && FromStageContains(request, "TM03.3.7.1"))
                {
                    return s => s.Code.Equals("TM03.3.9");
                }
            }

            return null;
        }

        /// <summary>
        /// Логика обработки этапов при исходящем документе "Уведомление о принятии к рассмотрению заявки на регистрацию ТЗ_знака обслуживания"
        /// </summary>
        /// <param name="document">Документ</param>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Готовые для передачи на полную экспертизу"</returns>
        private Expression<Func<DicRouteStage, bool>> ReadyToFullExaminationForPred1Logic(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(document, "OUT03.1"))
            {
                if (CurrentStageContains(request, "TM03.2.2") && HasPaidInvoices(request, DicTariff.Codes.TmRequestExamination, DicTariff.Codes.TmRequestExaminationWithThreeOrMoreClass))
                {
                    return s => s.Code.Equals("TM03.2.2.1");
                }
            }

            return null;
        }

        private Expression<Func<DicRouteStage, bool>> GetStagePredicate(RequestDocument rd)
        {
            return _logicMap.ContainsKey(rd.Document.Type.Code)
                ? _logicMap[rd.Document.Type.Code].Invoke(rd.Document, rd.Request)
                : null;
        }

        /// <summary>
        /// Логика обработки этапов при смене статуса исходящего документа "Экспертное заключение о регистрации ТЗ"
        /// </summary>
        /// <param name="document">Документ</param>
        /// <param name="request">Заявка</param>
        /// <returns>Этапы "Окончательное экспертное заключение" или "Передано в МЮ РК"</returns>
        private Expression<Func<DicRouteStage, bool>> RegistrationConclusionLogic(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(document, "OUT02.1"))
            {
                if (CurrentStageContains(request, "TM03.3.2.0", "TM03.3.2.0.0"))
                    return s => s.Code.Equals("TM03.3.3.0");

                if (CurrentStageContains(request, "TM03.3.2"))
                    return s => s.Code.Equals("TM03.3.3");
            }

            if (CurrentStageContains(document, "OUT03.1") && CurrentStageContains(request, "TM03.3.4"))
                return s => s.Code.Equals("TM03.3.5");
            
            return null;
        }

        /// <summary>
        /// Логика обработки этапов при смене статуса исходящих документов "КСПЕРТНОЕ ЗАКЛЮЧЕНИЕ (окончательное) о регистрации ТЗ (знака обслуживания)"
        /// и "ЭКСПЕРТНОЕ ЗАКЛЮЧЕНИЕ (окончательное) о регистрации ТЗ  с согласием заявителя"
        /// И "ЭКСПЕРТНОЕ ЗАКЛЮЧЕНИЕ (окончательное) о регистрации ТЗ  без согласия заявителя"
        /// </summary>
        /// <param name="document">Документ</param>
        /// <param name="request">Заявка</param>
        /// <returns>Этапы "Окончательное экспертное заключение" или "Передано в МЮ РК"</returns>
        private Expression<Func<DicRouteStage, bool>> FinalRegistrationConclusionLogic(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(document, "OUT02.1") &&
                CurrentStageContains(request, "TM03.3.2.0", "TM03.3.2.0.0"))
                return s => s.Code.Equals("TM03.3.3.0");

            if (CurrentStageContains(document, "OUT03.1") && CurrentStageContains(request, "TM03.3.4"))
                return s => s.Code.Equals("TM03.3.5");
            
            return null;
        }

        /// <summary>
        /// Логика обработки этапов при смене статуса исходящего документа "Экспертное заключение об отказе в регистрации ТЗ (знака обслуживания)"
        /// и "ЭКСПЕРТНОЕ ЗАКЛЮЧЕНИЕ о регистрации ТЗ (знака обслуживания) с дискламацией_частичным отказом"
        /// </summary>
        /// <param name="document">Документ</param>
        /// <param name="request">Заявка</param>
        /// <returns>Этапы "Окончательное экспертное заключение" или "Передано в МЮ РК"</returns>
        private Expression<Func<DicRouteStage, bool>> RefusalConclusionLogic(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(document, "OUT03.1"))
            {
                if (CurrentStageContains(request, "TM03.3.4"))
                {
                    if (document.Type.Code.Equals(DicDocumentType.Codes.ExpertTmRegistrationOpinionWithDisclaimer))
                        return s => s.Code.Equals("TM03.3.4.4");

                    if (document.Type.Code.Equals(DicDocumentType.Codes.ExpertTmRegisterRefusalOpinion))
                        return s => s.Code.Equals("TM03.3.4.4.0");
                }
            }

            if (CurrentStageContains(document, "OUT02.1") && CurrentStageContains(request, "TM03.3.2"))
            {
                return s => s.Code.Equals("TM03.3.3");
            }
            
            return null;
        }
    }
}
