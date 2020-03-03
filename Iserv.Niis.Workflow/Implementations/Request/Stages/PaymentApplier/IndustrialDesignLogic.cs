using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Workflow.Abstract;

namespace Iserv.Niis.Workflow.Implementations.Request.Stages.PaymentApplier {
    public class IndustrialDesignLogic : BaseLogic {
        private readonly Dictionary<string, Func<Domain.Entities.Request.Request, Expression<Func<DicRouteStage, bool>>>> _logicMap;
        public IndustrialDesignLogic(IWorkflowApplier<Domain.Entities.Request.Request> workflowApplier, NiisWebContext context) : base(workflowApplier, context) {
            _logicMap = new Dictionary<string, Func<Domain.Entities.Request.Request, Expression<Func<DicRouteStage, bool>>>> {
                { DicTariff.Codes.IndustrialDesignExaminationEmail, ReadyToExaminationLogic },
                { DicTariff.Codes.IndustrialDesignExaminationOnPurpose, ReadyToExaminationLogic },
            };
        }

        private Expression<Func<DicRouteStage, bool>> ReadyToExaminationLogic(Domain.Entities.Request.Request request) {
            if (CurrentStageContains(request, "PO02.2")) {
                return s => s.Code.Equals("PO03.0");
            }

            return null;
        }

        public override async Task ApplyAsync(PaymentUse paymentUse) {
            await ApplyStageAsync(
                _logicMap.TryGetValue(paymentUse.PaymentInvoice.Tariff.Code, out var logic)
                    ? logic.Invoke(paymentUse.PaymentInvoice.Request)
                    : null, paymentUse.PaymentInvoice.Request);
        }
    }
}
