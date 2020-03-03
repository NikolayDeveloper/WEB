using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Workflow.Abstract;

namespace Iserv.Niis.Workflow.Implementations.Request.Stages.PaymentApplier
{
	public class UsefulModelLogic : BaseLogic
	{
		private readonly Dictionary<string, Func<Domain.Entities.Request.Request, Expression<Func<DicRouteStage, bool>>>> _logicMap;
		private PaymentUse _paymentUse;
		public UsefulModelLogic(IWorkflowApplier<Domain.Entities.Request.Request> workflowApplier, NiisWebContext context) : base(workflowApplier, context)
		{
			_logicMap = new Dictionary<string, Func<Domain.Entities.Request.Request, Expression<Func<DicRouteStage, bool>>>>
			{
				{DicTariff.Codes.InventionExaminationOnUsefulModelOnPaper,InventionExaminationOnUsefulModel},
				{DicTariff.Codes.InventionExaminationOnUsefulModelOnOnline, InventionExaminationOnUsefulModel},
				{DicTariff.Codes.ExtensionAndRestorationTimesForAnswerToExaminationRequest, ExtensionAndRestorationTimesForAnswerToExaminationRequest},
			    {DicTariff.Codes.PreparationOfDocumentsForIssuanceOfPatent, ReadyForIssuanceOfPatent}
			};
		}

		public override async Task ApplyAsync(PaymentUse paymentUse)
		{
			_paymentUse = paymentUse;
			await ApplyStageAsync(
				_logicMap.TryGetValue(paymentUse.PaymentInvoice.Tariff.Code, out var logic)
					? logic.Invoke(paymentUse.PaymentInvoice.Request)
					: null, paymentUse.PaymentInvoice.Request);
		}

		/// <summary>
		/// Логика отправки заявки на этап «Готовые для передачи на экспертизу» с этапа "Ввод оплаты" или с этапа "Ожидаемые оплату за подачу"
		/// </summary>
		/// <param name="request">Заявка</param>
		/// <returns></returns>
		private Expression<Func<DicRouteStage, bool>> InventionExaminationOnUsefulModel(Domain.Entities.Request.Request request)
		{
			if (CurrentStageContains(request, "U02.2", "U02.2.7") && HasPaidInvoices(request, DicTariff.Codes.InventionExaminationOnUsefulModelOnPaper, DicTariff.Codes.InventionExaminationOnUsefulModelOnOnline))
			{
				return s => s.Code.Equals("U02.2.1");
			}
			return null;
		}

        /// <summary>
        /// Логика отправки с "Подготовка для передачи в Госреестр" на этап "Готовые для передачи в Госреестр"
        /// </summary>
        /// <param name="request">Заявка</param>
        /// <returns></returns>
        private Expression<Func<DicRouteStage, bool>> ReadyForIssuanceOfPatent(Domain.Entities.Request.Request request) {
	        if (CurrentStageContains(request, "U03.7.1", "U03.3.8")) {
	            return s => s.Code.Equals("U03.8");
			}
			return null;
	    }

        /// <summary>
        /// Логика отправки заявки с этапа "Запрос экспертизы" на этап "Продление срока"
        /// Логика отправки заявки с этапа "Восстановление срока" на этап "Экспертиза заявки на выдачу патента на ПМ"
        /// </summary>
        /// <param name="request">Заявка</param>
        /// <returns></returns>
        private Expression<Func<DicRouteStage, bool>> ExtensionAndRestorationTimesForAnswerToExaminationRequest(Domain.Entities.Request.Request request)
		{
			if (CurrentStageContains(request, "U03.2.1") && AnyDocuments(request, DicDocumentType.Codes.PetitionForExtendTimeRorResponse))
			{
				return s => s.Code.Equals("U02.2.6");
			}
			if (CurrentStageContains(request, "U02.2.7") && AnyDocuments(request, DicDocumentType.Codes.PetitionForExtendTime))
			{
				ProlongateCurentStage(request, Domain.Enums.ExpirationType.CalendarMonth, 2);
			}
			if (CurrentStageContains(request, "U03.3.1") && AnyDocuments(request, DicDocumentType.Codes.PetitionForRestoreTime) && AnyDocuments(request, DicDocumentType.Codes.AnswerToRequest))
			{
				return s => s.Code.Equals("U03.2");
			}
			if (CurrentStageContains(request, "U02.2.6") && AnyDocuments(request, DicDocumentType.Codes.PetitionForExtendTimeRorResponse) )
			{
				var count = _paymentUse. PaymentInvoice.TariffCount.HasValue ? _paymentUse.PaymentInvoice.TariffCount > 3 ? 3 : _paymentUse.PaymentInvoice.TariffCount.Value : 1;
				var queue = _context.WorkflowTaskQueues.LastOrDefault(a => a.RequestId == request.Id && a.ConditionStage.Code == "U03.2.1");
				if (queue == null) queue = _context.WorkflowTaskQueues.LastOrDefault(a => a.RequestId == request.Id);
				var maxDate = queue.DateCreate.AddMonths(6);//не больше 6 месяцев. Срок этапа «Продление срока» (U02.2.6) = срок завершения этапа «Запрос экспертизы» (U03.2.1) + количество месяцев, указанных в ходатайстве (платеже)
				var date = queue.ResolveDate.AddMonths(count);
				queue.ResolveDate = date <= maxDate ? date : maxDate;
				//_paymentUse.PaymentInvoice.
				//ProlongateCurentStage()
			}

			
			return null;
		}
	}
}
