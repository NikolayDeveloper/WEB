using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Contract;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.ContractCustomers
{
    public class GetContractCustomerByIdQuery : BaseQuery
    {
        public async Task<ContractCustomer> ExecuteAsync(int contractCustomerId)
        {
            var contractCustomerRepo = Uow.GetRepository<ContractCustomer>();

            return await contractCustomerRepo
                .AsQueryable()
                .Include(cc => cc.CustomerRole)
                .Include(cc => cc.Customer).ThenInclude(c => c.Type)
                .Include(cc => cc.Customer).ThenInclude(c => c.CustomerAttorneyInfos)
                .Include(rc => rc.Customer).ThenInclude(c => c.ContactInfos).ThenInclude(ci => ci.Type)
                .FirstOrDefaultAsync(cc => cc.Id == contractCustomerId);
        }
    }
}