using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.Notification;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.Notifications.Logic
{
    internal class UpdateNotificationStatusHandler : BaseHandler
    {
        public void Handle(NotificationTaskQueue notificationTaskQueue, string notificationStatusCode)
        {
            if(notificationTaskQueue.IsRequest)
            {
                UpdateReqeuestNotificationStatus(notificationTaskQueue, notificationStatusCode);
            }
            if(notificationTaskQueue.IsDocument)
            {
                UpdateDocumentNotificationStatus(notificationTaskQueue, notificationStatusCode);
            }
        }

        /// <summary>
        /// ОБЯЗАТЕЛЬНО ОПТИМИЗИРОВАТЬ, убрать этукучу дублирующихся методов
        /// </summary>
        /// <param name="taskQueue"></param>
        /// <param name="notificationStatusCode"></param>
        private void UpdateDocumentNotificationStatus(NotificationTaskQueue taskQueue, string notificationStatusCode)
        {
            var status = Executor.GetQuery<GetNotificationStatusByCodeQuery>()
                .Process(q => q.Execute(notificationStatusCode));

            var documentStatus = taskQueue.IsSms ? Executor.GetQuery<GetDocumentSmsNotificationStatusQuery>().Process(c => c.Execute((int)taskQueue.DocumentId)) :
                                                   Executor.GetQuery<GetDocumentEmailNotificationStatusQuery>().Process(c => c.Execute((int)taskQueue.DocumentId));

            if (documentStatus != null)
                Executor.GetCommand<DeleteDocumentNotificationStatusCommand>().Process(c => c.Execute(documentStatus));

            Executor.GetCommand<CreateDocumentNotificationStatusCommand>().Process(c =>
                c.Execute(new DocumentNotificationStatus
                {
                    DocumentId = (int)taskQueue.DocumentId,
                    NotificationStatusId = status.Id
                }));

            Executor.GetCommand<UpdateNotificationTaskQueueSetToExecutedCommand>().Process(c => c.Execute(taskQueue.Id));
        }

        private void UpdateReqeuestNotificationStatus(NotificationTaskQueue taskQueue, string notificationStatusCode)
        {
            var status = Executor.GetQuery<GetNotificationStatusByCodeQuery>()
                .Process(q => q.Execute(notificationStatusCode));

            var requestStatus = Executor.GetQuery<GetRequestNotificationStatusQuery>().Process(q => q.Execute((int)taskQueue.RequestId));

            if (requestStatus != null)
                Executor.GetCommand<DeleteRequestNotificationStatusCommand>().Process(c => c.Execute(requestStatus));

            Executor.GetCommand<DeleteRequestNotificationStatusCommand>().Process(c => c.Execute(
                new RequestNotificationStatus
                {
                    RequestId = (int)taskQueue.RequestId,
                    NotificationStatusId = status.Id
                }
            ));

            Executor.GetCommand<UpdateNotificationTaskQueueSetToExecutedCommand>().Process(c => c.Execute(taskQueue.Id));
        }
    }
}
