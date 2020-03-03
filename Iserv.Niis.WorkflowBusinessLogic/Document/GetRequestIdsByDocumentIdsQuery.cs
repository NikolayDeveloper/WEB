using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.WorkflowBusinessLogic.Document
{
    public class GetRequestIdsByDocumentIdsQuery : BaseQuery
    {
        public List<int> Execute(List<int> documentIds)
        {
            var repo = Uow.GetRepository<RequestDocument>();

            var requestIds = repo.AsQueryable()
                .Where(rd => documentIds.Contains(rd.DocumentId))
                .Select(rd => rd.Request.Id)
                .ToList();

            return requestIds;
        }
    }
}
