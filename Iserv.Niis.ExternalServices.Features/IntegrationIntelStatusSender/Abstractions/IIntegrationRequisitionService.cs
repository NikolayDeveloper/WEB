using Iserv.Niis.Domain.Entities.Integration;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelStatusSender.Abstractions
{
    public interface IIntegrationRequisitionService
    {
        IntegrationRequisition GetRequisition(int requestBarcode);
    }
}