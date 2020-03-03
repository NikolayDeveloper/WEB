using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Workflow.Abstract;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Iserv.Niis.Workflow.Implementations.Contract.Stages.GeneratedNumberApplier
{
    public class StageLogic
    {
        private readonly NiisWebContext _context;

        private readonly
            Dictionary<string, Func<Domain.Entities.Contract.Contract, Expression<Func<DicRouteStage, bool>>>> _logicMap
            ;

        private readonly IWorkflowApplier<Domain.Entities.Contract.Contract> _workflowApplier;

        public StageLogic(IWorkflowApplier<Domain.Entities.Contract.Contract> workflowApplier,
            NiisWebContext context)
        {
            _workflowApplier = workflowApplier;
            _context = context;
            _logicMap =
                new Dictionary<string, Func<Domain.Entities.Contract.Contract, Expression<Func<DicRouteStage, bool>>>>
                {
                    {DicDocumentType.Codes.PaymentInvoiceOfDk, PayDocumentLogic},
                    {DicDocumentType.Codes.NotificationOfPaymentOfStateDuty, PayDocumentLogic},
                    {DicDocumentType.Codes.RequestForCustomerOfDk, RequestAnswerWaitingLogic},
                    {DicDocumentType.Codes.ExpertConclusionsRegisterOfContract, MJDesicionWaitLogic},
                    {DicDocumentType.Codes.CoveringLetterOfDk, Logic},
                };
        }

        public async Task ApplyAsync(ContractDocument contractDocument)
        {
            await ApplyStageAsync(
                _logicMap.TryGetValue(contractDocument.Document.Type.Code, out var logic)
                    ? logic.Invoke(contractDocument.Contract)
                    : null, contractDocument.Contract);
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


        /// <summary>
        /// Логика при исходящем документе "сопроводительное письмо ДК"
        /// </summary>
        /// <param name="contract">Договор</param>
        /// <returns>Запрос для получения этапа "Ожидается решение МЮ"</returns>
        private Expression<Func<DicRouteStage, bool>> Logic(
            Domain.Entities.Contract.Contract contract)
        {
            //Уведомление заявителя о решении
            if (CurrentStageContains(contract, "DK02.9.2"))
            {
                // Зарегистрировано
                if (AnyDocuments(contract, DicDocumentType.Codes.ConclusionAboutRegistrationOfContract))
                    return s => s.Code.Equals("DK02.9.1");
                // Отказано в регистрации
                if (AnyDocuments(contract, DicDocumentType.Codes.ConclusionAboutRegistrationRefusalOfContract))
                    return s => s.Code.Equals("DK02.9.3");
            }
                

            return null;
        }

        /// <summary>
        /// Логика при исходящем документе "Реестры эксп.заключений в МЮ РК (Договора)"
        /// </summary>
        /// <param name="contract">Договор</param>
        /// <returns>Запрос для получения этапа "Ожидается решение МЮ"</returns>
        private Expression<Func<DicRouteStage, bool>> MJDesicionWaitLogic(
            Domain.Entities.Contract.Contract contract)
        {
            //Подготовка для передачи в МЮ
            if (CurrentStageContains(contract, "DK02.7"))
                return s => s.Code.Equals("DK02.8");

            return null;
        }

        /// <summary>
        ///     "101_Запрос заявителю"
        /// </summary>
        /// <param name="contract">Договор</param>
        /// <returns>Запрос для получения этапа "Ожидается ответ на запрос"</returns>
        private Expression<Func<DicRouteStage, bool>> RequestAnswerWaitingLogic(
            Domain.Entities.Contract.Contract contract)
        {
            //Экспертиза по существу
            if (CurrentStageContains(contract, "DK02.4"))
                return s => s.Code.Equals("DK02.4.2");

            return null;
        }

        /// <summary>
        ///     "ДК- Счет на оплату" или "Уведомление об оплате гос.пошлины (УВП-У5)"
        /// </summary>
        /// <param name="contract">Договор</param>
        /// <returns>Запрос для получения этапа "Предварительная экспертиза ДК"</returns>
        private Expression<Func<DicRouteStage, bool>> PayDocumentLogic(
            Domain.Entities.Contract.Contract contract)
        {
            if (CurrentStageContains(contract, "DK02.1.0"))
                return s => s.Code.Equals("DK02.4.0");

            return null;
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

        private bool AnyDocuments(Domain.Entities.Contract.Contract contract, params string[] documentTypeCodes)
        {
            return contract.Documents.Select(rd => rd.Document).Any(d => documentTypeCodes.Contains(d.Type.Code));
        }

        private bool HasPaidInvoices(Domain.Entities.Contract.Contract contract, params string[] tariffCodes)
        {
            var restorationInvoices = contract.PaymentInvoices
                .Where(pi => tariffCodes.Contains(pi.Tariff.Code)).ToList();
            return restorationInvoices.Any() && restorationInvoices.All(pi => pi.Status.Code != "notpaid");
        }
    }
}