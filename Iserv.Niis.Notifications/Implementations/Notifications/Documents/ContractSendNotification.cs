using System;
using System.Linq;
using Iserv.Niis.Notifications.Resources;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Notifications.Logic;

namespace Iserv.Niis.Notifications.Implementations.Notifications.Documents
{
    public class ContractSendNotification : INotificationMessageRequirement<Document>
    {
        private ContractDocument _contractDocument;
        public Document CurrentRequestObject { get; set; }

        public string Message
        {
            get
            {
                return string.Format(MessageTemplates.ContractSendNotification, _contractDocument?.Contract?.ContractNum ?? string.Empty);
            }
        }

        public DateTimeOffset NotificationDate => NiisAmbientContext.Current.DateTimeProvider.Now;

        public bool IsPassesRequirements()
        {
            _contractDocument = NiisAmbientContext.Current.Executor.GetQuery<GetContractDocumentByDocumentIdQuery>().Process(q => q.Execute(CurrentRequestObject.Id));

            if (_contractDocument == null) return false;

            var onlineStatusCodes = new[]
            {
                DicOnlineRequisitionStatus.Codes.PendingPayment,
                DicOnlineRequisitionStatus.Codes.RegistrationRefusal,
                DicOnlineRequisitionStatus.Codes.AwaitingResponse,
                DicOnlineRequisitionStatus.Codes.WithdrawnDiscontinued,
                DicOnlineRequisitionStatus.Codes.Paused
            };

            var docTypeCodes = new[]
            {
                DicDocumentTypeCodes.NotificationOfGovernmentDuty,
                DicDocumentTypeCodes.Notification,
                DicDocumentTypeCodes.NotificationOfPausedWork,
                DicDocumentTypeCodes.NotificationOfTerminationWork,
            };

            if (CurrentRequestObject.CurrentWorkflows.Any(cwf => cwf.CurrentStage.Code == RouteStageCodes.DocumentOutgoing_03_1)
                && docTypeCodes.Contains(CurrentRequestObject.Type.Code)
                && onlineStatusCodes.Contains(_contractDocument.Contract.CurrentWorkflow.CurrentStage.OnlineRequisitionStatus.Code))
            {
                return true;
            }

            return false;
        }
    }
}
