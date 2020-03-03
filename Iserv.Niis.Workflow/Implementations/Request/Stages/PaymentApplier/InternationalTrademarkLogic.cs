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
    public class InternationalTrademarkLogic: BaseLogic
    {
        private readonly Dictionary<string, Func<Domain.Entities.Request.Request, Expression<Func<DicRouteStage, bool>>>> _logicMap;
        public InternationalTrademarkLogic(IWorkflowApplier<Domain.Entities.Request.Request> workflowApplier, NiisWebContext context) : base(workflowApplier, context)
        {
            _logicMap = new Dictionary<string, Func<Domain.Entities.Request.Request, Expression<Func<DicRouteStage, bool>>>>
            {
                {DicTariff.Codes.ExpertiseConclusionObjectionTermExtensionMonthly,ConclusionPublishingTermExtensionLogic},
                {DicTariff.Codes.RequestAnswerTimeExtensionForMonth, RequestTermExtensionLogic}
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
        /// Логика отправки заявки на международкый товарный знак на этап Продление срока с этапа
        /// Публикация заключения об отказе в ВОИС при получении оплаты и добавлении документа.
        /// </summary>
        /// <param name="request">Заявка</param>
        /// <returns></returns>
        private Expression<Func<DicRouteStage, bool>> ConclusionPublishingTermExtensionLogic(Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(request, "TMI03.3.4.1.0") &&
                 AnyDocuments(request, DicDocumentType.Codes.PetitionForExtendTimeRorResponse))
            {
                return s => s.Code.Equals("TMI03.3.4.5.1");
            }

	        if (CurrentStageContains(request, "TMI03.3.4.1.0") &&
	            AnyDocuments(request, DicDocumentType.Codes.PetitionForExtendTimeRorObjections)) {
		        return s => s.Code.Equals("TMI03.3.4.5.1");
	        }

			return null;
        }

        /// <summary>
        /// Логика отправки заявки на международный товарный знак на этап Продление срока с этапа
        /// Направлен запрос при получении оплаты и добавлении документа.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private Expression<Func<DicRouteStage, bool>> RequestTermExtensionLogic(Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(request, "TMI03.3.2.0") &&
                 AnyDocuments(request, DicDocumentType.Codes.PetitionForExtendTimeRorResponse))
            {
                return s => s.Code.Equals("TMI03.3.4.5.1");
            }

            return null;
        }
    }
}
