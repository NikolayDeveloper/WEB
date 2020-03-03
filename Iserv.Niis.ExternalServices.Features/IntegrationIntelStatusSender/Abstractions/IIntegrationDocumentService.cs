using System.Linq;
using Iserv.Niis.Domain.Entities.Integration;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelStatusSender.Abstractions
{
    public interface IIntegrationDocumentService
    {
        IQueryable<IntegrationDocument> GetUnsentDocuments();
        void MarkSentDocument(int id);
    }
}