using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Abstractions
{
    public interface IGetRequisitionListByMessageTypeService
    {
        void Handle(GetRequisitionListByMessageTypeArgument argument, GetRequisitionListByMessageTypeResult result);
    }
}