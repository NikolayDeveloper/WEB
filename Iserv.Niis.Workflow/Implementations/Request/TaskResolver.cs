using System;
using System.Linq;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Entities.Workflow;
using Iserv.Niis.Workflow.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Workflow.Implementations.Request
{
    public class TaskResolver : ITaskResolver<Domain.Entities.Request.Request>
    {
        private readonly NiisWebContext _context;
        private readonly IWorkflowApplier<Domain.Entities.Request.Request> _workflowApplier;
        private const int Count = 10;

        public TaskResolver(NiisWebContext context, IWorkflowApplier<Domain.Entities.Request.Request> workflowApplier)
        {
            _context = context;
            _workflowApplier = workflowApplier;
        }

        public void Resolve()
        {
            try
            {
                var tuple = ResolvePortionInternal();

                if (tuple.Item1 > 0)
                {
                    ConsoleClear();
                    Console.WriteLine($"[{DateTime.Now}] Resolved {tuple.Item1} tasks. {tuple.Item2} requests were affected");
                }
            }
            catch (Exception ex)
            {
                ConsoleClear();
                Console.WriteLine($"[{DateTime.Now}] Error occured: {ex.Message}");
            }
        }

        private static void ConsoleClear()
        {
            Console.Clear();
            Console.WriteLine("Working in background\nPress any key for exit");
        }

        private Tuple<int, int> ResolvePortionInternal()
        {
            var tasks = _context.WorkflowTaskQueues
                .Include(q => q.Request)
                    .ThenInclude(r => r.CurrentWorkflow)
                    .ThenInclude(cw => cw.CurrentStage)
                .Include(q => q.Request)
                    .ThenInclude(r => r.CurrentWorkflow)
                    .ThenInclude(cw => cw.Owner)
                    .ThenInclude(cw => cw.Workflows)
                    .ThenInclude(cw => cw.CurrentStage)
                .Include(q => q.Request)
                    .ThenInclude(r => r.CurrentWorkflow)
                    .ThenInclude(cw => cw.Owner)
                    .ThenInclude(cw => cw.Workflows)
                    .ThenInclude(cw => cw.FromStage)
                .Include(q => q.ConditionStage)
                .Include(q => q.ResultStage)
                .IgnoreQueryFilters()
                .Where(q => q.ResolveDate < DateTimeOffset.Now && q.RequestId != null)
                .Take(Count)
                .ToList();

            var applyedCount = 0;

            foreach (var task in tasks)
            {
                if (PosiviteConditions(task))
                {
                    _workflowApplier.ApplyAsync(GetWorkflow(task.Request.CurrentWorkflow, task)).Wait();
                    applyedCount++;
                }
            }

            _context.WorkflowTaskQueues.RemoveRange(tasks);
            _context.SaveChangesAsync().Wait();

            return new Tuple<int, int>(tasks.Count, applyedCount);
        }

        private static bool PosiviteConditions(WorkflowTaskQueue task)
        {
            return !task.Request.IsDeleted && task.Request.CurrentWorkflow.CurrentStage.Id == task.ConditionStage.Id;
        }

        private RequestWorkflow GetWorkflow(RequestWorkflow currentWorkflow, WorkflowTaskQueue task)
        {
            var taskResultStage = task.ResultStage ?? GetCalculatedResultStage(currentWorkflow);

            return new RequestWorkflow
            {
                OwnerId = currentWorkflow.OwnerId,
                FromUserId = currentWorkflow.CurrentUserId,
                FromStageId = currentWorkflow.CurrentStageId,
                CurrentUserId = currentWorkflow.CurrentUserId,
                CurrentStageId = taskResultStage.Id,
                RouteId = taskResultStage.RouteId,
                IsComplete = taskResultStage.IsLast,
                IsSystem = taskResultStage.IsSystem,
                IsMain = taskResultStage.IsMain
            };
        }

        private DicRouteStage GetCalculatedResultStage(RequestWorkflow currentWorkflow)
        {
            if (currentWorkflow.CurrentStage.Code.Equals("B03.3.1.1.0"))
            {
                if (currentWorkflow.FromStage.Code.Equals("B03.3.1.1"))
                {
                    return _context.DicRouteStages.Single(s => s.Code.Equals("B03.3.1.1.1"));
                }

                if (currentWorkflow.FromStage.Code.Equals("B03.3.4.1"))
                {
                    return _context.DicRouteStages.Single(s => s.Code.Equals("B03.3.9"));
                }
            }

            if (currentWorkflow.CurrentStage.Code.Equals("B03.3.1.1.1") && (AnyDocuments(currentWorkflow.OwnerId,
                    DicDocumentType.Codes.NotificationOfAnswerlessPatentRequestFinalRecall,
                    DicDocumentType.Codes.NotificationOfPaymentlessExaminationFinalRecall)
                    || currentWorkflow.Owner.Workflows.Any(rw => rw.CurrentStage.Code.Equals("B03.2.4"))))
            {
                return _context.DicRouteStages.Single(s => s.Code.Equals("B04.0"));
            }

            if (currentWorkflow.CurrentStage.Code.Equals("B02.2.0")
                || currentWorkflow.CurrentStage.Code.Equals("B03.3.1.1")
                && (AnyDocuments(currentWorkflow.OwnerId, DicDocumentType.Codes.NotificationOfRevocationOfPatentApplication)
                || currentWorkflow.Owner.Workflows.Any(rw => rw.CurrentStage.Code.Equals("B03.2.4"))))
            {
                return _context.DicRouteStages.Single(s => s.Code.Equals("B03.3.1.1.1"));
            }

            if (currentWorkflow.CurrentStage.Code.Equals("TM03.3.7.1") && AnyDocuments(currentWorkflow.OwnerId, DicDocumentType.Codes.NotificationOfAnswerTimeExpiration)
                || currentWorkflow.CurrentStage.Code.Equals("TM03.3.7.3") && AnyDocuments(currentWorkflow.OwnerId, DicDocumentType.Codes.NotificationOfAnswerTimeExpiration)
                || currentWorkflow.CurrentStage.Code.Equals("TM03.2.2.0") && AnyDocuments(currentWorkflow.OwnerId, DicDocumentType.Codes.NotificationOfRegistrationExaminationTimeExpiration))
            {
                return _context.DicRouteStages.Single(s => s.Code.Equals("TM03.3.7.0"));
            }

            if (currentWorkflow.CurrentStage.Code.Equals("TM03.3.7.0") && AnyDocuments(currentWorkflow.OwnerId, DicDocumentType.Codes.NotificationOfRefusal, DicDocumentType.Codes.NotificationOfPaymentlessOfficeWorkTermination)
                || currentWorkflow.CurrentStage.Code.Equals("TM03.3.7.3") && AnyDocuments(currentWorkflow.OwnerId, DicDocumentType.Codes.NotificationOfAnswerlessOfficeWorkTermination))
            {
                return _context.DicRouteStages.Single(s => s.Code.Equals("TM03.3.9"));
            }

            return _context.DicRouteStages.Single(s => s.Code.Equals("X01"));
        }

        private bool AnyDocuments(int requestId, params string[] docType)
        {
            return _context.RequestsDocuments.Any(rd =>
                rd.RequestId == requestId && docType.Contains(rd.Document.Type.Code));
        }
    }
}