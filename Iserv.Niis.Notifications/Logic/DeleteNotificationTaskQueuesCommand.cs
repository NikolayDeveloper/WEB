using System.Collections.Generic;
using Iserv.Niis.Domain.Entities.Notification;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

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
