using System.Linq;
using Iserv.Niis.Domain.Entities.Integration;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Abstractions
{
    public interface IGetCountTextForPaySumService
    {
        string GetCountTextForPaySumResult(int DocumentTypeId, int MainDocumentTypeId);
    }
}