using Iserv.Niis.Domain.Entities.AccountingData;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicCustomers
{
    public class GetDicCostumersByXinQuery : BaseQuery
    {
        public async Task<List<DicCustomer>> ExecuteAsync(string xin, bool? isPatentAttorney)
        {
            var dicRepository = Uow.GetRepository<DicCustomer>();

            var customerQuery = dicRepository.AsQueryable()
                .Include(c => c.Type)
                .Include(c => c.CustomerAttorneyInfos)
                .Include(c => c.ContactInfos).ThenInclude(ci => ci.Type)
                .Where(d => d.IsDeleted == false)
                .Where(r => r.Xin == xin)
                .Where(r => isPatentAttorney.HasValue && isPatentAttorney.Value && !string.IsNullOrEmpty(r.PowerAttorneyFullNum)
                    || string.IsNullOrEmpty(r.PowerAttorneyFullNum));
            
            return await customerQuery.ToListAsync();
        }
    }
}