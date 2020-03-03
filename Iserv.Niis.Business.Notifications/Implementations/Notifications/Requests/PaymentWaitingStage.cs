using System;
using System.Linq;
using Iserv.Niis.Notifications.Resources;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.Request;

namespace Iserv.Niis.Notifications.Implementations.Notifications.Requests
{
    public class PaymentWaitingStage : INotificationMessageRequirement<Request>
    {
        public Request CurrentRequestObject { get; set; }

        public string Message
        {
            get
            {
                var calendarProvider = NiisAmbientContext.Current.CalendarProvider;
                var date = calendarProvider.GetExecutionDate(DateTimeOffset.Now, CurrentRequestObject.CurrentWorkflow.CurrentStage.ExpirationType, (short)CurrentRequestObject.CurrentWorkflow.CurrentStage.ExpirationValue);

                return string.Format(MessageTemplates.PaymentWaitingStage, CurrentRequestObject.RequestNum, date.ToString("dd/MM/yyyy"));
            }
        }

        public DateTimeOffset NotificationDate => NiisAmbientContext.Current.DateTimeProvider.Now;

        public bool IsPassesRequirements()
        {
            var routeStages = new[]
            {
                RouteStageCodes.PO_02_2_0,
                RouteStageCodes.I_02_2_0,
                RouteStageCodes.SA_02_2_1,
                RouteStageCodes.UM_02_2_7,
            };

            if (CurrentRequestObject.CurrentWorkflow.CurrentStage.OnlineRequisitionStatus.Code == DicOnlineRequisitionStatus.Codes.PendingPayment
                && routeStages.Contains(CurrentRequestObject.CurrentWorkflow.CurrentStage.Code))
            {
                return true;
            }

            return false;
        }
    }
}
