using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Linq;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicCustomers.Requests
{
    public class GetAddresseeByRequestIdQuery : BaseQuery
    {
        public (DicCustomer addressee, string requestAddresseeAddress) Execute(int requestId)
        {
            var repo = Uow.GetRepository<Request>();
            var request = repo.AsQueryable()
                .Include(r => r.Addressee)
                .FirstOrDefault(r => r.Id == requestId);

            return (request?.Addressee, request?.AddresseeAddress);

        }
    }
}
