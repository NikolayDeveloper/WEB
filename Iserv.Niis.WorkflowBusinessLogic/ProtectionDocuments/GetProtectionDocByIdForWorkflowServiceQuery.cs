using System.Linq;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.WorkflowBusinessLogic.Workflows;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.ProtectionDocuments
{
    public class GetProtectionDocByIdForWorkflowServiceQuery : BaseQuery
    {
        public ProtectionDoc Execute(int requestId, int? specialUserId = null)
        {
            var repository = Uow.GetRepository<ProtectionDoc>();

            var res = repository.AsQueryable()
                .Include(r => r.Type)
                .Include(r => r.CurrentWorkflow).ThenInclude(r => r.CurrentStage)
                .Include(r => r.CurrentWorkflow).ThenInclude(r => r.Route)
                .Select(d => new ProtectionDoc
                {
                    CurrentWorkflow = new ProtectionDocWorkflow
                    {
                        Route = d.CurrentWorkflow.Route,
                        CurrentStage = d.CurrentWorkflow.CurrentStage,
                        DateCreate = d.CurrentWorkflow.DateCreate,
                        Id = d.CurrentWorkflow.Id,
                        ExternalId = d.CurrentWorkflow.ExternalId,
                        DateUpdate = d.CurrentWorkflow.DateUpdate,
                        ControlDate = d.CurrentWorkflow.ControlDate,
                        CurrentStageId = d.CurrentWorkflow.CurrentStageId,
                        CurrentUserId = d.CurrentWorkflow.CurrentUserId,
                        DateReceived = d.CurrentWorkflow.DateReceived,
                        Description = d.CurrentWorkflow.Description,
                        FromStageId = d.CurrentWorkflow.FromStageId,
                        FromUserId = d.CurrentWorkflow.FromUserId,
                        OwnerId = d.CurrentWorkflow.OwnerId,
                        PreviousWorkflowId = d.CurrentWorkflow.PreviousWorkflowId,
                        RouteId = d.CurrentWorkflow.RouteId,
                        IsComplete = d.CurrentWorkflow.IsComplete,
                        IsMain = d.CurrentWorkflow.IsMain,
                        IsSystem = d.CurrentWorkflow.IsSystem,
                        SecondaryCurrentUserId = d.CurrentWorkflow.SecondaryCurrentUserId
                    },
                    Type = new DicProtectionDocType
                    {
                        Code = d.Type.Code
                    },
                    Id = d.Id
                })
                .FirstOrDefault(r => r.Id == requestId);
            if(res.CurrentWorkflow != null && res.CurrentWorkflow.CurrentStage != null && res.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.ODParallel)
            {
                var userId = specialUserId.HasValue ? specialUserId.Value : NiisAmbientContext.Current.User.Identity.UserId;
                ProtectionDocWorkflow parallelWorkflow = null;

                //var t = NiisAmbientContext.Current.Executor.GetQuery<TryGetProtectionDocWorkflowFromParalleByOwnerIdCommand>();
                //var t1 = t.Process(q => q.Execute(requestId, userId, out parallelWorkflow));

                if (NiisAmbientContext.Current.Executor.GetQuery<TryGetProtectionDocWorkflowFromParalleByOwnerIdCommand>().Process(q => q.Execute(requestId, userId, out parallelWorkflow))
                    && parallelWorkflow != null)
                {
                    res.CurrentWorkflow = parallelWorkflow;
                    NiisAmbientContext.Current.Executor.GetCommand<FinishProtectionDocParallelWorkflowCommand>().Process(q => q.Execute(parallelWorkflow));
                }
            }

            return res;
        }
    }
}