using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Linq;

namespace Iserv.Niis.BusinessLogic.Requests
{
    public class GetRequestNamesByIdQuery : BaseQuery
    {
        public Request Execute(int requestId)
        {
            var repository = Uow.GetRepository<Request>();

            var names = repository.AsQueryable().Select(r => new Request
            {
                NameRu = r.NameRu,
                NameKz = r.NameKz,
                NameEn = r.NameEn
            }).FirstOrDefault(r => r.Id == requestId);

            return names;
        }
    }
}
