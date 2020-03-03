using System;
using System.Linq;
using Iserv.Niis.Notifications.Resources;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.Request;

namespace Iserv.Niis.Notifications.Implementations.Notifications.Requests
{
    class OnlineStatusChanged : INotificationMessageRequirement<Request>
    {
        public Request CurrentRequestObject { get; set; }
        
        public string Message
        {
            get
            {
                var calendarProvider = NiisAmbientContext.Current.CalendarProvider;
                var date = calendarProvider.GetExecutionDate(DateTimeOffset.Now, CurrentRequestObject.CurrentWorkflow.CurrentStage.ExpirationType, (short)CurrentRequestObject.CurrentWorkflow.CurrentStage.ExpirationValue);

                return string.Format(MessageTemplates.OnlineStatusChanged, CurrentRequestObject.RequestNum, CurrentRequestObject.CurrentWorkflow.CurrentStage.OnlineRequisitionStatus.NameRu);
            }
        }

        public DateTimeOffset NotificationDate => NiisAmbientContext.Current.DateTimeProvider.Now;

        public bool IsPassesRequirements()
        {
            var statusCodes = new[]
            {
                DicOnlineRequisitionStatus.Codes.AwaitingResponse, DicOnlineRequisitionStatus.Codes.Paused,
                DicOnlineRequisitionStatus.Codes.PendingPayment, DicOnlineRequisitionStatus.Codes.Registered,
                DicOnlineRequisitionStatus.Codes.RegistrationRefusal,
                DicOnlineRequisitionStatus.Codes.WithdrawnDiscontinued

            };

            return CurrentRequestObject.CurrentWorkflow.FromStage.OnlineRequisitionStatus.Code
                    != CurrentRequestObject.CurrentWorkflow.CurrentStage.OnlineRequisitionStatus.Code
                    && statusCodes.Contains(CurrentRequestObject.CurrentWorkflow.CurrentStage.OnlineRequisitionStatus.Code);
        }
    }
}
