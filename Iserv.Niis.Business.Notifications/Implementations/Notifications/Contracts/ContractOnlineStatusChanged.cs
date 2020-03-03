using System;
using System.Linq;
using Iserv.Niis.Notifications.Resources;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;

namespace Iserv.Niis.Notifications.Implementations.Notifications.Contracts
{
    public class ContractOnlineStatusChanged : INotificationMessageRequirement<Contract>
    {
        public Contract CurrentRequestObject { get; set; }

        public DateTimeOffset NotificationDate => NiisAmbientContext.Current.DateTimeProvider.Now;

        public string Message
        {
            get
            {

                return string.Format(MessageTemplates.ContractOnlineStatusChanged, CurrentRequestObject.ContractNum, CurrentRequestObject.CurrentWorkflow.CurrentStage.OnlineRequisitionStatus.NameRu);
            }
        }

        public bool IsPassesRequirements()
        {
            var onlineStatusCodes = new[]
            {
                DicOnlineRequisitionStatus.Codes.PendingPayment,
                DicOnlineRequisitionStatus.Codes.RegistrationRefusal,
                DicOnlineRequisitionStatus.Codes.WithdrawnDiscontinued,
                DicOnlineRequisitionStatus.Codes.Paused,
                DicOnlineRequisitionStatus.Codes.AwaitingResponse
            };

            return CurrentRequestObject.CurrentWorkflow.FromStage.OnlineRequisitionStatus.Code != CurrentRequestObject.CurrentWorkflow.CurrentStage.OnlineRequisitionStatus.Code
                && onlineStatusCodes.Contains(CurrentRequestObject.CurrentWorkflow.CurrentStage.OnlineRequisitionStatus.Code);
        }
    }
}
