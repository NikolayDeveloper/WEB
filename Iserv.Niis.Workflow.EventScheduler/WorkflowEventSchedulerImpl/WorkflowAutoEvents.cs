using System.Collections.Concurrent;
using System.Linq;

namespace Iserv.Niis.Workflow.EventScheduler.WorkflowEventSchedulerImpl
{
    /// <summary>
    /// Статичекский класс, который хранит, запускает и останавливает выполняемые задачи. Потокобезопасен.
    /// </summary>
    public static class WorkflowAutoEvents
    {
        /// <summary>
        /// Потокобезопасный словарь, в котором хранятся выполняемые задачи по их именам.
        /// </summary>
        private static readonly ConcurrentDictionary<string, WorkflowAutoExecutionEventObject> AutoExecutionEvents;

        static WorkflowAutoEvents()
        {
            AutoExecutionEvents = new ConcurrentDictionary<string, WorkflowAutoExecutionEventObject>();
        }

        /// <summary>
        /// Добавляет задачу в словарь задач и запускает ее.
        /// </summary>
        /// <param name="autoExecutionEventObject">Выполняемая задача.</param>
        public static void AddEvent(WorkflowAutoExecutionEventObject autoExecutionEventObject)
        {
            AutoExecutionEvents.GetOrAdd(autoExecutionEventObject.EventName, autoExecutionEventObject);
            autoExecutionEventObject.StartEventExecution();
        }
        
        /// <summary>
        /// Удаляет задачу из словаря и останавливает ее.
        /// </summary>
        /// <param name="eventName">Название задачи.</param>
        public static void RemoveEvent(string eventName)
        {
            if (AutoExecutionEvents.TryGetValue(eventName, out var autoExecutionEventObject) == false)
            {
                return;
            }
            autoExecutionEventObject.StopEventExecution();
            AutoExecutionEvents.TryRemove(eventName, out _);
        }

        /// <summary>
        /// Останавливает выполнение всех задач.
        /// </summary>
        public static void StopAllEvents()
        {
            foreach (var autoExecutionEventObject in AutoExecutionEvents)
            {
                autoExecutionEventObject.Value.StopEventExecution();
            }
        }

        /// <summary>
        /// Проверяет, есть ли в словаре указанная задача.
        /// </summary>
        /// <param name="eventObject">Выполняемая задача.</param>
        public static bool ContainsEvent(WorkflowAutoExecutionEventObject eventObject)
        {
            return AutoExecutionEvents.Select(a => a.Value.WorkflowTaskEvent)
                .Any(e => e.Id == eventObject.WorkflowTaskEvent.Id);
        }
    }
}
