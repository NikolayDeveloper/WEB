using System.Linq;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.Documents.DocumentsBusinessLogic.Requests
{
    public class GetLastRequestByNumberQuery: BaseQuery
    {
        public Request Execute()
        {
            var requestRepository = Uow.GetRepository<Request>();

            var request = requestRepository.AsQueryable()
                .OrderBy(r => r.RequestNum)
                .LastOrDefault();

            return request;
        }
    }
}
