using System.Collections.Generic;
using Iserv.Niis.Domain.Entities.Integration;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelStatusSender.Abstractions
{
    public interface IIntegrationStatusService
    {
        List<IntegrationStatus> GetUnsentStatuses();
        void MarkSentStatus(IntegrationStatus integrationStatus);
        void SetNoteStatus(IntegrationStatus integrationStatus, string note);
    }
}