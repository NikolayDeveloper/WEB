using System.Linq;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.Document
{
    public class GetDocumentUserInputByDocumentIdQuery : BaseQuery
    {
        public DocumentUserInput Execute(int documentId)
        {
            var repo = Uow.GetRepository<DocumentUserInput>();

            return repo.AsQueryable()
                .Where(d => d.DocumentId == documentId)
                .FirstOrDefault();
        }
    }
}
