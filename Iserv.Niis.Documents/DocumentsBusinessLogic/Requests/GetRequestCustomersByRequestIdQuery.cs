using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.Documents.DocumentsBusinessLogic.Requests
{
    public class GetRequestCustomersByRequestIdQuery: BaseQuery
    {
        public List<RequestCustomer> Execute(int requestId)
        {
            var repo = Uow.GetRepository<RequestCustomer>();
            var result = repo.AsQueryable()
                .Include(r => r.CustomerRole)
                .Include(r => r.Customer).ThenInclude(c => c.Country)
                .Where(r => r.RequestId == requestId);

            return result.ToList();
        }
    }
}
