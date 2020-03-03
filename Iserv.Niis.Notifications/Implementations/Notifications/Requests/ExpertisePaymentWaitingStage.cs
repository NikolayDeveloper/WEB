using System;
using System.Linq;
using Iserv.Niis.Notifications.Resources;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.Request;

namespace Iserv.Niis.Notifications.Implementations.Notifications.Requests
{
    public class ExpertisePaymentWaitingStage : INotificationMessageRequirement<Request>
    {
        public Request CurrentRequestObject { get; set; }

        public string Message
        {
            get
            {
                var calendarProvider = NiisAmbientContext.Current.CalendarProvider;
                var date = calendarProvider.GetExecutionDate(DateTimeOffset.Now, CurrentRequestObject.CurrentWorkflow.CurrentStage.ExpirationType, (short)CurrentRequestObject.CurrentWorkflow.CurrentStage.ExpirationValue);

                return string.Format(MessageTemplates.ExpertisePaymentWaitingStage, CurrentRequestObject.RequestNum, date.ToString("dd/MM/yyyy"));
            }
        }

        public DateTimeOffset NotificationDate => NiisAmbientContext.Current.DateTimeProvider.Now;

        public bool IsPassesRequirements()
        {
            var routeStages = new[]
            {
                RouteStageCodes.I_02_2_0,
                RouteStageCodes.PO_03_2_02,
                RouteStageCodes.TZ_03_2_2_0,
            };

            return CurrentRequestObject.CurrentWorkflow.CurrentStage.OnlineRequisitionStatus.Code == DicOnlineRequisitionStatus.Codes.PendingPayment
                    && routeStages.Contains(CurrentRequestObject.CurrentWorkflow.CurrentStage.Code);
        }
    }
}
