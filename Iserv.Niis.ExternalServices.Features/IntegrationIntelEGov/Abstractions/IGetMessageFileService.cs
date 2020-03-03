using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Abstractions
{
    public interface IGetMessageFileService
    {
        void GetMessageFile(GetMessageFileArgument argument, GetMessageFileResult result);
    }
}