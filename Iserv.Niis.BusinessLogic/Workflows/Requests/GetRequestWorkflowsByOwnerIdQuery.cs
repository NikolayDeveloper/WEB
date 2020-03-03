using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Workflows.Requests
{
    public class GetRequestWorkflowsByOwnerIdQuery : BaseQuery
    {
        public async ValueTask<List<RequestWorkflow>> ExecuteAsync(int ownerId)
        {
            var requestWorkflowRepository = Uow.GetRepository<RequestWorkflow>();
            var requestWorkflows = await requestWorkflowRepository
                .AsQueryable()
                .Include(r => r.FromStage)
                .Include(r => r.CurrentStage)
                .Include(r => r.FromUser)
                .Include(r => r.CurrentUser)
                .Include(r => r.Route)
                .Where(rc => rc.OwnerId == ownerId)
                .OrderBy(rc => rc.DateCreate)
                .ToListAsync();

            return requestWorkflows;
        }
    }
}