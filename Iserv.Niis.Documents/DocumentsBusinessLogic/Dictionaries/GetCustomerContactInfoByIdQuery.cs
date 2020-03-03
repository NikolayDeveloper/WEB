using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.Documents.DocumentsBusinessLogic.Dictionaries
{
    public class GetCustomerContactInfoByIdQuery : BaseQuery
    {
        public ICollection<ContactInfo> Execute(int customerId)
        {
            var customerRepository = Uow.GetRepository<DicCustomer>();
            var customer = customerRepository
                    .AsQueryable()
                    .Include(d => d.ContactInfos).ThenInclude(d => d.Type)
                    .FirstOrDefault(d => d.Id == customerId);

            return customer == null
                    ? new ContactInfo[0]
                    : customer.ContactInfos;
        }
    }
}
