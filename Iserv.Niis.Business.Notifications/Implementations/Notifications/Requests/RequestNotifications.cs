using Iserv.Niis.Domain.Entities.Request;
using System.Collections.Generic;

namespace Iserv.Niis.Notifications.Implementations.Notifications.Requests
{
    public class RequestNotifications : BaseNotification<Request>
    {
        internal override List<INotificationMessageRequirement<Request>> NotificationRequirements
        {
            get
            {
                return new List<INotificationMessageRequirement<Request>>
                {
                    new RecoveryStage(),
                    new PreparationForTransferStage(),
                    new OnlineStatusChanged(),
                    new ExpertisePaymentWaitingStage(),
                    new PaymentWaitingStage()
                };
            }
            set { }
        }
    }

}
