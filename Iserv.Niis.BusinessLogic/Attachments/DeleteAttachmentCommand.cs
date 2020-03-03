using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Attachments
{
    public class DeleteAttachmentCommand : BaseCommand
    {
        public async Task ExecuteAsync(Attachment attachment)
        {
            var repo = Uow.GetRepository<Attachment>();
            var fetchedAttachment = repo.GetById(attachment.Id);
            repo.Delete(fetchedAttachment);

            await Uow.SaveChangesAsync();
        }
    }
}
