using System.Linq;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Notifications.Helpers;
using Iserv.Niis.Notifications.Logic;
using Iserv.Niis.Notifications.Models;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using Iserv.Niis.DI;

namespace Iserv.Niis.Notifications.Implementations
{
    public class NotificationSender
    {
        private readonly IExecutor _executor;

        public NotificationSender()
        {
            _executor = NiisAmbientContext.Current.Executor;
        }

        public void SendNotificationByNotificationTaskQueueId(int notificationTaskQueueId, NotificationsCredentials credentials)
        {
            var notificationTaskQueue = _executor.GetQuery<GetNotificationTaskQueueByIdQuery>().Process(q => q.Execute(notificationTaskQueueId));

            if (notificationTaskQueue != null)
            {
                if (notificationTaskQueue.ConditionStageId == null || notificationTaskQueue.Document.CurrentWorkflows.Any(d => d.CurrentStage.Code.Equals(notificationTaskQueue.ConditionStage.Code)))
                //if (notificationTaskQueue.ConditionStageId == null || notificationTaskQueue.ConditionStage.Code.Equals(notificationTaskQueue.Document.CurrentWorkflow.CurrentStage.Code))
                {
                    var sendModel = new SendModel
                    {
                        IsSms = notificationTaskQueue.IsSms,
                        EmailAddresses = notificationTaskQueue.DicCustomer.ContactInfos
                            .Where(ci => ci.Type.Code == DicContactInfoType.Codes.Email)
                            .Select(ci => ci.Info)
                            .ToList(),
                        MobilePhones = notificationTaskQueue.DicCustomer.ContactInfos
                            .Where(ci => ci.Type.Code == DicContactInfoType.Codes.MobilePhone)
                            .Select(ci => ci.Info)
                            .ToList(),
                        Message = notificationTaskQueue.Message,
                        Subject = notificationTaskQueue.Subject,
                        Attachment = notificationTaskQueue.Attachment,
                        Credentials = credentials
                    };

                    var statusCode = new SendHelper().Send(sendModel);

                    // TODO: Если успешно отправлено, ставить Executed, если нет, то обновить дату отправки сообщения.
                    _executor.GetHandler<UpdateNotificationStatusHandler>().Process<int>(h => h.Handle(notificationTaskQueue, statusCode));
                }
            }
        }
    }
}
