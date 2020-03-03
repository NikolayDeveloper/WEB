using System.Linq;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Requests
{
    public class GetRequestsQuery : BaseQuery
    {
        public IQueryable<Request> Execute()
        {
            var repository = Uow.GetRepository<Request>();

            return repository.AsQueryable()
                .Include(r => r.ReceiveType)
                .Include(r => r.RequestType)
                .Include(r => r.ProtectionDocType)
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentUser)
                .Include(r => r.RequestCustomers).ThenInclude(rc => rc.Customer).ThenInclude(c => c.Country)
                .Include(r => r.RequestCustomers).ThenInclude(rc => rc.CustomerRole);
        }

        public IQueryable<Request> ExecuteByProtectionDocTypeId(int protectionDocTypeId)
        {
            var repository = Uow.GetRepository<Request>();

            return repository.AsQueryable()
                .Include(r => r.ReceiveType)
                .Include(r => r.RequestType)
                .Include(r => r.ProtectionDocType)
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentUser)
                .Include(r => r.RequestCustomers).ThenInclude(rc => rc.Customer).ThenInclude(c => c.Country)
                .Include(r => r.RequestCustomers).ThenInclude(rc => rc.CustomerRole)
                .Where(r => r.ProtectionDocTypeId == protectionDocTypeId);
        }
    }
}