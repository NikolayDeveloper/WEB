using System.Linq;
using Iserv.Niis.Domain.Entities.Notification;
using Microsoft.EntityFrameworkCore;
using NetCoreCQRS.Queries;

namespace Iserv.Niis.Notifications.Logic
{
    public class GetNotificationTaskQueueByIdQuery : BaseQuery
    {
        public NotificationTaskQueue Execute(int id)
        {
            var notificationTaskQueueRepository = Uow.GetRepository<NotificationTaskQueue>();

            var notificationTaskQueue = notificationTaskQueueRepository.AsQueryable()
                .Where(r => r.Id == id)
                .Include(q => q.Request)
                .Include(q => q.Contract)
                .Include(q => q.Document).ThenInclude(d => d.Workflows).ThenInclude(cw => cw.CurrentStage)
                .Include(q => q.DicCustomer)
                .ThenInclude(dc => dc.ContactInfos)
                .ThenInclude(ci => ci.Type)
                .Include(q => q.ProtectionDoc).ThenInclude(d => d.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
                .IgnoreQueryFilters()
                .FirstOrDefault();

            return notificationTaskQueue;
        }
    }
}
