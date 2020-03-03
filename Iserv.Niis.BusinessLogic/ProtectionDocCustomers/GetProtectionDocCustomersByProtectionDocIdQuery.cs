using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.ProtectionDocCustomers
{
    public class GetProtectionDocCustomersByProtectionDocIdQuery : BaseQuery
    {
        public async Task<List<ProtectionDocCustomer>> ExecuteAsync(int protectionDocId)
        {
            var protectionDocCustomerRepo = Uow.GetRepository<ProtectionDocCustomer>();
            return await protectionDocCustomerRepo
                .AsQueryable()
                .Include(rc => rc.CustomerRole)
                .Include(cc => cc.Customer).ThenInclude(c => c.Type)
                .Include(rc => rc.Customer).ThenInclude(c => c.Country)
                .Include(cc => cc.Customer).ThenInclude(c => c.ContactInfos).ThenInclude(ci => ci.Type)
                .Where(cc => cc.ProtectionDocId == protectionDocId)
                .ToListAsync();
        }
    }
}