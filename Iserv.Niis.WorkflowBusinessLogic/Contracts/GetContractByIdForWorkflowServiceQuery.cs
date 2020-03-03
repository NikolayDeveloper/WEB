using System.Linq;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.Contracts
{
    public class GetContractByIdForWorkflowServiceQuery : BaseQuery
    {
        public Contract Execute(int requestId)
        {
            var repository = Uow.GetRepository<Contract>();

            return repository.AsQueryable()
                .Include(r => r.CurrentWorkflow).ThenInclude(r => r.CurrentStage)
                .Include(r => r.CurrentWorkflow).ThenInclude(r => r.Route)
                .Select(d => new Contract
                {
                    CurrentWorkflow = new ContractWorkflow
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
                        IsSystem = d.CurrentWorkflow.IsSystem
                    },
                    Id = d.Id
                })
                .FirstOrDefault(r => r.Id == requestId);
        }
    }
}