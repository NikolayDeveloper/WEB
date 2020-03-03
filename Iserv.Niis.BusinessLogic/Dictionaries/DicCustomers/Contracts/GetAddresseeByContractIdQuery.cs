using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Contract;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Linq;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicCustomers.Contracts
{
    public class GetAddresseeByContractIdQuery : BaseQuery
    {
        public (DicCustomer addressee, string contractAddresseeAddress) Execute(int customerId)
        {
            var repo = Uow.GetRepository<Contract>();
            var contract = repo.AsQueryable()
                 .Include(c => c.Addressee)
                 .FirstOrDefault(c => c.Id == customerId);

            return (contract?.Addressee, contract?.AddresseeAddress);
        }
    }
}
