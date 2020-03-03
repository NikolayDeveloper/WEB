using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.Domain.Entities.Notification;
using NetCoreCQRS.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
