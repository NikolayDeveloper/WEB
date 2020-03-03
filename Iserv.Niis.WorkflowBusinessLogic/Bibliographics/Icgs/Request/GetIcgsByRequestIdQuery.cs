using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.Bibliographics.Icgs.Request
{
    public class GetIcgsByRequestIdQuery: BaseQuery
    {
        public List<ICGSRequest> Execute(int requestId)
        {
            var repository = Uow.GetRepository<ICGSRequest>();

            return repository.AsQueryable()
                .Where(i => i.RequestId == requestId)
                .ToList();
        }
    }
}
