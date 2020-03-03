using Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Models;

namespace Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Abstractions
{
    public interface IRfProtectionDocService
    {
        void GetRfProtectionDocs(GetRfPatentListArgument argument, GetRfPatentListResult result);
    }
}