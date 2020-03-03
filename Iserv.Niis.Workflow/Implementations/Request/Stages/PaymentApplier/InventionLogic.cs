using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Workflow.Abstract;

namespace Iserv.Niis.Workflow.Implementations.Request.Stages.PaymentApplier
{
    public class InventionLogic : BaseLogic
    {
        private readonly Dictionary<string, Func<Domain.Entities.Request.Request, Expression<Func<DicRouteStage, bool>>>> _logicMap;

        public InventionLogic(
            NiisWebContext context,
            IWorkflowApplier<Domain.Entities.Request.Request> workflowApplier) 
            : base(workflowApplier, context)
	    {
	        _logicMap = new Dictionary<string, Func<Domain.Entities.Request.Request, Expression<Func<DicRouteStage, bool>>>>
	        {
                { DicTariff.Codes.InventionFormalExaminationOnPurpose, FormalExaminationPayedLogic },
                { DicTariff.Codes.InventionFormalExaminationEmail, FormalExaminationPayedLogic },
                { DicTariff.Codes.InventionAcceleratedFormalExaminationOnPurpose, FormalExaminationPayedLogic },
                { DicTariff.Codes.InventionAcceleratedFormalExaminationEmail, FormalExaminationPayedLogic },
                { DicTariff.Codes.ExtensionAndRestorationTimesForAnswerToExaminationRequest, ExtensionAndRestorationTimesLogic },
                { DicTariff.Codes.ExaminationOfApplicationForInventionMerits, ReadyToMeritsExaminationLogic },
                { DicTariff.Codes.AcceleratedExaminationOfApplicationForInventionMerits, ReadyToMeritsExaminationLogic },
                { DicTariff.Codes.ExaminationOfApplicationForInventionMeritsWithInternationalReport, ReadyToMeritsExaminationLogic },
                { DicTariff.Codes.ExaminationOfApplicationForInventionMeritsWithSearchReport, ReadyToMeritsExaminationLogic },
                { DicTariff.Codes.PreparationOfDocumentsForIssuanceOfPatent, Logic }
            };
        }

        public override async Task ApplyAsync(PaymentUse paymentUse)
        {
            await ApplyStageAsync(
                _logicMap.TryGetValue(paymentUse.PaymentInvoice.Tariff.Code, out var logic)
                    ? logic.Invoke(paymentUse.PaymentInvoice.Request)
                    : null, paymentUse.PaymentInvoice.Request);
        }


        /// <summary>
        /// Логика обработки этапов при применении оплаты на услуги "Подготовка документов к выдаче охранного документа и удостоверения автора,  публикация сведений о выдаче охранного документа"
        /// </summary>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Готовые для передачи в Госреестр"</returns>
        private Expression<Func<DicRouteStage, bool>> Logic(Domain.Entities.Request.Request request)
        {
            if(CurrentStageContains(request, "B03.3.7.1"))
            {
                return s => s.Code.Equals("B03.3.8");
            }

            if (CurrentStageContains(request, "B03.3.7.4"))
            {
                return s => s.Code.Equals("B03.3.8");
            }

            return null;
        }

        /// <summary>
        /// Логика обработки этапов при применении оплаты на услуги 
        /// </summary>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Готовые для передачи на экспертизу по существу"</returns>
        private Expression<Func<DicRouteStage, bool>> ReadyToMeritsExaminationLogic(Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(request, "B03.2.1") 
                && AnyDocuments(request, DicDocumentType.Codes.NotificationForPozitiveFormalExamination))
            {
                return s => s.Code.Equals("B03.2.1.1");
            }

            if (CurrentStageContains(request, "B02.2.0"))
            {
                return s => s.Code.Equals("B03.2.1.1");
            }

            if (CurrentStageContains(request, "B03.3.1.1.1") && AnyDocuments(request, DicDocumentType.Codes.PetitionForRestoreTime)
                && HasPaidInvoices(request, DicTariff.Codes.ExtensionAndRestorationTimesForAnswerToExaminationRequest))
            {
                return s => s.Code.Equals("B03.2.1.1");
            }

            return null;
        }

        /// <summary>
        /// Логика обработки этапов при применении оплаты на услуги "Продление и восстановление сроков представления ответа на запрос экспертизы и оплаты",
        /// </summary>
        /// <param name="request">Заявка</param>
        /// <returns>Этап "Ожидаем оплату за подачу" или "Формальная экспертиза изобретения"</returns>
        private Expression<Func<DicRouteStage, bool>> ExtensionAndRestorationTimesLogic(Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(request, "B02.2") && AnyDocuments(request, DicDocumentType.Codes.PetitionForExtendTimeRorResponse)
                && (AnyDocuments(request, DicDocumentType.Codes.NotificationForInventionPaymentForIndividuals)
                    || AnyDocuments(request, DicDocumentType.Codes.NotificationForInventionPaymentForBeneficiaries)))
            {
                return s => s.Code.Equals("B02.2.0.0");
            }

            if (FromStageContains(request, "B02.2.1.0.0") && CurrentStageContains(request, "B03.3.1.1")
                && AnyDocuments(request, DicDocumentType.Codes.PetitionForRestoreTime)
                && AnyDocuments(request, DicDocumentType.Codes.AnswerToRequest))
            {
                return s => s.Code.Equals("B03.2.1");
            }

            if (CurrentStageContains(request, "B03.3.1.1.1") 
                && AnyDocuments(request, DicDocumentType.Codes.PetitionForRestoreTime)
                && HasPaidInvoices(request,
                DicTariff.Codes.ExaminationOfApplicationForInventionMerits,
                DicTariff.Codes.AcceleratedExaminationOfApplicationForInventionMerits,
                DicTariff.Codes.ExaminationOfApplicationForInventionMeritsWithInternationalReport,
                DicTariff.Codes.ExaminationOfApplicationForInventionMeritsWithSearchReport))
            {
                return s => s.Code.Equals("B03.2.1.1");
            }

            if (CurrentStageContains(request, "B03.3.1.1") && AnyDocuments(request,
                    DicDocumentType.Codes.PetitionForExtendTimeRorResponse))
            {
                return s => s.Code.Equals("B03.3.1.1.0");
            }

            if (CurrentStageContains(request, "B03.3.1.1.1")
                && AnyDocuments(request, DicDocumentType.Codes.AnswerToRequest)
                && AnyDocuments(request, DicDocumentType.Codes.PetitionForRestoreTime))
            {
                return s => s.Code.Equals("B03.2.4");
            }

            return null;
        }

        /// <summary>
        /// Логика обработки этапов при применении оплаты на услуги "Прием заявок и проведение формальной экспертизы на изобретение (при приеме заявки на бумажном носителе)",
        /// "Прием заявок и проведение формальной экспертизы на изобретение (при электронном приеме заявки)",
        /// "Прием заявок и ускоренное проведение формальной экспертизы на изобретение по перечню установленного уполномоченным органом",
        /// "Прием заявок и ускоренное проведение формальной экспертизы на изобретение по перечню установленного уполномоченным органом"
        /// </summary>
        /// <param name="request">Заявка</param>
        /// <returns>Этап "Готовые на передачу на предварительную экспертизу"</returns>
        private Expression<Func<DicRouteStage, bool>> FormalExaminationPayedLogic(Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(request, "B02.2", "B02.3", "B02.2.0.0"))
            {
                return s => s.Code.Equals("B02.2.1");
            }

            return null;
        }
    }
}