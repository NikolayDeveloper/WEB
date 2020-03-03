using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Workflow.Abstract;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Iserv.Niis.Workflow.Implementations.Request.Stages.DocumentApplier
{
    public abstract class BaseLogic
    {
        private readonly IWorkflowApplier<Domain.Entities.Request.Request> _workflowApplier;
        private readonly NiisWebContext _context;

        protected BaseLogic(
            IWorkflowApplier<Domain.Entities.Request.Request> workflowApplier, 
            NiisWebContext context)
        {
            _workflowApplier = workflowApplier;
            _context = context;
        }

        public abstract Task ApplyAsync(RequestDocument requestDocument);

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
                Log.Warning($"Workflow logic applyer. The next stage query is incorrect: {nextStageFunc}!");
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
				IsMain = nextStage.IsMain,
            };

            await _workflowApplier.ApplyAsync(workflow);
        }
        
        protected bool FromStageContains(Domain.Entities.Request.Request request, params string[] codes)
        {
            return codes.Contains(request.CurrentWorkflow.FromStage.Code);
        }

        protected bool CurrentStageContains(Domain.Entities.Request.Request request, params string[] codes)
        {
            return codes.Contains(request.CurrentWorkflow.CurrentStage.Code);
        }

        protected bool CurrentStageContains(Domain.Entities.Document.Document document, params string[] codes)
        {
            return codes.Contains(document.CurrentWorkflow.CurrentStage.Code);
        }
        
        protected bool AnyDocuments(Domain.Entities.Request.Request request, string documentTypeCode)
        {
            return request.Documents.Select(rd => rd.Document).Any(d => d.Type.Code.Equals(documentTypeCode));
        }

        protected bool AnyDocuments(Domain.Entities.Request.Request request, params string[] documentTypeCodes)
        {
            return request.Documents.Select(rd => rd.Document).Any(d => documentTypeCodes.Contains(d.Type.Code));
        }

        protected bool HasPaidInvoices(Domain.Entities.Request.Request request, params string[] tariffCodes)
        {
            var restorationInvoices = request.PaymentInvoices
                .Where(pi => tariffCodes.Contains(pi.Tariff.Code)).ToList();
            return restorationInvoices.Any() && restorationInvoices.All(pi => pi.Status.Code != "notpaid");
        }
    }
}