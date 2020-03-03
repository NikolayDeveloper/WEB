using Iserv.Niis.Domain.Entities.Document;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Linq;

namespace Iserv.Niis.BusinessLogic.Documents
{
    public class GetDocumentWithIncludesByDocumentIdQuery : BaseQuery
    {
        public Document Execute(int documentId)
        {
            var repository = Uow.GetRepository<Document>();
            var document = repository.AsQueryable()
                    .Include(d => d.Addressee)
                    .Include(d => d.Status)
                    .Include(d => d.Workflows).ThenInclude(w => w.FromStage)
                    .Include(d => d.Workflows).ThenInclude(w => w.CurrentStage)
                    .Include(d => d.Workflows).ThenInclude(w => w.FromUser)
                    .Include(d => d.Workflows).ThenInclude(w => w.CurrentUser)
                    .Include(d => d.Workflows).ThenInclude(w => w.Route)
                    .Include(d => d.Workflows).ThenInclude(w => w.DocumentUserSignature)
                    .Include(d => d.PaymentInvoice).ThenInclude(w => w.Tariff)
                    .Include(d => d.IncomingAnswer).ThenInclude(d => d.Type)
                    .Include(d => d.Comments)
                    .Include(d => d.MainAttachment)
                    .Include(d => d.AdditionalAttachments)
                    .Include(d => d.Requests)
                    .Include(d => d.Contracts)
                    .Include(d => d.ProtectionDocs)
                    .Include(d => d.Type)
                    .Include(d => d.DocumentLinks).ThenInclude(d => d.ParentDocument).ThenInclude(d => d.Type)
                    .Include(d => d.DocumentLinks).ThenInclude(d => d.ChildDocument).ThenInclude(d => d.Type)
                    .Include(d => d.DocumentParentLinks).ThenInclude(d => d.ParentDocument).ThenInclude(d => d.Type)
                    .Include(d => d.DocumentParentLinks).ThenInclude(d => d.ChildDocument).ThenInclude(d => d.Type)
                    .FirstOrDefault(r => r.Id == documentId);

            return document;
        }
    }
}
