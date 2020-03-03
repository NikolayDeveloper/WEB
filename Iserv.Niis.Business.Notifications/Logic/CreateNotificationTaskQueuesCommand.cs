using System.Collections.Generic;
using Iserv.Niis.Domain.Entities.Notification;
using NetCoreCQRS.Commands;

namespace Iserv.Niis.Notifications.Logic
{
    public class CreateNotificationTaskQueuesCommand : BaseCommand
    {
        public void Execute(IEnumerable<NotificationTaskQueue> notificationTaskQueues)
        {
            var repository = Uow.GetRepository<NotificationTaskQueue>();

            repository.CreateRange(notificationTaskQueues);

            Uow.SaveChanges();
        }
    }
}
