using System.Collections.Concurrent;

namespace Iserv.Niis.Workflow.EventScheduler.NotificationEventSchedulerImpl
{
    public static class NotificationAutoEvents
    {
        private static readonly ConcurrentDictionary<string, NotificationAutoExecutionEventObject> AutoExecutionEvents;

        static NotificationAutoEvents()
        {
            AutoExecutionEvents = new ConcurrentDictionary<string, NotificationAutoExecutionEventObject>();
        }

        public static void AddEvent(NotificationAutoExecutionEventObject autoExecutionEventObject)
        {
            AutoExecutionEvents.GetOrAdd(autoExecutionEventObject.EventName, autoExecutionEventObject);
            autoExecutionEventObject.StartEventExecution();
        }

        public static void RemoveEvent(string eventName)
        {
            if (AutoExecutionEvents.TryGetValue(eventName, out var autoExecutionEventObject) == false)
            {
                return;
            }
            autoExecutionEventObject.StopEventExecution();
            AutoExecutionEvents.TryRemove(eventName, out _);
        }

        public static void StopAllEvents()
        {
            foreach (var autoExecutionEventObject in AutoExecutionEvents)
            {
                autoExecutionEventObject.Value.StopEventExecution();
            }
        }
    }
}
