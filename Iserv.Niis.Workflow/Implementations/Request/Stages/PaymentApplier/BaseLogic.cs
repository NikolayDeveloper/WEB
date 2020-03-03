using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Iserv.Niis.Business.Implementations;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Workflow.Abstract;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Iserv.Niis.Workflow.Implementations.Request.Stages.PaymentApplier
{
    public abstract class BaseLogic
    {
        private readonly IWorkflowApplier<Domain.Entities.Request.Request> _workflowApplier;
        protected readonly NiisWebContext _context;

        protected BaseLogic(
            IWorkflowApplier<Domain.Entities.Request.Request> workflowApplier, 
            NiisWebContext context)
        {
            _workflowApplier = workflowApplier;
            _context = context;
        }

        public abstract Task ApplyAsync(PaymentUse paymentUse);

        protected virtual async Task ApplyStageAsync(Expression<Func<DicRouteStage, bool>> nextStageFunc, Domain.Entities.Request.Request request)
        {
            if (nextStageFunc == null)
            {
                return;
            }

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

            var workflow = new RequestWorkflow
            {
                RouteId = nextStage.RouteId,
                OwnerId = request.Id,
                Owner = request,
                CurrentStageId = nextStage.Id,
                CurrentUserId = request.CurrentWorkflow.CurrentUserId,
                FromStageId = request.CurrentWorkflow.CurrentStageId,
                FromUserId = request.CurrentWorkflow.CurrentUserId,
                IsComplete = nextStage.IsLast,
                IsSystem = nextStage.IsSystem,
                IsMain = nextStage.IsMain
            };

            await _workflowApplier.ApplyAsync(workflow);
        }

        protected static bool FromStageContains(Domain.Entities.Request.Request request, params string[] codes)
        {
            return codes.Contains(request.CurrentWorkflow.FromStage.Code);
        }

        protected static bool CurrentStageContains(Domain.Entities.Request.Request request, params string[] codes)
        {
            return codes.Contains(request.CurrentWorkflow.CurrentStage.Code);
        }

        protected static bool CurrentStageContains(Domain.Entities.Document.Document document, params string[] codes)
        {
            return codes.Contains(document.CurrentWorkflow.CurrentStage.Code);
        }
        
        protected bool AnyDocuments(Domain.Entities.Request.Request request, string documentTypeCode)
        {
            return request.Documents.Select(rd => rd.Document).Any(d => d.Type.Code.Equals(documentTypeCode));
        }

        protected bool HasPaidInvoices(Domain.Entities.Request.Request request, params string[] tariffCodes)
        {
            var restorationInvoices = _context.PaymentInvoices.Include(pi => pi.Status)
                    .Include(pi => pi.Tariff)
                    .LastOrDefault(pi =>
                        tariffCodes.Contains(pi.Tariff.Code) && pi.RequestId == request.Id &&
                        pi.Status.Code != "notpaid");
            return restorationInvoices != null;
		}

		protected void ProlongateCurentStage(Domain.Entities.Request.Request request, ExpirationType type, short count)
		{
			var queue = _context.WorkflowTaskQueues.LastOrDefault(a => a.RequestId == request.Id && a.ConditionStageId == request.CurrentWorkflow.CurrentStageId);
			if (queue != null) {
				//Ожидаемые оплату за подачу срок этапа два месяца плюс можно один раз продлить еще на два
				if (CurrentStageContains(request, "U02.2.7") && (queue.ResolveDate - request.DateCreate).TotalDays > 65) return;
				queue.ResolveDate = new CalendarProvider(_context).GetExecutionDate(queue.ResolveDate, type, count);
			}
		}
	}
}