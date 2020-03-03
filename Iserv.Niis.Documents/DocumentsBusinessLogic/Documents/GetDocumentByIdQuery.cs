using System.Linq;
using Iserv.Niis.Domain.Entities.Document;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.Documents.DocumentsBusinessLogic.Documents
{
    public class GetDocumentByIdQuery: BaseQuery
    {
        public Document Execute(int documentId)
        {
            var repository = Uow.GetRepository<Document>();

            var result = repository.AsQueryable()
                .Include(d => d.Type)
                .Include(r => r.Addressee).ThenInclude(d => d.ContactInfos).ThenInclude(d => d.Type)
                .Include(d => d.ProtectionDocType)
                .Include(d => d.Workflows).ThenInclude(cw => cw.CurrentStage)
                .Include(d => d.Workflows).ThenInclude(cw => cw.CurrentUser).ThenInclude(cu => cu.Position).ThenInclude(u => u.PositionType)
                .Include(d => d.ProtectionDocs).ThenInclude(pd => pd.ProtectionDoc).ThenInclude(pd => pd.Addressee)
                .Include(d => d.Workflows).ThenInclude(w => w.CurrentStage)
                .Include(d => d.Workflows).ThenInclude(w => w.DocumentUserSignature).ThenInclude(s => s.User).ThenInclude(u => u.Position)
                .SingleOrDefault(r => r.Id == documentId);

            return result;
        }
    }
}
