using System.Collections.Generic;
using Iserv.Niis.Domain.Entities.Notification;
using NetCoreCQRS.Commands;

namespace Iserv.Niis.Notifications.Logic
{
    public class DeleteNotificationTaskQueuesCommand : BaseCommand
    {
        public void Execute(IEnumerable<NotificationTaskQueue> taskQueues)
        {
            Uow.GetRepository<NotificationTaskQueue>().DeleteRange(taskQueues);
            Uow.SaveChanges();
        }
    }
}
