using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.Requests
{
    public class GetRequestWorkflowsByRequestIdQuery: BaseQuery
    {
        public List<RequestWorkflow> Execute(int requestId)
        {
            var repo = Uow.GetRepository<RequestWorkflow>();
            var requestWorkflows = repo.AsQueryable()
                .Include(rw => rw.CurrentStage)
                .Include(rw => rw.FromStage)
                .Include(rw => rw.CurrentUser)
                .Include(rw => rw.FromUser)
                .Where(rw => rw.OwnerId == requestId);

            return requestWorkflows.ToList();
        }
    }
}
