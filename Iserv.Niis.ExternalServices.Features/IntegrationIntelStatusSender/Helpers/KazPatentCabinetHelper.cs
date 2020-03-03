using System;
using AutoMapper;
using Iserv.Niis.Domain.Entities.Integration;
using Iserv.Niis.ExternalServices.Domain.Entities;
using Iserv.Niis.ExternalServices.Features.Constants;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelStatusSender.External;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelStatusSender.Models;
using Iserv.Niis.ExternalServices.Features.Utils;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelStatusSender.Helpers
{
    public class KazPatentCabinetHelper
    {
        private const string ProjectTypeStatusSend = "StatusSendKazpatent";
        private const string ProjectTypeDocSend = "DocSendKazpatent";

        private readonly Configuration _configuration;
        private readonly LoggingHelper _logging;

        public KazPatentCabinetHelper(Configuration configuration, LoggingHelper logging)
        {
            _configuration = configuration;
            _logging = logging;
        }

        public void SendStatus(IntegrationStatus integrationStatus, IntegrationRequisition requisition)
        {
            var client = GetClientKazPatent(_configuration.KazPatentWebServiceUrl);

            var argument = new StatusSendArgument
            {
                SystemInfo = new SystemInfo
                {
                    ChainId = requisition.ChainId,
                    MessageDate = DateTime.Now,
                    MessageId = Guid.NewGuid().ToString(),
                    Sender = CommonConstants.SystemInfoSenderNiis
                },
                Status = new Status
                {
                    RowID = integrationStatus.Id,
                    DocumentID = integrationStatus.RequestBarcode,
                    StatusID = integrationStatus.OnlineRequisitionStatusId,
                    Note = integrationStatus.Note
                }
            };
            var logAction = new LogAction
            {
                DbDateTime = DateTimeOffset.Now,
                Project = CommonConstants.StatusSender,
                Type = ProjectTypeStatusSend,
                Note =
                    $"RequestBarcode = {integrationStatus.RequestBarcode} StatusId = {integrationStatus.OnlineRequisitionStatusId}",
                SystemInfoQueryId = _logging.CreateLogSystemInfo(Mapper.Map<LogSystemInfo>(argument.SystemInfo))
            };
            _logging.CreateLogAction(logAction);

            var result = client.StatusSend(argument);

            logAction.SystemInfoAnswerId = _logging.CreateLogSystemInfo(Mapper.Map<LogSystemInfo>(result.SystemInfo));
            _logging.UpdateLogAction(logAction);

            if (result.SystemInfo == null)
            {
                throw new Exception();
            }

            if (int.Parse(result.SystemInfo.Status.Code) < 0)
            {
                var text = $"KazPatent => URL:  {requisition.StatusURL}  => {result.SystemInfo.Status.MessageKz}";
                throw new Exception(text);
            }
        }

        public void SendDocument(IntegrationDocument integrationDocument)
        {
            var doc = new Doc
            {
                ID = integrationDocument.Id,
                DocUID = integrationDocument.DocumentBarcode,
                Note = integrationDocument.Note,
                File = integrationDocument.File,
                FileName = integrationDocument.FileName,
                DocumentType = integrationDocument.DocumentTypeId,
                InOutDate = integrationDocument.InOutDate?.Date,
                InOutNumber = integrationDocument.InOutNumber,
                OwnerDocUId = integrationDocument.RequestBarcode
            };
            var client = GetClientKazPatent(_configuration.KazPatentWebServiceUrl);
            var argument = new DocSendArgument
            {
                DocArray = new[] {doc},
                SystemInfo = new SystemInfo
                {
                    MessageDate = DateTime.Now,
                    MessageId = Guid.NewGuid().ToString(),
                    Sender = CommonConstants.SystemInfoSenderNiis
                }
            };
            var logAction = new LogAction
            {
                DbDateTime = DateTimeOffset.Now,
                Note = $"IntegrationDocumentId = {integrationDocument.Id}",
                Project = CommonConstants.StatusSender,
                Type = ProjectTypeDocSend,
                SystemInfoQueryId = _logging.CreateLogSystemInfo(Mapper.Map<LogSystemInfo>(argument.SystemInfo))
            };
            _logging.CreateLogAction(logAction);

            var result = client.DocSend(argument);

            logAction.SystemInfoAnswerId = _logging.CreateLogSystemInfo(Mapper.Map<LogSystemInfo>(result.SystemInfo));
            _logging.UpdateLogAction(logAction);

            if (int.Parse(result.SystemInfo.Status.Code) < 0)
            {
                throw new Exception(result.SystemInfo.Status.MessageRu);
            }
        }

        #region PrivateMethods

        private IntelStatusReceiver GetClientKazPatent(string url)
        {
            var client =
                new IntelStatusReceiver
                {
                    Timeout = 1000 * 60 * _configuration.RequestTimeoutInMinutes,
                    Url = url
                };
            return client;
        }

        #endregion
    }
}