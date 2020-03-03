using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Workflow.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Workflow.Implementations.Request.Stages.PaymentApplier
{
    public class TrademarkLogic : BaseLogic
    {
        private readonly Dictionary<string, Func<Domain.Entities.Request.Request, Expression<Func<DicRouteStage, bool>>>> _logicMap;
        private readonly NiisWebContext _context;

        public TrademarkLogic(
            NiisWebContext context,
            IWorkflowApplier<Domain.Entities.Request.Request> workflowApplier) 
            : base(workflowApplier, context)
        {
            _context = context;
	        _logicMap = new Dictionary<string, Func<Domain.Entities.Request.Request, Expression<Func<DicRouteStage, bool>>>>
	        {
	            { DicTariff.Codes.TmFormalExaminationOnPurpose, ReadyToPreExaminationLogic },
	            { DicTariff.Codes.TmFormalExaminationEmail, ReadyToPreExaminationLogic },
	            { DicTariff.Codes.RequestAnswerTimeExtensionForMonth, ProlongationLogic },
	            { DicTariff.Codes.TmRequestExamination, ReadyToFullExaminationLogic },
	            { DicTariff.Codes.TmRequestExaminationWithThreeOrMoreClass, ReadyToFullExaminationLogic },
	            { DicTariff.Codes.RequestAnswerMissedTimeRestoration, RestorationPaymentApplyLogic },
	            { DicTariff.Codes.TmNmptRegistration, ReadyToGosRegisterTransferLogic }
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
        /// Логика обработки этапов при применении оплаты на услугу "регистрация товарных знаков, знаков обслуживания и наименования мест происхождения товаров и публикация сведений о регистрации"
        /// </summary>
        /// <param name="request">Заявка</param>
        /// <returns>Этап "Готовые для передачи в Госреестр"</returns>
        private Expression<Func<DicRouteStage, bool>> ReadyToGosRegisterTransferLogic(Domain.Entities.Request.Request request)
        {
            // Подготовка для передачи в Госреестр
            if (request.CurrentWorkflow.CurrentStage.Code.Equals("TM03.3.7"))
            {
                // Готовые для передачи в Госреестр
                return s => s.Code.Equals("TM03.3.8");
            }

            // Ожидание восстановления пропущенного срока
            if (request.CurrentWorkflow.CurrentStage.Code.Equals("TM03.3.7.0") && HasPaidInvoices(request, DicTariff.Codes.RequestAnswerMissedTimeRestoration) &&
                AnyDocuments(request, DicDocumentType.Codes.PetitionForRestoreTime))
            {
                // Готовые для передачи в Госреестр
                return s => s.Code.Equals("TM03.3.8");
            }

            return null;
        }

        /// <summary>
        /// Логика обработки этапов при применении оплаты на услугу "Прием и проведение формальной экспертизы заявок по товарным знакам, знакам обслуживания"
        /// </summary>
        /// <param name="request">Заявка</param>
        /// <returns>Этап "Готовые на передачу на предварительную экспертизу"</returns>
        private Expression<Func<DicRouteStage, bool>> ReadyToPreExaminationLogic(Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(request, "TM02.2", "TM02.2.2"))
            {
                return s => s.Code.Equals("TM02.2.0");
            }

            return null;
        }

        /// <summary>
        /// Логика обработки этапов при применении оплаты на услугу "Продление срока ответа на запрос за каждый месяц"
        /// </summary>
        /// <param name="request">Заявка</param>
        /// <returns>Этап "Продление срока"</returns>
        private Expression<Func<DicRouteStage, bool>> ProlongationLogic(Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(request, "TM03.3.7.0", "TM03.3.4.4", "TM03.3.4.4.0") &&
                AnyDocuments(request, DicDocumentType.Codes.PetitionForExtendTimeRorResponse))
            {
                return s => s.Code.Equals("TM03.3.7.3");
            }

            var invoiceDate = _context.PaymentInvoices.Include(pi => pi.Tariff)
                .LastOrDefault(pi =>
                    pi.RequestId == request.Id &&
                    pi.Tariff.Code.Contains(DicTariff.Codes.RequestAnswerTimeExtensionForMonth) &&
                    pi.Status.Code != "notpaid")?.DateCreate;
            if (invoiceDate == null)
            {
                return null;
            }

            var stageDate = _context.RequestWorkflows.Include(rw => rw.CurrentStage)
                .LastOrDefault(rw => rw.CurrentStage.Code.Equals("TM03.3.7.1") && rw.OwnerId == request.Id)
                ?.DateCreate;
            if (stageDate == null)
            {
                return null;
            }

            if (CurrentStageContains(request, "TM03.3.7.1") &&
                HasPaidInvoices(request, DicTariff.Codes.RequestAnswerTimeExtensionForMonth) &&
                invoiceDate.Value > stageDate.Value)
            {
                return s => s.Code.Equals("TM03.3.7.3");
            }

            return null;
        }
        
        /// <summary>
        /// Логика обработки этапов при применении оплаты на услуги "Проведение экспертизы заявки на регистрацию товарных знаков, знаков обслуживания" и
        /// "проведение экспертизы заявки  на регистрацию товарных знаков, знаков обслуживания дополнительно за каждый класс свыше трех"
        /// </summary>
        /// <param name="request">Заявка</param>
        /// <returns>Этап "Готовые для передачи на полную экспертизу"</returns>
        private Expression<Func<DicRouteStage, bool>> ReadyToFullExaminationLogic(Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(request, "TM03.2.2") && AnyDocuments(request, DicDocumentType.Codes.NotificationOfTrademarkRequestRegistation))
            {
                return s => s.Code.Equals("TM03.2.2.1");
            }

            if (CurrentStageContains(request, "TM03.2.2.0"))
            {
                return s => s.Code.Equals("TM03.2.2.1");
            }

            if (CurrentStageContains(request, "TM03.3.7.0") && AnyDocuments(request, DicDocumentType.Codes.PetitionForRestoreTime) &&
                HasPaidInvoices(request, DicTariff.Codes.RequestAnswerMissedTimeRestoration))
            {
                return s => s.Code.Equals("TM03.2.2.1");
            }

            return null;
        }
        
        /// <summary>
        /// Логика обработки этапов при применении оплаты на услугу "Восстановление пропущенного срока ответа на запрос, оплаты, подачи возражения заявителем (п.3 ст.15 Закона)"
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Этап "Готовые для передачи на полную экспертизу" или "Предварительная экспертиза" или "Полная экспертиза" или "Готовые для передачи в Госреестр"</returns>
        private Expression<Func<DicRouteStage, bool>> RestorationPaymentApplyLogic(Domain.Entities.Request.Request request)
        {
            if (!AnyDocuments(request, DicDocumentType.Codes.PetitionForRestoreTime)) return null;

            if (FromStageContains(request, "TM03.2.1") && CurrentStageContains(request, "TM03.3.7.0") && 
                AnyDocuments(request, DicDocumentType.Codes.AnswerToRequest))
            {
                return s => s.Code.Equals("TM03.2.2");
            }

            if (FromStageContains(request, "TM03.2.2") && CurrentStageContains(request, "TM03.3.7.0") &&
                HasPaidInvoices(request, DicTariff.Codes.TmRequestExamination, DicTariff.Codes.TmRequestExaminationWithThreeOrMoreClass))
            {
                return s => s.Code.Equals("TM03.2.2.1");
            }

            if (FromStageContains(request, "TM03.3.1") && CurrentStageContains(request, "TM03.3.7.0") &&
                AnyDocuments(request, DicDocumentType.Codes.AnswerToRequest))
            {
                return s => s.Code.Equals("TM03.3.2");
            }

            if (CurrentStageContains(request, "TM03.3.7.0") && AnyDocuments(request, DicDocumentType.Codes.Objection))
            {
                return s => s.Code.Equals("TM03.3.2");
            }

            // Ожидание восстановления пропущенного срока
            if (CurrentStageContains(request, "TM03.3.7.0") && HasPaidInvoices(request, DicTariff.Codes.TmNmptRegistration))
            {
                // Готовые для передачи в Госреестр
                return s => s.Code.Equals("TM03.3.8");
            }

            return null;
        }
    }
}