using System;
using System.Linq;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Integration;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Business.Implementations
{
    public class IntegrationStatusUpdater : IIntegrationStatusUpdater
    {
        private readonly NiisWebContext _context;

        public IntegrationStatusUpdater(NiisWebContext context)
        {
            _context = context;
        }

        public void Add(int requestWorkflowId)
        {
            var requestWorkflow = _context.RequestWorkflows
                .Include(w => w.Owner).ThenInclude(r => r.ReceiveType)
                .Include(w => w.FromStage)
                .Include(w => w.CurrentStage)
                .First(w => w.Id == requestWorkflowId);
            var request = requestWorkflow.Owner;
            if (request == null)
            {
                return;
            }

            if (requestWorkflow.FromStage?.OnlineRequisitionStatusId ==
                requestWorkflow.CurrentStage?.OnlineRequisitionStatusId ||
                requestWorkflow.CurrentStage?.OnlineRequisitionStatusId == null)
            {
                return;
            }

            if (request.ReceiveType.Code != DicReceiveTypeCodes.ElectronicFeedEgov &&
                request.ReceiveType.Code != DicReceiveTypeCodes.ElectronicFeed)
            {
                return;
            }

            var additionalInfo =
                $"вх. № {request.IncomingNumber ?? string.Empty} штрих-код {request.Barcode}";
            var protectionDocRegNumber = _context.ProtectionDocs
                                             .Where(x => x.RequestId == request.Id)
                                             .Select(x => x.RegNumber)
                                             .FirstOrDefault() ?? string.Empty;
            if (!string.IsNullOrEmpty(protectionDocRegNumber))
            {
                additionalInfo = $"рег. № {protectionDocRegNumber}, {additionalInfo}";
            }

            _context.IntegrationStatuses.Add(new IntegrationStatus
            {
                AdditionalInfo = additionalInfo,
                DateCreate = DateTimeOffset.Now,
                OnlineRequisitionStatusId = requestWorkflow.CurrentStage.OnlineRequisitionStatusId.Value,
                RequestBarcode = request.Barcode
            });
            _context.SaveChanges();
        }
    }
}