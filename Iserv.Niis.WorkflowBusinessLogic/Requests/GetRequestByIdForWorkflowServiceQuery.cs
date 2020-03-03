using System.Linq;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.Requests
{
    /// <summary>
    /// Класс, представляющий запрос на получение заявки по его идентификатору.
    /// </summary>
    public class GetRequestByIdForWorkflowServiceQuery : BaseQuery
    {
        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <param name="requestId">Идентификатор заявки.</param>
        /// <returns></returns>
        public Request Execute(int requestId)
        {
            var repository = Uow.GetRepository<Request>();

            return repository.AsQueryable()
                .Include(r => r.CurrentWorkflow).ThenInclude(r => r.Route)
                .Include(r => r.CurrentWorkflow).ThenInclude(r => r.CurrentStage)
                .Include(r => r.Department)
                .Select(d => new Request
                {
                    CurrentWorkflow = new RequestWorkflow
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
                        PaymentCharges = d.CurrentWorkflow.PaymentCharges,
                        IsChangeScenarioEntry = d.CurrentWorkflow.IsChangeScenarioEntry,
                        IsComplete = d.CurrentWorkflow.IsComplete,
                        IsMain = d.CurrentWorkflow.IsMain,
                        IsSystem = d.CurrentWorkflow.IsSystem
                    },
                    Department = new DicDepartment
                    {
                        Code = d.Department.Code
                    },
                    Id = d.Id
                })
                .FirstOrDefault(r => r.Id == requestId);
        }
    }
}