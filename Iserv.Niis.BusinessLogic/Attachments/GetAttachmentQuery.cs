using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using Iserv.Niis.Domain.Entities.Document;

namespace Iserv.Niis.BusinessLogic.Attachments
{
    public class GetAttachmentQuery : BaseQuery
    {
        public async Task<Attachment> ExecuteAsync(int attachmentId)
        {
            var repo = Uow.GetRepository<Attachment>();
            var attachment = await repo.AsQueryable()
                .Where(r => r.Id == attachmentId)
                .FirstOrDefaultAsync();

            return attachment;
        }
    }
}
