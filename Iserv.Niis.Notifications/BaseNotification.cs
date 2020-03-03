using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Notification;
using System.Collections.Generic;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Entities.ProtectionDoc;

namespace Iserv.Niis.Notifications
{
    public abstract class BaseNotification<TRequestObject>
        where TRequestObject : Entity<int>, new()
    {
        public List<NotificationTaskQueue> GenerateNotificationTaskQueue(TRequestObject requestObject)
        {
            if (NotificationRequirements == null) return null;

            var notificationTaskQueues = new List<NotificationTaskQueue>();

            foreach (var notificationRequirement in NotificationRequirements)
            {
                notificationRequirement.CurrentRequestObject = requestObject;

                var ispassed = notificationRequirement.IsPassesRequirements();
                if (ispassed)
                {
                    var notificationTaskQueue = new NotificationTaskQueue { IsSms = true };

                    if (notificationRequirement.CurrentRequestObject is Request)
                    {
                        notificationTaskQueue.RequestId = requestObject.Id;
                    }
                    if (notificationRequirement.CurrentRequestObject is ProtectionDoc)
                    {
                        notificationTaskQueue.ProtectionDocId = requestObject.Id;
                    }
                    if (notificationRequirement.CurrentRequestObject is Contract)
                    {
                        notificationTaskQueue.ContractId = requestObject.Id;
                    }
                    else if (notificationRequirement.CurrentRequestObject is Domain.Entities.Document.Document)
                    {
                        notificationTaskQueue.DocumentId = requestObject.Id;
                    }

                    if (notificationRequirement is IEmailNotificationRequirement)
                    {
                        if (((IEmailNotificationRequirement)notificationRequirement).IsEmail)
                        {
                            notificationTaskQueue.IsSms = false;
                            notificationTaskQueue.Attachment = ((IEmailNotificationRequirement)notificationRequirement).Attachment;
                        }
                        notificationTaskQueue.Subject = ((IEmailNotificationRequirement)notificationRequirement).Subject;
                    }

                    var receiverIdValue = requestObject.GetType().GetProperty("AddresseeId").GetValue(requestObject);
                    if (receiverIdValue != null)
                    {
                        int.TryParse(receiverIdValue.ToString(), out int receiverId);
                        notificationTaskQueue.DicCustomerId = receiverId;
                    }

                    notificationTaskQueue.Message = notificationRequirement.Message;
                    notificationTaskQueue.ResolveDate = notificationRequirement.NotificationDate;

                    notificationTaskQueues.Add(notificationTaskQueue);
                }
            }

            return notificationTaskQueues;
        }

        internal abstract List<INotificationMessageRequirement<TRequestObject>> NotificationRequirements { get; set; }
    }
}
