using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Abstractions
{
    public interface IGetRequisitionInfoService
    {
        void GetRequisitionInfo(GetRequisitionInfoArgument argument, GetRequisitionInfoResult result);
    }
}