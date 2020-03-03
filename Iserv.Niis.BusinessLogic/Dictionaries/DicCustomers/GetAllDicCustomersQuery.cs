using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.AccountingData;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicCustomers
{
    public class GetAllDicCustomersQuery: BaseQuery
    {
        public IQueryable<DicCustomer> Execute()
        {
            var repo = Uow.GetRepository<DicCustomer>();

            var customers = repo.AsQueryable()
                .Where(d => d.IsDeleted == false)
                .Include(r => r.Type)
                .AsQueryable();

            return customers;
        }
    }
}
