using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.Bibliographics.Icis.Request
{
    public class GetIcisByRequestIdQuery: BaseQuery
    {
        public List<ICISRequest> Execute(int requestId)
        {
            var repository = Uow.GetRepository<ICISRequest>();

            return repository.AsQueryable()
                .Where(i => i.RequestId == requestId)
                .ToList();
        }
    }
}
