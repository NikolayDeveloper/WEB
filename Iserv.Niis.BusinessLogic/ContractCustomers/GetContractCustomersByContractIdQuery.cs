using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Domain.Entities.Contract;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.ContractCustomers
{
    public class GetContractCustomersByContractIdQuery : BaseQuery
    {
        public List<ContractCustomer> Execute(int contractId)
        {
            var contractCustomerRepo = Uow.GetRepository<ContractCustomer>();
            return contractCustomerRepo
                .AsQueryable()
                .Include(cc => cc.CustomerRole)
                .Include(cc => cc.Customer).ThenInclude(c => c.Type)
                .Include(rc => rc.Customer).ThenInclude(c => c.Country)
                .Include(cc => cc.Customer).ThenInclude(c => c.ContactInfos).ThenInclude(ci => ci.Type)
                .Where(cc => cc.ContractId == contractId)
                .ToList();
        }
    }
}