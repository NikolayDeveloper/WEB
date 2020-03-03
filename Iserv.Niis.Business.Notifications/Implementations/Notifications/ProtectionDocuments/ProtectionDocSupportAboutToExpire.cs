using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Notifications.Resources;

namespace Iserv.Niis.Notifications.Implementations.Notifications.ProtectionDocuments
{
    public class ProtectionDocSupportAboutToExpire : INotificationMessageRequirement<ProtectionDoc>
    {
        public ProtectionDoc CurrentRequestObject { get; set; }

        public DateTimeOffset NotificationDate
        {
            get
            {
                var now = NiisAmbientContext.Current.DateTimeProvider.Now;
                return new DateTimeOffset(now.Year, now.Month, now.Day, 9, 0, 0, TimeZoneInfo.Utc.GetUtcOffset(now));
            }
        }

        public string Message => string.Format(MessageTemplates.ProtectionDocSupportAboutToExpire, CurrentRequestObject.GosNumber);

        public bool IsPassesRequirements()
        {
            return CurrentRequestObject.MaintainDate?.AddMonths(1) > NiisAmbientContext.Current.DateTimeProvider.Now;
        }
    }
}
