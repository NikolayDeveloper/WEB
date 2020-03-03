using System.Linq;
using Iserv.Niis.Domain.Entities.Document;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.Document
{
    public class GetDocumentByIdForWorkflowServiceQuery : BaseQuery
    {
        public Domain.Entities.Document.Document Execute(int documentId)
        {
            var repository = Uow.GetRepository<Domain.Entities.Document.Document>();

            return repository.AsQueryable()
                .Include(r => r.Workflows).ThenInclude(r => r.CurrentStage)
                .Include(r => r.Workflows).ThenInclude(r => r.Route)
                .Select(r => new Domain.Entities.Document.Document
                {
                    Workflows = r.Workflows.Where(c => c.IsCurent).Select(d => new DocumentWorkflow
                            {
                                Route = d.Route,
                                CurrentStage = d.CurrentStage,
                                DateCreate = d.DateCreate,
                                Id = d.Id,
                                ExternalId = d.ExternalId,
                                DateUpdate = d.DateUpdate,
                                ControlDate = d.ControlDate,
                                CurrentStageId = d.CurrentStageId,
                                CurrentUserId = d.CurrentUserId,
                                DateReceived = d.DateReceived,
                                Description = d.Description,
                                FromStageId = d.FromStageId,
                                FromUserId = d.FromUserId,
                                OwnerId = d.OwnerId,
                                PreviousWorkflowId = d.PreviousWorkflowId,
                                RouteId = d.RouteId,
                                IsComplete = d.IsComplete,
                                IsMain = d.IsMain,
                                IsSystem = d.IsSystem,
                                IsCurent = d.IsCurent
                            }).ToArray(),
                    Id = r.Id
                })
                .FirstOrDefault(r => r.Id == documentId);
        }
    }
}