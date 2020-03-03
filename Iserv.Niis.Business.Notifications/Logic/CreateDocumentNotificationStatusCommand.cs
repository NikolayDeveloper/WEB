using Iserv.Niis.Domain.Entities.Document;
using NetCoreCQRS.Commands;

namespace Iserv.Niis.Notifications.Logic
{
    public class CreateDocumentNotificationStatusCommand : BaseCommand
    {
        public void Execute(DocumentNotificationStatus documentNotificationStatus)
        {
            Uow.GetRepository<DocumentNotificationStatus>().Create(documentNotificationStatus);
            Uow.SaveChanges();
        }
    }
}
