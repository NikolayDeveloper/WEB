using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.WorkflowBusinessLogic.Document
{
    public class GetProtectionDocIdsByDocumentIdsQuery : BaseQuery
    {
        public List<int> Execute(List<int> documentIds)
        {
            var repo = Uow.GetRepository<ProtectionDocDocument>();

            var requestIds = repo.AsQueryable()
                .Where(rd => documentIds.Contains(rd.DocumentId))
                .Select(rd => rd.ProtectionDoc.Id)
                .ToList();

            return requestIds;
        }
    }
}
