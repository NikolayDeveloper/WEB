using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.Documents.DocumentsBusinessLogic.Workflows
{
    public class GetRequestWorkflowsByOwnerIdQuery : BaseQuery
    {
        public List<RequestWorkflow> Execute(int ownerId)
        {
            var requestWorkflowRepository = Uow.GetRepository<RequestWorkflow>();
            var requestWorkflows = requestWorkflowRepository
                .AsQueryable()
                .Include(r => r.FromStage)
                .Include(r => r.CurrentStage)
                .Include(r => r.FromUser).ThenInclude(u => u.Position).ThenInclude(u => u.PositionType)
                .Include(r => r.CurrentUser)
                .Include(r => r.Route)
                .Where(rc => rc.OwnerId == ownerId)
                .ToList();

            return requestWorkflows;
        }
    }
}
