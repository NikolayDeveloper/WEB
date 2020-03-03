using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Linq;

namespace Iserv.Niis.BusinessLogic.Requests
{
    public class GetRequsetCurrentWorkflowByIdQuery : BaseQuery
    {
        public Request Execute(int requsetId)
        {
            var repo = Uow.GetRepository<Request>();
            return repo.AsQueryable()
                .Include(r => r.CurrentWorkflow)
                .FirstOrDefault(r => r.Id == requsetId);

        }
    }
}
