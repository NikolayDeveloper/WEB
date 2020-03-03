using Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Models;

namespace Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Abstractions
{
    public interface IProtectionDocService
    {
        void GetProtectionDocs(GetBtBasePatentListArgument argument, GetBtBasePatentListResult result);
    }
}