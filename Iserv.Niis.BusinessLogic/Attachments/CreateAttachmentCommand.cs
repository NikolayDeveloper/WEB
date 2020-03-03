using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using Iserv.Niis.Domain.Entities.Document;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Attachments
{
    public class CreateAttachmentCommand : BaseCommand
    {
        public async Task ExecuteAsync(Attachment attachment)
        {
            var repo = Uow.GetRepository<Attachment>();
            await repo.CreateAsync(attachment);

            await Uow.SaveChangesAsync();
        }
    }
}
