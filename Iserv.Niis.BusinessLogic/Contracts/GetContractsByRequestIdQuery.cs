using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Contract;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Contracts
{
    public class GetContractsByRequestIdQuery: BaseQuery
    {
        public async Task<List<Contract>> ExecuteAsync(int requestId)
        {
            var repo = Uow.GetRepository<Contract>();
            var contracts = repo.AsQueryable()
                .Include(c => c.Workflows).ThenInclude(cw => cw.CurrentStage)
                .Include(c => c.Workflows).ThenInclude(cw => cw.CurrentUser)
                .Include(c => c.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
                .Include(c => c.CurrentWorkflow).ThenInclude(cw => cw.CurrentUser)
                .Include(c => c.Status)
                .Include(c => c.Category)
                .Include(c => c.Type)
                .Include(c => c.ContractCustomers).ThenInclude(cc => cc.CustomerRole)
                .Include(c => c.ContractCustomers).ThenInclude(cc => cc.Customer)
                .Where(c => c.RequestsForProtectionDoc.Any(cr => cr.RequestId == requestId));

            return await contracts.ToListAsync();
        }
    }
}
