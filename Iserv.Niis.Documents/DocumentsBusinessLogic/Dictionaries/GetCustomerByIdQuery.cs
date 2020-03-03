using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.Documents.DocumentsBusinessLogic.Dictionaries
{
    public class GetCustomerByIdQuery: BaseQuery
    {
        public DicCustomer Execute(int customerId)
        {
            var customerRepository = Uow.GetRepository<DicCustomer>();
            var customer = customerRepository.GetById(customerId);

            return customer;
        }
    }
}
