using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Linq;

namespace Iserv.Niis.WorkflowBusinessLogic.Document
{
    public class GetDocumentByIdQuery : BaseQuery
    {
        public Domain.Entities.Document.Document Execute(int documentId)
        {
            var repository = Uow.GetRepository<Domain.Entities.Document.Document>();

            return repository.AsQueryable()
                .Include(r => r.Workflows).ThenInclude(r => r.CurrentStage)
                .Include(r => r.Workflows).ThenInclude(r => r.FromStage)
                .Include(r => r.Workflows).ThenInclude(r => r.Route)
                .Include(r => r.Workflows).ThenInclude(r => r.DocumentUserSignature)
                .Include(r => r.Requests)
                .Include(r => r.ProtectionDocs).ThenInclude(pdd => pdd.ProtectionDoc).ThenInclude(pd => pd.Type)
                .Include(r => r.ProtectionDocs).ThenInclude(pdd => pdd.ProtectionDoc).ThenInclude(pd => pd.Bulletins).ThenInclude(b => b.Bulletin)
                .Include(r => r.Contracts)
                .Include(r => r.Type)
                .FirstOrDefault(r => r.Id == documentId);
        }
    }
}
