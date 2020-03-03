using System;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Business.Notifications.Abstract;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Entities.Workflow;
using Iserv.Niis.Workflow.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Workflow.Implementations.Request
{
    public class WorkflowApplier : IWorkflowApplier<Domain.Entities.Request.Request>
    {
        private readonly NiisWebContext _context;
        private readonly INotificationSender _notificationSender;
        private readonly ITaskRegister<Domain.Entities.Request.Request> _taskRegister;

        public WorkflowApplier(
            NiisWebContext context,
            ITaskRegister<Domain.Entities.Request.Request> taskRegister,
            INotificationSender notificationSender)
        {
            _context = context;
            _taskRegister = taskRegister;
            _notificationSender = notificationSender;
        }

        public async Task ApplyAsync(BaseWorkflow workflow)
        {
            if (!(workflow is RequestWorkflow))
                throw new ArgumentException(
                    $"The argument type: ${workflow.GetType()} is not {nameof(RequestWorkflow)}");

            var requestWorkflow = (RequestWorkflow) workflow;
            requestWorkflow.DateUpdate = DateTimeOffset.Now;
            if (requestWorkflow.Id < 1)
            {
                requestWorkflow.DateCreate = DateTimeOffset.Now;
            }
            await _context.RequestWorkflows.AddAsync(requestWorkflow);

            var request = requestWorkflow.Owner ?? _context.Requests
                              .Include(r => r.ProtectionDocType).Include(r => r.Documents)
                              .ThenInclude(a => a.Document.Type).Include(r => r.CurrentWorkflow.CurrentStage)
                              .SingleOrDefault(r => r.Id == requestWorkflow.OwnerId);
            if (request == null)
                throw new ApplicationException($"Workflow has incorrect request id: {requestWorkflow.OwnerId}");
            var stage =
                await _context.DicRouteStages.SingleOrDefaultAsync(rs => rs.Id == requestWorkflow.CurrentStageId);
            CheckStage(request, stage);
            request.StatusId = stage?.RequestStatusId ?? request.StatusId;
            request.IsRead = false;

            request.CurrentWorkflow = requestWorkflow;
            request.CurrentWorkflowId = requestWorkflow.Id;            
            request.DateUpdate = DateTimeOffset.Now;

            request.IsComplete = workflow.IsComplete ?? request.IsComplete;
            _context.Requests.Update(request);
            _context.SaveChanges();
            await _taskRegister.RegisterAsync(request.Id);
            await _notificationSender.ProcessRequestAsync(request.Id);
        }

        public async Task ApplyInitialAsync(Domain.Entities.Request.Request request, int userId)
        {
            var initialWorkflow = CreateInitialWorkflow(request, userId);
            if (initialWorkflow != null)
                await ApplyAsync(initialWorkflow);
        }

        private void CheckStage(Domain.Entities.Request.Request request, DicRouteStage nextStage)
        {
            var queue = _context.WorkflowTaskQueues.Include(a => a.ConditionStage).Include(a => a.ResultStage)
                .LastOrDefault(a => a.RequestId == request.Id);
            if (nextStage.Code == "U04" && request.CurrentWorkflow.CurrentStage.Code == "U03.8"
            ) //Готовые для передачи в Госреестр (ПМ)
                if (queue != null && request.Documents.All(a => a.Document.Type.Code != "001.004G.1") &&
                    queue.ConditionStage.Code == "U03.8" && queue.ResultStage.Code == "U04" &&
                    queue.ResolveDate > DateTime.Now)
                    throw new Exception(
                        "Переход на этап \"Создание охранного документа\" не возможно пока не истечет срок публикации, либо отсутствует входящий документ «Ходатайство о досрочной публикации» ");
        }

        private RequestWorkflow CreateInitialWorkflow(Domain.Entities.Request.Request request, int userId)
        {
            var protectionDocType = _context.DicProtectionDocTypes.Single(t => t.Id == request.ProtectionDocTypeId);
            var initialStage =
                _context.DicRouteStages.SingleOrDefault(s => s.IsFirst && s.RouteId == protectionDocType.RouteId);
            if (initialStage == null) return null;

            return new RequestWorkflow
            {
                CurrentUserId = userId,
                OwnerId = request.Id,
                Owner = request,
                CurrentStageId = initialStage.Id,
                RouteId = initialStage.RouteId,
                IsComplete = initialStage.IsLast,
                IsSystem = initialStage.IsSystem,
                IsMain = initialStage.IsMain
            };
        }
    }
}