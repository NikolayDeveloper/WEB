using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.BusinessLogic.Requests
{
    public class GetRequestsByIdsQuery : BaseQuery
    {
        public List<Request> Execute(int[] Ids)
        {
            var repository = Uow.GetRepository<Request>();
            var result = repository
                .AsQueryable()
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentUser)
                .Include(r => r.RequestCustomers).ThenInclude(rc => rc.CustomerRole)
                .Include(r => r.RequestCustomers).ThenInclude(rc => rc.Customer).ThenInclude(c => c.Type)
                .Include(r => r.ProtectionDocType)
				.Include(r => r.RequestType)
				.Include(r => r.IPCRequests).ThenInclude(ipc => ipc.Ipc)
                .Where(r => Ids.Contains(r.Id))
                .ToList();
            return result;
        }
    }
}
