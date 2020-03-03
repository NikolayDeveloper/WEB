using Iserv.Niis.Business.Helpers;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Utils;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Implementations
{
    public class GetRequisitionListForPaymentService : IGetRequisitionListForPaymentService
    {
        private readonly IntegrationRequisitionInfoHelper _integrationRequisitionInfoHelper;
        private readonly DictionaryHelper _dictionaryHelper;

        public GetRequisitionListForPaymentService(IntegrationRequisitionInfoHelper integrationRequisitionInfoHelper, DictionaryHelper dictionaryHelper)
        {
            _integrationRequisitionInfoHelper = integrationRequisitionInfoHelper;
            _dictionaryHelper = dictionaryHelper;
        }

        public void Handle(GetRequisitionListForPaymentArgument argument,
            GetRequisitionListForPaymentResult result)
        {
            int dicDocTypeId = _dictionaryHelper.GetDictionaryIdByCode(nameof(DicDocumentType), DicDocumentTypeCodes._001_002);
            if (argument.DocumentType.UID == dicDocTypeId)
            {
                result.RequisitionList = _integrationRequisitionInfoHelper
                    .GetRequisitionInfoByMessageType(argument.DocumentType.UID, argument.PatentType.UID, argument.XIN)
                    .ToArray();
            }
            else
            {
                result.RequisitionList = _integrationRequisitionInfoHelper
                    .GetRequistionsListForPayment(argument.DocumentType.UID, argument.PatentType.UID, argument.XIN)
                    .ToArray();
            }
        }
    }
}
