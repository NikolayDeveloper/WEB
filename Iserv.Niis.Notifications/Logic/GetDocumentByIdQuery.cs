using System.Linq;
using Iserv.Niis.Domain.Entities.Document;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.Notifications.Logic
{
    public class GetDocumentByIdQuery : BaseQuery
    {
        public Document Execute(int documentId)
        {
            var repository = Uow.GetRepository<Document>();

            return repository.AsQueryable()
                .Where(r => r.Id == documentId)
                .Include(d => d.Type)
                .Include(d => d.Workflows)
                .ThenInclude(cw => cw.CurrentStage)
                .Include(d => d.Addressee)
                .ThenInclude(a => a.ContactInfos)
                .Include(d => d.Addressee)
                .ThenInclude(a => a.Type)
                .FirstOrDefault();
        }
    }
}
