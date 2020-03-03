using System;
using System.Linq;
using Iserv.Niis.Domain.Entities.Integration;
using Iserv.Niis.ExternalServices.Features.Constants;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelStatusSender.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelStatusSender.Helpers;
using Iserv.Niis.ExternalServices.Features.Utils;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelStatusSender.Implementations
{
    public class Session : ISession
    {
        private readonly IIntegrationDocumentService _documentService;
        private readonly KazPatentCabinetHelper _kazPatentCabinet;
        private readonly LoggingHelper _logging;
        private readonly PepHelper _pep;
        private readonly IIntegrationRequisitionService _requisitionService;
        private readonly IIntegrationStatusService _statusService;

        public Session(
            IIntegrationDocumentService documentService,
            IIntegrationStatusService statusService,
            IIntegrationRequisitionService requisitionService,
            KazPatentCabinetHelper kazPatentCabinet,
            PepHelper pep,
            LoggingHelper logging)
        {
            _documentService = documentService;
            _statusService = statusService;
            _requisitionService = requisitionService;
            _kazPatentCabinet = kazPatentCabinet;
            _pep = pep;
            _logging = logging;
        }

        public void PerformSendStatuses()
        {
            var statuses = _statusService.GetUnsentStatuses();
            if (!statuses.Any())
            {
                return;
            }

            foreach (var status in statuses)
            {
                _logging.CreateMonitorLog(false, $"Статус отправляется: {status}");
                SendStatus(status);
                _statusService.MarkSentStatus(status);
                _logging.CreateMonitorLog(false, $"Статус отправлен: {status}");
            }
        }

        public void PerformSendDocument()
        {
            var documents = _documentService.GetUnsentDocuments();
            if (!documents.Any())
            {
                return;
            }

            foreach (var document in documents)
            {
                _logging.CreateMonitorLog(false, $"Документ отправляется: {document}");
                _kazPatentCabinet.SendDocument(document);
                _documentService.MarkSentDocument(document.Id);
                _logging.CreateMonitorLog(false, $"Документ отправлен: {document}");
            }
        }

        #region PrivateMethods

        private void SendStatus(IntegrationStatus status)
        {
            var requisition = _requisitionService.GetRequisition(status.RequestBarcode);
            if (requisition == null)
            {
                _statusService.SetNoteStatus(status, "Связанная с статусом заявка не найдена");
                return;
            }

            //if (CommonConstants.SenderPep.Equals(requisition.Sender, StringComparison.CurrentCultureIgnoreCase))
            //{
            //    _pep.SendStatus(status, requisition);
            //}
            //else 
            if (CommonConstants.SenderKazPatent.Equals(requisition.Sender, StringComparison.CurrentCultureIgnoreCase))
            {
                _kazPatentCabinet.SendStatus(status, requisition);
            }
            else
            {
                throw new InvalidOperationException($"Отправитель {requisition.Sender} неизвестен!");
            }
        }

        #endregion
    }
}