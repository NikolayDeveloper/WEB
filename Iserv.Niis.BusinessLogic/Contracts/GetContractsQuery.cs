using System.Linq;
using Iserv.Niis.Domain.Entities.Contract;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Contracts
{
    public class GetContractsQuery : BaseQuery
    {
        public IQueryable<Contract> Execute()
        {
            var repository = Uow.GetRepository<Contract>();

            return repository.AsQueryable()
                .Include(r => r.ReceiveType)
                .Include(r => r.Type)
                .Include(r => r.Category)
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentUser)
                .Include(r => r.ContractCustomers).ThenInclude(rc => rc.Customer).ThenInclude(c => c.Country)
                .Include(r => r.ContractCustomers).ThenInclude(rc => rc.CustomerRole);
        }
    }
}