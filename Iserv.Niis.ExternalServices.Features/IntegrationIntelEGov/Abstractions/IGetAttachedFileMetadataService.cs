using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Abstractions
{
    public interface IGetAttachedFileMetadataService
    {
        void Handle(GetAttachedFileMetadataArgument argument, GetAttachedFileMetadataResult result);
    }
}