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
    public class InventionLogic : BaseLogic
    {
        private readonly Dictionary<string, Func<Domain.Entities.Document.Document, Domain.Entities.Request.Request, Expression<Func<DicRouteStage, bool>>>> _logicMap;

        public InventionLogic(
            NiisWebContext context,
            IWorkflowApplier<Domain.Entities.Request.Request> workflowApplier)
            : base(workflowApplier, context)
        {
            _logicMap = new Dictionary<string, Func<Domain.Entities.Document.Document, Domain.Entities.Request.Request, Expression<Func<DicRouteStage, bool>>>>
            {
                { DicDocumentType.Codes.NotificationForPaymentlessInventionRequest, PaymentlessInventionRequestLogic },
                { DicDocumentType.Codes.NotificationForInventionPaymentForIndividuals, ExpectPaymentLogic },
                { DicDocumentType.Codes.NotificationForInventionPaymentForBeneficiaries, ExpectPaymentLogic },
                { DicDocumentType.Codes.NotificationForPozitiveFormalExamination, ReadyToMeritsExaminationLogic },
                { DicDocumentType.Codes.NotificationOfRevocationOfPatentApplication, RestoreLogic },
                { DicDocumentType.Codes.NotificationForPaymentlessPozitiveFormalExamination, PaymentWaitLogic },
                { DicDocumentType.Codes.NotificationForSubstantiveExamination, PaymentWaitLogic },
                { DicDocumentType.Codes.RequestForFormalExamForInvention, ExaminationRequestLogic },
                { DicDocumentType.Codes.NotificationOfAnswerlessPatentRequestFinalRecall, RecallLogic },
                { DicDocumentType.Codes.NotificationOfPaymentlessExaminationFinalRecall, RecallLogic },
                { DicDocumentType.Codes.NotificationOfRevocationOfPaymentlessSubstantiveExamination, RecallLogic },
                { DicDocumentType.Codes.ReportOfSearch, SearchLogic },
                { DicDocumentType.Codes.ConclusionOfInventionPatentGrant, PatentGrantLogic },
                { DicDocumentType.Codes.ConclusionOfInventionPatentGrantRefuse, PatentRefuseLogic },
                { DicDocumentType.Codes.RequestForSubstantiveExamination, SubstantiveExaminationRequestLogic },
                { DicDocumentType.Codes.RequestForExaminationOfInventionPatentRequest, SubstantiveExaminationRequestLogic },
                { DicDocumentType.Codes.NotificationOfDecisionPatentGrant, PreparationToGosRegisterTransferLogic },
                { DicDocumentType.Codes.FreeFormNotification, FreeFormNotificationLogic },
            };
        }

        public override async Task ApplyAsync(RequestDocument requestDocument)
        {
            await ApplyStageAsync(GetStagePredicate(requestDocument), requestDocument.Request);
        }

        /// <summary>
        /// Логика обработки этапов при входящем документе "33_Уведомление о принятии решения о выдаче патента (УВ-Кб)"
        /// </summary>
        /// <param name="document">Документ</param>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Подготовка для передачи в Госреестр"</returns>
        private Expression<Func<DicRouteStage, bool>> PreparationToGosRegisterTransferLogic(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(request, "B03.3.5") && CurrentStageContains(document, "OUT03.1"))
            {
                return s => s.Code.Equals("B03.3.7.1");
            }

            if (CurrentStageContains(request, "B03.3.4.1.0") && CurrentStageContains(document, "OUT03.1"))
            {
                return s => s.Code.Equals("B03.3.7.1");
            }

            return null;
        }

        private Expression<Func<DicRouteStage, bool>> FreeFormNotificationLogic(Domain.Entities.Document.Document document,
            Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(request, "B03.3.4.1.0") && CurrentStageContains(document, "OUT03.1"))
            {
                return s => s.Code.Equals("B03.3.9");
            }

            return null;
        }

        /// <summary>
        /// Логика обработки этапов при исходящем документе "Запрос экспертизы по существу (ИЗ-2а_kz)"
        /// или "30_Запрос экспертизы заявки на выдачу патента на изобретение (Форма ИЗ-2б)"
        /// </summary>
        /// <param name="document">Документ</param>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Запрос экспертизы"</returns>
        private Expression<Func<DicRouteStage, bool>> SubstantiveExaminationRequestLogic(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(document, "OUT03.1"))
            {
                if (CurrentStageContains(request, "B03.2.4"))
                {
                    return s => s.Code.Equals("B03.3.1.1");
                }
            }

            return null;
        }

        /// <summary>
        /// Логика обработки этапов при исходящем документе "24_Заключение о выдаче патента на изобретение  (Форма ИЗ-3б)"
        /// </summary>
        /// <param name="document">Документ</param>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Вынесено экспертное заключение" или "Передано в МЮ РК"</returns>
        private Expression<Func<DicRouteStage, bool>> PatentGrantLogic(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(document, "OUT02.1") && CurrentStageContains(request, "B03.2.4"))
            {
                return s => s.Code.Equals("B03.3.2");
            }

            if (CurrentStageContains(document, "OUT02.3") && CurrentStageContains(request, "B03.3.2"))
            {
                return s => s.Code.Equals("B03.3.2.1");
            }

            if (CurrentStageContains(document, "OUT03.1") && CurrentStageContains(request, "B03.3.3"))
            {
                return s => s.Code.Equals("B03.3.4");
            }

            return null;
        }

        /// <summary>
        /// Логика обработки этапов при исходящем документе "27_Заключение об отказе в выдаче патента на изобретение (ОТРИЦАТЕЛЬНОЕ) (Форма ИЗ-4п)"
        /// </summary>
        /// <param name="document">Документ</param>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Вынесено экспертное заключение" или "Отказано в выдаче охранного документа"</returns>
        private Expression<Func<DicRouteStage, bool>> PatentRefuseLogic(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(document, "OUT02.1") && CurrentStageContains(request, "B03.2.4"))
            {
                return s => s.Code.Equals("B03.3.2");
            }

            if (CurrentStageContains(document, "OUT02.3") && CurrentStageContains(request, "B03.3.2"))
            {
                return s => s.Code.Equals("B03.3.2.1");
            }

            if (CurrentStageContains(document, "OUT03.1") && CurrentStageContains(request, "B03.3.3"))
            {
                return s => s.Code.Equals("B03.3.4.1");
            }

            return null;
        }

        /// <summary>
        /// Логика обработки этапов при исходящем документе " Отчет о поиске ИЗ"
        /// </summary>
        /// <param name="document">Документ</param>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Экспертиза заявки на выдачу патента"</returns>
        private Expression<Func<DicRouteStage, bool>> SearchLogic(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(document, "OUT03.1") && CurrentStageContains(request, "B03.2.2.0"))
            {
                return s => s.Code.Equals("B03.2.3.0");
            }

            if (CurrentStageContains(document, "OUT03.1") && CurrentStageContains(request, "B03.2.3.0"))
            {
                return s => s.Code.Equals("B03.2.4");
            }

            return null;
        }

        /// <summary>
        /// Логика обработки этапов при исходящем документе "44_Уведомление об окончательном отзыве  заявки на патент из-за непредоставления ответа на запрос (УВ-2(о))"
        /// или "Уведомление об окончательном отзыве в связи с неоплатой экспертизы по существу (УВ-2 эс(о))"
        /// или "41_Уведомление об отзыве в связи с неоплатой экспертизы по существу (УВ-2 эс)"
        /// </summary>
        /// <param name="document">Документ</param>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Отозванные"</returns>
        private Expression<Func<DicRouteStage, bool>> RecallLogic(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(document, "OUT03.1") && CurrentStageContains(request, "X01"))
            {
                if (FromStageContains(request, "B03.3.1.1.1"))
                {
                    return s => s.Code.Equals("B04.0");
                }

                if (FromStageContains(request, "B02.2.0"))
                {
                    return s => s.Code.Equals("B03.3.1.1.1");
                }
            }

            return null;
        }

        /// <summary>
        /// Логика обработки этапов при исходящем документе "4.7_1_Уведомление о положительном результате ФЭ (без оплаты за экспертизу по сущ.)"
        /// и "Уведомление ПО(экспертиза по существу)"
        /// </summary>
        /// <param name="document">Документ</param>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Ожидает оплату за экспертизу по существу"</returns>
        private Expression<Func<DicRouteStage, bool>> PaymentWaitLogic(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(document, "OUT03.1") && CurrentStageContains(request, "B03.2.1") 
                && !HasPaidInvoices(request,
                    DicTariff.Codes.ExaminationOfApplicationForInventionMerits,
                    DicTariff.Codes.AcceleratedExaminationOfApplicationForInventionMerits,
                    DicTariff.Codes.ExaminationOfApplicationForInventionMeritsWithInternationalReport,
                    DicTariff.Codes.ExaminationOfApplicationForInventionMeritsWithSearchReport))
            {
                return s => s.Code.Equals("B02.2.0");
            }

            return null;
        }

        /// <summary>
        /// Логика обработки этапов при исходящем документе "38_Уведомление об  отзыве заявки на патент из-за непредоставления ответа на запрос (УВ-2п(З))"
        /// </summary>
        /// <param name="document">Документ</param>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Восстановление срока"</returns>
        private Expression<Func<DicRouteStage, bool>> RestoreLogic(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(document, "OUT03.1") && CurrentStageContains(request, "X01"))
            {
                if (FromStageContains(request, "B03.3.1.1"))
                {
                    return s => s.Code.Equals("B03.3.1.1.1");
                }
            }

            return null;
        }

        /// <summary>
        /// Логика обработки этапов при исходящем документе "4.7_Уведомление о положительном результате ФЭ"
        /// </summary>
        /// <param name="document">Документ</param>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Готовые для передачи на экспертизу по существу"</returns>
        private Expression<Func<DicRouteStage, bool>> ReadyToMeritsExaminationLogic(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(document, "OUT03.1"))
            {
                if (CurrentStageContains(request, "B03.2.1"))
                {
                    if (HasPaidInvoices(request, DicTariff.Codes.ExaminationOfApplicationForInventionMerits)
                        || HasPaidInvoices(request, DicTariff.Codes.AcceleratedExaminationOfApplicationForInventionMerits)
                        || HasPaidInvoices(request, DicTariff.Codes.ExaminationOfApplicationForInventionMeritsWithInternationalReport)
                        || HasPaidInvoices(request, DicTariff.Codes.ExaminationOfApplicationForInventionMeritsWithSearchReport))
                    {
                        return s => s.Code.Equals("B03.2.1.1");
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Логика обработки этапов при исходящем документе "4.17_Запрос формальной экспертизы на изобретение"
        /// </summary>
        /// <param name="document">Документ</param>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Запрос экспертизы"</returns>
        private Expression<Func<DicRouteStage, bool>> ExaminationRequestLogic(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(document, "OUT03.1"))
            {
                if (CurrentStageContains(request, "B03.3.1.1"))
                {
                    return s => s.Code.Equals("B03.2.1");
                }

                if (CurrentStageContains(request, "B03.2.1"))
                {
                    return s => s.Code.Equals("B03.3.1.1");
                }
            }

            return null;
        }

    /// <summary>
    /// Логика обработки этапов при исходящем документе "4.18_Уведомление на оплату для физических лиц _ИЗ" 
    /// и "4.19_Уведомление на оплату для физических лиц, имеющих льготу_ИЗ"
    /// </summary>
    /// <param name="document">Документ</param>
    /// <param name="request">Заявка</param>
    /// <returns>Запрос для получения этапа "Ожидает оплату за подачу"</returns>
    private Expression<Func<DicRouteStage, bool>> ExpectPaymentLogic(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(document, "OUT03.1"))
            {
                if (CurrentStageContains(request, "B02.2"))
                {
                    return s => s.Code.Equals("B02.2.0.0");
                }
            }

            return null;
        }

        /// <summary>
        /// Логика обработки этапов при исходящем документе "4.20_Уведомление на изобретение (неподан. в связи с отсутствием оплаты за прием заявки)_ИЗ"
        /// </summary>
        /// <param name="document">Документ</param>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Признаны неподанными или ошибочно зарегистрированные"</returns>
        private Expression<Func<DicRouteStage, bool>> PaymentlessInventionRequestLogic(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(document, "OUT03.1"))
            {
                if (CurrentStageContains(request, "X01") && FromStageContains(request, "B02.2.0.0"))
                {
                    return s => s.Code.Equals("B04.0.0.1");
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
    }
}
