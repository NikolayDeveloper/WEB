using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.ProtectionDocCustomers
{
    public class GetProtectionDocCustomerByIdQuery: BaseQuery
    {
        public async Task<ProtectionDocCustomer> ExecuteAsync(int protectionDocCustomerId)
        {
            var repo = Uow.GetRepository<ProtectionDocCustomer>();
            return await repo
                .AsQueryable()
                .Include(cc => cc.CustomerRole)
                .Include(cc => cc.Customer).ThenInclude(c => c.Type)
                .Include(cc => cc.Customer).ThenInclude(c => c.CustomerAttorneyInfos)
                .Include(rc => rc.Customer).ThenInclude(c => c.ContactInfos).ThenInclude(ci => ci.Type)
                .FirstOrDefaultAsync(cc => cc.Id == protectionDocCustomerId);
        }
    }
}
