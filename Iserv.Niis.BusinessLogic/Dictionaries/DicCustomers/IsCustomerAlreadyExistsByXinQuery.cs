using System.Linq;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicCustomers
{
    public class IsCustomerAlreadyExistsByXinQuery : BaseQuery
    {
        public bool Execute(string xin)
        {
            var customerRepo = Uow.GetRepository<DicCustomer>();
            return customerRepo.AsQueryable().Where(d => d.IsDeleted == false).Any(dc => dc.Xin == xin);
        }
    }
}