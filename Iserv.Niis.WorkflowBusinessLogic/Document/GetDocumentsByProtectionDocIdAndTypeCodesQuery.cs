using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.Document
{
    public class GetDocumentsByProtectionDocIdAndTypeCodesQuery: BaseQuery
    {
        public List<Domain.Entities.Document.Document> Execute(int protectionDocId, string[] typeCodes)
        {
            var documents = Uow.GetRepository<Domain.Entities.Document.Document>()
                .AsQueryable()
                .Include(d => d.Workflows).ThenInclude(cw => cw.CurrentStage)
                .Where(d => d.ProtectionDocs.Any(r => r.ProtectionDocId == protectionDocId) && typeCodes.Contains(d.Type.Code))
                .ToList();

            return documents;
        }
    }
}
