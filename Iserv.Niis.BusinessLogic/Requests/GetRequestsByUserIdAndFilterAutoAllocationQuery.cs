using System.Linq;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Requests
{
    public class GetRequestsByUserIdAndFilterAutoAllocationQuery : BaseQuery
    {
        public IQueryable<Request> Execute(int userId)
        {
            var repository = Uow.GetRepository<Request>();

            var routeStagesForAutoAllocation = new[] { RouteStageCodes.I_03_2_3, RouteStageCodes.I_03_2_3_0, RouteStageCodes.UM_03_1 };

            return repository.AsQueryable()
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentUser)
                .Include(r => r.RequestCustomers).ThenInclude(rc => rc.CustomerRole)
                .Include(r => r.RequestCustomers).ThenInclude(rc => rc.Customer).ThenInclude(c => c.Type)
                .Include(r => r.ProtectionDocType)
                .Include(r => r.IPCRequests).ThenInclude(ipc => ipc.Ipc)
                .Where(r => r.CurrentWorkflowId != null
                            && r.CurrentWorkflow.CurrentUserId == userId
                            && routeStagesForAutoAllocation.Contains(r.CurrentWorkflow.CurrentStage.Code)
                            && r.IPCRequests.Any(i => i.IsMain));
        }
    }
}
