using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.RequestCustomers
{
    public class GetRequestCustomerByIdQuery : BaseQuery
    {
        public async Task<RequestCustomer> ExecuteAsync(int requestCustomerId)
        {
            var requestCustomerRepo = Uow.GetRepository<RequestCustomer>();
            return await requestCustomerRepo
                .AsQueryable()
                .Include(rc => rc.Customer).ThenInclude(c => c.Type)
                .Include(rc => rc.Customer).ThenInclude(c => c.Country)
                .Include(rc => rc.Customer).ThenInclude(c => c.CustomerAttorneyInfos)
                .Include(rc => rc.Customer).ThenInclude(c => c.ContactInfos).ThenInclude(ci => ci.Type)
                .Include(rc => rc.CustomerRole)
                .FirstOrDefaultAsync(rc => rc.Id == requestCustomerId);
        }
    }
}