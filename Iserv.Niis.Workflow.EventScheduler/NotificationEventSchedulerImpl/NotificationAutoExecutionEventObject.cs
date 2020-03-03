using System;
using System.Threading;
using System.Threading.Tasks;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Notification;
using Iserv.Niis.Notifications.Implementations;
using Iserv.Niis.Notifications.Models;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowTaskQueues;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Workflow.EventScheduler.NotificationEventSchedulerImpl
{
    public class NotificationAutoExecutionEventObject
    {
        private readonly IExecutor _executor;
        private readonly CancellationTokenSource _workflowEventCancellationToken;

        public NotificationAutoExecutionEventObject(IExecutor executor)
        {
            _executor = executor;
        }
        
        public string EventName { get; private set; }
        public Action<NotificationTaskQueue> EventAction { get; private set; }
        public NotificationTaskQueue NotificationTaskEvent { get; private set; }

        public NotificationAutoExecutionEventObject()
        {
            _executor = NiisAmbientContext.Current.Executor;
            _workflowEventCancellationToken = new CancellationTokenSource();
        }

        //Todo! Логирование ошибок при отправке
        public void StartEventExecution()
        {
            Task.Run(async delegate
            {
                WriteLogMessage($"Init {EventName}");

                var delayTime = NotificationTaskEvent.ResolveDate - NiisAmbientContext.Current.DateTimeProvider.Now;
                if (delayTime > TimeSpan.Zero)
                {
                    await Task.Delay(delayTime, _workflowEventCancellationToken.Token);
                }

                EventAction?.Invoke(NotificationTaskEvent);

                NotificationAutoEvents.RemoveEvent(EventName);

                WriteLogMessage($"test {EventName}");
            }, _workflowEventCancellationToken.Token);

            WriteLogMessage($"new event start {EventName}");
        }

        public void StopEventExecution()
        {
            WriteLogMessage($"new event stop {EventName}");
            _workflowEventCancellationToken.Cancel();
        }

        public static NotificationAutoExecutionEventObject ConstructFrom(NotificationTaskQueue notificationTaskQueue)
        {
            var notificationEventObject = new NotificationAutoExecutionEventObject
            {
                EventName = notificationTaskQueue.WorkflowEventKey,
                NotificationTaskEvent = notificationTaskQueue,
                EventAction = SendMessage
            };

            return notificationEventObject;
        }

        private static void SendMessage(NotificationTaskQueue notificationTaskQueue)
        {
            var credentials = new NotificationsCredentials
            {
                EmailFrom = Properties.Settings.Default.EmailFrom,
                EmailPassword = Properties.Settings.Default.EmailPassword,
                SmtpPort = Properties.Settings.Default.SmtpPort,
                SmtpServer = Properties.Settings.Default.SmtpServer,
                SmscLogin = Properties.Settings.Default.SmscLogin,
                SmscPassword = Properties.Settings.Default.SmscPassword
            };
            new NotificationSender().SendNotificationByNotificationTaskQueueId(notificationTaskQueue.Id, credentials);
        }
        
        private static void WriteLogMessage(string logMessage)
        {
            Console.WriteLine(logMessage);
        }
    }
}
