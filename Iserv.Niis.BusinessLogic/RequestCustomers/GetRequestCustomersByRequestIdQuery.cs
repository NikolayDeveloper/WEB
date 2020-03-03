using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.RequestCustomers
{
    public class GetRequestCustomersByRequestIdQuery : BaseQuery
    {
        public async Task<List<RequestCustomer>> ExecuteAsync(int requestId)
        {
            var requestCustomerRepo = Uow.GetRepository<RequestCustomer>();
            return await requestCustomerRepo
                .AsQueryable()
                .Include(rc => rc.CustomerRole)
                .Include(rc => rc.Customer).ThenInclude(c => c.Type)
                .Include(rc => rc.Customer).ThenInclude(c => c.Country)
                .Include(rc => rc.Customer).ThenInclude(c => c.ContactInfos).ThenInclude(ci => ci.Type)
                .Where(rc => rc.RequestId == requestId)
                .ToListAsync();
        }
    }
}