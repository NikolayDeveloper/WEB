using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Notifications.Resources;

namespace Iserv.Niis.Notifications.Implementations.Notifications.ProtectionDocuments
{
    public class ProtectionDocValidityAboutToExpire: INotificationMessageRequirement<ProtectionDoc>
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

        public string Message => string.Format(MessageTemplates.ProtectionDocValidityAboutToExpire, CurrentRequestObject.GosNumber, CurrentRequestObject.ValidDate?.ToString("dd/MM/yyyy") ?? string.Empty);
        public bool IsPassesRequirements()
        {
            return CurrentRequestObject.ValidDate?.AddMonths(1) > NiisAmbientContext.Current.DateTimeProvider.Now;
        }
    }
}
