using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Abstractions
{
    public interface IGetRequisitionListForPaymentService
    {
        void Handle(GetRequisitionListForPaymentArgument argument, GetRequisitionListForPaymentResult result);
    }
}