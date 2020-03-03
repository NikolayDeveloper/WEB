using System.Collections.Generic;
using Iserv.Niis.Domain.Entities.Contract;

namespace Iserv.Niis.Notifications.Implementations.Notifications.Contracts
{
    public class ContractNotifications : BaseNotification<Contract>
    {
        internal override List<INotificationMessageRequirement<Contract>> NotificationRequirements
        {
            get
            {
                return new List<INotificationMessageRequirement<Contract>>
                {
                    new ContractOnlineStatusChanged()
                };
            }
            set { }
        }
    }
}
