using System.Linq;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Requests
{
    public class GetRequestsByUserIdQuery: BaseQuery
    {
        public IQueryable<Request> Execute(int userId)
        {
			var repository = Uow.GetRepository<Request>();

            return repository.AsQueryable()
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentUser)
                .Include(r => r.PaymentExecutors)
                .Include(r => r.Workflows).ThenInclude(w => w.CurrentStage)
                .Include(r => r.RequestCustomers).ThenInclude(rc => rc.CustomerRole)
                .Include(r => r.RequestCustomers).ThenInclude(rc => rc.Customer).ThenInclude(c => c.Type)
                .Include(r => r.IPCRequests).ThenInclude(ir => ir.Ipc)
                .Include(r => r.ProtectionDocType)
                .Where(r => (r.CurrentWorkflowId != null && r.CurrentWorkflow.CurrentUserId == userId) ||
                            //(r.UserId != null && r.UserId == userId) ||
                            r.PaymentExecutors.Select(pe => pe.UserId).Contains(userId));
        }
    }
}
