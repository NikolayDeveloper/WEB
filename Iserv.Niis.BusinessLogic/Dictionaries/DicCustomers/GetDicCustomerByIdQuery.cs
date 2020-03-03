using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicCustomers
{
    public class GetDicCustomerByIdQuery : BaseQuery
    {
        public DicCustomer Execute(int id)
        {
            var repo = Uow.GetRepository<DicCustomer>();
            return repo.GetById(id);
        }
    }
}
