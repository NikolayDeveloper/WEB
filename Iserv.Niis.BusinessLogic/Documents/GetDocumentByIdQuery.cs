using Iserv.Niis.Domain.Entities.Document;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Documents
{
    public class GetDocumentByIdQuery : BaseQuery
    {
        public async Task<Document> ExecuteAsync(int documentId)
        {
            var repository = Uow.GetRepository<Document>();

            var result = repository.AsQueryable()
                .Include(r => r.Workflows)
                .Include(r => r.MainAttachment)
                .Include(r => r.AdditionalAttachments)
                .Include(r => r.Type)
                .Include(r => r.Requests).ThenInclude(d => d.Request).ThenInclude(d => d.CurrentWorkflow)
                .Include(r => r.Contracts).ThenInclude(d => d.Contract).ThenInclude(d => d.CurrentWorkflow)
                .Include(r => r.DocumentLinks).ThenInclude(d => d.ParentDocument).ThenInclude(d => d.Type)
                .Include(r => r.DocumentLinks).ThenInclude(d => d.ChildDocument).ThenInclude(d => d.Type)
                .Include(r => r.DocumentParentLinks).ThenInclude(d => d.ParentDocument).ThenInclude(d => d.Type)
                .Include(r => r.DocumentParentLinks).ThenInclude(d => d.ChildDocument).ThenInclude(d => d.Type)
                .Include(r => r.ProtectionDocs).ThenInclude(pdd => pdd.ProtectionDoc).ThenInclude(pd => pd.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
                .Include(r => r.ProtectionDocs).ThenInclude(pdd => pdd.ProtectionDoc).ThenInclude(d => d.CurrentWorkflow)
                .Include(r => r.ProtectionDocs).ThenInclude(pdd => pdd.ProtectionDoc).ThenInclude(pd => pd.Type)
                .Include(r => r.ProtectionDocs).ThenInclude(pdd => pdd.ProtectionDoc).ThenInclude(pd => pd.Bulletins).ThenInclude(pb => pb.Bulletin)
                .FirstOrDefaultAsync(r => r.Id == documentId);

            return await result;
        }
    }
}
