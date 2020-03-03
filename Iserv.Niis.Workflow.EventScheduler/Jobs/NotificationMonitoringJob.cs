using Iserv.Niis.DI;
using Iserv.Niis.Workflow.EventScheduler.NotificationEventSchedulerImpl;
using Iserv.Niis.WorkflowBusinessLogic.NotificationStatusQueue;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using Quartz;

namespace Iserv.Niis.Workflow.EventScheduler.Jobs
{
    public class NotificationMonitoringJob : IJob
    {
        private readonly IExecutor _executor;

        public NotificationMonitoringJob()
        {
            _executor = NiisAmbientContext.Current.Executor;
        }

        public void Execute(IJobExecutionContext context)
        {
            var requestEventsStartDate = NiisAmbientContext.Current.DateTimeProvider.NowStartDateTime;
            var requestEventsEndDate = NiisAmbientContext.Current.DateTimeProvider.NowEndDateTime;

            var notificationEvents = _executor.GetQuery<NotificationTaskQueueQuery>()
                                                        .Process(q => q.Execute(requestEventsStartDate, requestEventsEndDate));

            foreach (var notificationEvent in notificationEvents)
            {
                var requestAutoEvent = NotificationAutoExecutionEventObject.ConstructFrom(notificationEvent);
                NotificationAutoEvents.AddEvent(requestAutoEvent);
            }
        }
    }
}
