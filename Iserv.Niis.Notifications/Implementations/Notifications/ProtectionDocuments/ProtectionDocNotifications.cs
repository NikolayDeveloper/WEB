using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ProtectionDoc;

namespace Iserv.Niis.Notifications.Implementations.Notifications.ProtectionDocuments
{
    public class ProtectionDocNotifications: BaseNotification<ProtectionDoc>
    {
        internal override List<INotificationMessageRequirement<ProtectionDoc>> NotificationRequirements
        {
            get
            {
                return new List<INotificationMessageRequirement<ProtectionDoc>>
                {
                    new ProtectionDocValidityAboutToExpire(),
                    new ProtectionDocSupportAboutToExpire()
                };
            }
            set { }
        }
    }
}
