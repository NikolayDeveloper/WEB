using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Utils;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Implementations
{
    public class GetRequisitionListByMessageTypeService : IGetRequisitionListByMessageTypeService
    {
        private readonly IntegrationRequisitionInfoHelper _integrationRequisitionInfoHelper;

        public GetRequisitionListByMessageTypeService(IntegrationRequisitionInfoHelper integrationRequisitionInfo)
        {
            _integrationRequisitionInfoHelper = integrationRequisitionInfo;
        }

        public void Handle(GetRequisitionListByMessageTypeArgument argument,
            GetRequisitionListByMessageTypeResult result)
        {
            result.RequisitionList = _integrationRequisitionInfoHelper
                .GetRequisitionInfoByMessageType(argument.DocumentType.UID, argument.PatentType.UID, argument.XIN)
                .ToArray();
        }
    }
}