using Iserv.Niis.Domain.Entities.Notification;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Iserv.Niis.WorkflowBusinessLogic.NotificationStatusQueue
{
    public class NotificationTaskQueueQuery : BaseQuery
    {
        public List<NotificationTaskQueue> Execute(DateTimeOffset eventResolveStartDate, DateTimeOffset eventResolveEndDate)
        {
            var notificationTaskQueueRepository = Uow.GetRepository<NotificationTaskQueue>();
            var workflowTaskQueues = notificationTaskQueueRepository
                .AsQueryable()
                .IgnoreQueryFilters()
                .Where(IsRequestEventsResolveDateToday(eventResolveStartDate, eventResolveEndDate))
                .Where(r => r.IsExecuted != true)
                .ToList();

            return workflowTaskQueues;
        }

        private static Expression<Func<NotificationTaskQueue, bool>> IsRequestEventsResolveDateToday(DateTimeOffset eventResolveStartDate, DateTimeOffset eventResolveEndDate)
        {
            return queue => queue.ResolveDate >= eventResolveStartDate && queue.ResolveDate <= eventResolveEndDate && (queue.IsDocument || queue.IsRequest || queue.IsContract || queue.IsProtectionDoс);
        }
    }
}
