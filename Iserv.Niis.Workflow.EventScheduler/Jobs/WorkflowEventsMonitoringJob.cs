using Iserv.Niis.DI;
using Iserv.Niis.Workflow.EventScheduler.WorkflowEventSchedulerImpl;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowTaskQueues;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using Quartz;

namespace Iserv.Niis.Workflow.EventScheduler.Jobs
{
    /// <summary>
    /// Класс, который представляет постоянно повторяющуюся работу, связанную с выполнением запланированных задач. 
    /// </summary>
    public class WorkflowEventsMonitoringJob : IJob
    {
        private readonly IExecutor _executor;

        public WorkflowEventsMonitoringJob()
        {
            _executor = NiisAmbientContext.Current.Executor;
        }

        /// <summary>
        /// Достает запланированные задачи, у которых близится время выполнения, и просроченные задачи и помещает их в очередь выполнения задач.
        /// </summary>
        /// <param name="context">Информация о системе, которая предоставляется библотекой Quartz.</param>
        public void Execute(IJobExecutionContext context)
        {
            var startOfDay = NiisAmbientContext.Current.DateTimeProvider.NowStartDateTime;
            var endOfDay = NiisAmbientContext.Current.DateTimeProvider.NowEndDateTime;

            var requestWorkflowQueueEvents = _executor
                                                .GetQuery<GetWorkflowQueueByPeriodQuery>()
                                                .Process(q => q.Execute(startOfDay, endOfDay));

            foreach (var requestWorkflowQueueEvent in requestWorkflowQueueEvents)
            {
                var requestAutoEvent = WorkflowAutoExecutionEventObject.ConstructFrom(requestWorkflowQueueEvent);

                if (!WorkflowAutoEvents.ContainsEvent(requestAutoEvent))
                {
                    WorkflowAutoEvents.AddEvent(requestAutoEvent);
                }
            }
        }
    }
}
