using System;
using System.Linq;
using Iserv.Niis.Notifications.Resources;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Request;

namespace Iserv.Niis.Notifications.Implementations.Notifications.Requests
{
    public class RecoveryStage : INotificationMessageRequirement<Request>
    {
        public string Message
        {
            get
            {
                var calendarProvider = NiisAmbientContext.Current.CalendarProvider;
                var date = calendarProvider.GetExecutionDate(DateTimeOffset.Now, CurrentRequestObject.CurrentWorkflow.CurrentStage.ExpirationType, (short)CurrentRequestObject.CurrentWorkflow.CurrentStage.ExpirationValue);

                return string.Format(MessageTemplates.RecoveryStage, CurrentRequestObject.RequestNum, date.ToString("dd/MM/yyyy"));
            }
        }

        public DateTimeOffset NotificationDate => NiisAmbientContext.Current.DateTimeProvider.Now;

        public Request CurrentRequestObject { get; set; }
        
        public bool IsPassesRequirements()
        {
            var routeStageCodes = new[]
            {
                RouteStageCodes.I_03_3_1_1_1,
                RouteStageCodes.UM_03_3_1,
                RouteStageCodes.TZ_03_3_7_4,
                RouteStageCodes.NMPT_03_2_3,
            };

            return routeStageCodes.Contains(CurrentRequestObject.CurrentWorkflow.CurrentStage.Code);
        }
    }
}
