using Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Models;

namespace Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Abstractions
{
    public interface IDocumentService
    {
        void GetRequests(GetDocumentListArgument argument, GetDocumentListResult result);
    }
}