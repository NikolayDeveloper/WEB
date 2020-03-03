using System;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Integration;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelStatusSender.Abstractions;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelStatusSender.Implementations
{
    public class IntegrationStatusService : IIntegrationStatusService
    {
        private readonly NiisWebContext _niisWebContext;

        public IntegrationStatusService(NiisWebContext niisWebContext)
        {
            _niisWebContext = niisWebContext;
        }

        public List<IntegrationStatus> GetUnsentStatuses()
        {
            return _niisWebContext.IntegrationStatuses
                .Where(x => x.DateSent == null)
                .ToList();
        }

        public void MarkSentStatus(IntegrationStatus integrationStatus)
        {
            if (integrationStatus != null)
            {
                integrationStatus.DateSent = DateTimeOffset.Now;
                _niisWebContext.SaveChanges();
            }
        }

        public void SetNoteStatus(IntegrationStatus integrationStatus, string note)
        {
            if (integrationStatus != null)
            {
                integrationStatus.Note = note;
                _niisWebContext.SaveChanges();
            }
        }
    }
}