using System.Linq;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Requests
{
    public class GetRequestIdByIncomingNumberQuery : BaseQuery
    {
        public int? Execute(string number)
        {
            var repository = Uow.GetRepository<Request>();

            var resuests = repository
                .AsQueryable()
                .Where(r => r.IncomingNumber == number || r.RequestNum == number);

            return resuests.Select(d => d.Id).FirstOrDefault();
        }
    }
}