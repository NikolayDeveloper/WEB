using Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Models;

namespace Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Abstractions
{
    public interface ITypeInfoService
    {
        void GetTypesInfo(GetReferenceArgument argument, GetReferenceResult result);
    }
}