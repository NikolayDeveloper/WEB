using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.Bibliographics.Ipc.Request
{
    public class GetIpcByRequestIdQuery: BaseQuery
    {
        public List<IPCRequest> Execute(int requestId)
        {
            var repository = Uow.GetRepository<IPCRequest>();

            return repository.AsQueryable()
                .Where(i => i.RequestId == requestId)
                .ToList();
        }
    }
}
