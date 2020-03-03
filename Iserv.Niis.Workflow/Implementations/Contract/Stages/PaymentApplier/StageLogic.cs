using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Workflow.Abstract;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Iserv.Niis.Workflow.Implementations.Contract.Stages.PaymentApplier
{
    public class StageLogic
    {
        private readonly NiisWebContext _context;

        private readonly
            Dictionary<string, Func<Domain.Entities.Contract.Contract, Expression<Func<DicRouteStage, bool>>>> _logicMap
            ;

        private readonly IWorkflowApplier<Domain.Entities.Contract.Contract> _workflowApplier;

        public StageLogic(
            NiisWebContext context,
            IWorkflowApplier<Domain.Entities.Contract.Contract> workflowApplier)
        {
            _context = context;
            _workflowApplier = workflowApplier;
            _logicMap =
                new Dictionary<string, Func<Domain.Entities.Contract.Contract, Expression<Func<DicRouteStage, bool>>>>
                {
                    {DicTariff.Codes.NEW_034, ContractPaymentUseLogic},
                    {DicTariff.Codes.NEW_035, ContractPaymentUseLogic},
                    {DicTariff.Codes.NEW_036, ContractPaymentUseLogic},
                    {DicTariff.Codes.NEW_037, ContractPaymentUseLogic},
                    {DicTariff.Codes.NEW_064, ContractPaymentUseLogic},
                    {DicTariff.Codes.NEW_065, ContractPaymentUseLogic},
                    {DicTariff.Codes.NEW_066, ContractPaymentUseLogic},
                    {DicTariff.Codes.NEW_067, ContractPaymentUseLogic},
                    {DicTariff.Codes.NEW_102, ContractPaymentUseLogic},
                    {DicTariff.Codes.NEW_103, ContractPaymentUseLogic},
                    {DicTariff.Codes.NEW_104, ContractPaymentUseLogic}
                };
        }

        public async Task ApplyAsync(PaymentUse paymentUse)
        {
            await ApplyStageAsync(
                _logicMap.TryGetValue(paymentUse.PaymentInvoice.Tariff.Code, out var logic)
                    ? logic.Invoke(paymentUse.PaymentInvoice.Contract)
                    : null, paymentUse.PaymentInvoice.Contract);
        }

        /// <summary>
        /// Услуги договоров
        /// </summary>
        /// <param name="contract">Договор</param>
        /// <returns>Запрос для получения этапа "Предварительная экспертиза ДК"</returns>
        private Expression<Func<DicRouteStage, bool>> ContractPaymentUseLogic(
            Domain.Entities.Contract.Contract contract)
        {
            if (CurrentStageContains(contract, "DK02.1.0"))
                return s => s.Code.Equals("DK02.2");

            return null;
        }

        private async Task ApplyStageAsync(Expression<Func<DicRouteStage, bool>> nextStageFunc,
            Domain.Entities.Contract.Contract contract)
        {
            if (nextStageFunc == null) return;

            DicRouteStage nextStage;

            try
            {
                nextStage = await _context.DicRouteStages.SingleAsync(nextStageFunc);
            }
            catch (Exception)
            {
                Log.Warning($"Workflow logic applier. The next stage query is incorrect: {nextStageFunc}!");
                return;
            }

            var workflow = new ContractWorkflow
            {
                RouteId = nextStage.RouteId,
                OwnerId = contract.Id,
                Owner = contract,
                CurrentStageId = nextStage.Id,
                CurrentUserId = contract.CurrentWorkflow.CurrentUserId,
                FromStageId = contract.CurrentWorkflow.CurrentStageId,
                FromUserId = contract.CurrentWorkflow.CurrentUserId,
                IsComplete = nextStage.IsLast,
                IsSystem = nextStage.IsSystem,
                IsMain = nextStage.IsMain
            };

            await _workflowApplier.ApplyAsync(workflow);
        }

        private bool FromStageContains(Domain.Entities.Contract.Contract contract, params string[] codes)
        {
            return codes.Contains(contract.CurrentWorkflow.FromStage.Code);
        }

        private bool CurrentStageContains(Domain.Entities.Contract.Contract contract, params string[] codes)
        {
            return codes.Contains(contract.CurrentWorkflow.CurrentStage.Code);
        }

        private bool CurrentStageContains(Domain.Entities.Document.Document document, params string[] codes)
        {
            return codes.Contains(document.CurrentWorkflow.CurrentStage.Code);
        }

        private bool AnyDocuments(Domain.Entities.Contract.Contract contract, string documentTypeCode)
        {
            return contract.Documents.Select(rd => rd.Document).Any(d => d.Type.Code.Equals(documentTypeCode));
        }

        private bool HasPaidInvoices(Domain.Entities.Contract.Contract contract, params string[] tariffCodes)
        {
            var restorationInvoices = _context.PaymentInvoices.Include(pi => pi.Status)
                .Include(pi => pi.Tariff)
                .LastOrDefault(pi =>
                    tariffCodes.Contains(pi.Tariff.Code) && pi.RequestId == contract.Id &&
                    pi.Status.Code != "notpaid");
            return restorationInvoices != null;
        }
    }
}