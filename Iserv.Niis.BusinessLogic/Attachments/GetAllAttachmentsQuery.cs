using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using Iserv.Niis.Domain.Entities.Document;

namespace Iserv.Niis.BusinessLogic.Attachments
{
    public class GetAllAttachmentsQuery : BaseQuery
    {
        public async Task<List<Attachment>> ExecuteAsync(int documentId)
        {
            var repo = Uow.GetRepository<Attachment>();
            var attachments = await repo.AsQueryable()
                .Where(r => r.DocumentId == documentId)
                .ToListAsync();

            return attachments;
        }
    }
}
