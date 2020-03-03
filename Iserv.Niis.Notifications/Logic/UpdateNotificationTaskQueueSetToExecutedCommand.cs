using Iserv.Niis.Domain.Entities.Notification;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using Iserv.Niis.Exceptions;

namespace Iserv.Niis.Notifications.Logic
{
    public class UpdateNotificationTaskQueueSetToExecutedCommand: BaseCommand
    {
        public NotificationTaskQueue Execute(int taskId)
        {
            var repo = Uow.GetRepository<NotificationTaskQueue>();
            var queue = repo.GetById(taskId);

            if (queue == null)
                throw new DataNotFoundException(nameof(NotificationTaskQueue), DataNotFoundException.OperationType.Read, taskId);

            queue.IsExecuted = true;
            Uow.SaveChanges();

            return null;
        }
    }
}
