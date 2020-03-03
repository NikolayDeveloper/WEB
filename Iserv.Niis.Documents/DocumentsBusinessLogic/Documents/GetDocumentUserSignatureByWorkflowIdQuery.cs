using Iserv.Niis.Domain.Entities.Document;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.Documents.DocumentsBusinessLogic.Documents
{
    public class GetDocumentUserSignatureByWorkflowIdQuery : BaseQuery
    {
        public IList<DocumentUserSignature> Execute(IList<int> wfIds)
        {
            var repository = Uow.GetRepository<DocumentUserSignature>();

            var result = repository.AsQueryable()
                .Include(d => d.User).ThenInclude(d => d.Position).ThenInclude(d => d.PositionType)
                .Where(r => wfIds.Contains(r.WorkflowId)).ToList();

            return result;
        }
    }
}
