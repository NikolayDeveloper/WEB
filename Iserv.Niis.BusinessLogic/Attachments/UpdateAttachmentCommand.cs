using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Attachments
{
    public class UpdateAttachmentCommand: BaseCommand
    {
        public async Task ExecuteAsync(Attachment attachment)
        {
            var repo = Uow.GetRepository<Attachment>();
            repo.Update(attachment);

            await Uow.SaveChangesAsync();
        }
    }
}
