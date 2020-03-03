using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Abstractions
{
    public interface IGetAttorneyInfoService
    {
        void GetAttorneyInfo(GetAttorneyInfoArgument argument, GetAttorneyInfoResult result);
    }
}