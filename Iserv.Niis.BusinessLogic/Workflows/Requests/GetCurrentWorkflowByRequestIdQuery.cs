using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Linq;

namespace Iserv.Niis.BusinessLogic.Workflows.Requests
{
    public class GetCurrentWorkflowByRequestIdQuery : BaseQuery
    {
        public RequestWorkflow Execute(int requestId)
        {
            var repo = Uow.GetRepository<Request>();
            var currentRequestWorkflow = repo
                .AsQueryable()
                .Include(r => r.CurrentWorkflow).ThenInclude(c => c.CurrentStage)
                .Where(r => r.Id == requestId)
                .FirstOrDefault()?.CurrentWorkflow;
            return currentRequestWorkflow;
        }
    }
}
