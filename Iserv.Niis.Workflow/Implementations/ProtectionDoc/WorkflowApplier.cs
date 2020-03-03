using System;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Business.Notifications.Abstract;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Workflow;
using Iserv.Niis.Workflow.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Workflow.Implementations.ProtectionDoc
{
    public class WorkflowApplier: IWorkflowApplier<Domain.Entities.ProtectionDoc.ProtectionDoc>
    {
        private readonly NiisWebContext _context;
        private readonly INotificationSender _notificationSender;

        public WorkflowApplier(
            NiisWebContext context,
            INotificationSender notificationSender)
        {
            _context = context;
            _notificationSender = notificationSender;
        }

        public async Task ApplyAsync(BaseWorkflow workflow)
        {
            if (!(workflow is ProtectionDocWorkflow))
                throw new ArgumentException(
                    $"The argument type: ${workflow.GetType()} is not {nameof(ProtectionDocWorkflow)}");

            var protectionDocWorkflow = (ProtectionDocWorkflow)workflow;
            protectionDocWorkflow.DateUpdate = DateTimeOffset.Now;
            if (protectionDocWorkflow.Id < 1)
            {
                protectionDocWorkflow.DateCreate = DateTimeOffset.Now;
            }
            await _context.ProtectionDocWorkflows.AddAsync(protectionDocWorkflow);

            var protectionDoc = protectionDocWorkflow.Owner ?? _context.ProtectionDocs
                              .Include(r => r.Type)
                              .SingleOrDefault(r => r.Id == protectionDocWorkflow.OwnerId);
            if (protectionDoc == null)
                throw new ApplicationException($"Workflow has incorrect request id: {protectionDocWorkflow.OwnerId}");
            var stage = await _context.DicRouteStages.SingleOrDefaultAsync(rs => rs.Id == protectionDocWorkflow.CurrentStageId);
            protectionDoc.StatusId = stage?.ProtectionDocStatusId ?? protectionDoc.StatusId;

            protectionDoc.CurrentWorkflow = protectionDocWorkflow;
            protectionDoc.CurrentWorkflowId = protectionDocWorkflow.Id;
            protectionDoc.DateUpdate = DateTimeOffset.Now;

            _context.ProtectionDocs.Update(protectionDoc);
            await _context.SaveChangesAsync();
            await _notificationSender.ProcessRequestAsync(protectionDoc.Id);

        }
        
        public async Task ApplyInitialAsync(Domain.Entities.ProtectionDoc.ProtectionDoc protectionDoc, int userId)
        {
            var initialWorkflow = CreateInitialWorkflow(protectionDoc, userId);

            if (initialWorkflow != null)
            {
                await ApplyAsync(initialWorkflow);
            }
        }

        private ProtectionDocWorkflow CreateInitialWorkflow(Domain.Entities.ProtectionDoc.ProtectionDoc protectionDoc, int userId)
        {
            var routeId = _context.DicRoutes.Single(r => r.Code.Equals("GR")).Id;
            var initialStage = _context.DicRouteStages.Single(s => s.IsFirst && s.RouteId == routeId);

            return new ProtectionDocWorkflow
            {
                CurrentUserId = userId,
                OwnerId = protectionDoc.Id,
                Owner = protectionDoc,
                CurrentStageId = initialStage.Id,
                RouteId = initialStage.RouteId,
                IsComplete = initialStage.IsLast,
                IsSystem = initialStage.IsSystem,
                IsMain = initialStage.IsMain
            };
        }
    }
}
