using System;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Documents;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Documents.Helpers;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    [TemplateFieldName(TemplateFieldName.ExpertiseRequestData)]
    public class ExpertiseRequestData: TemplateFieldValueBase
    {
        public ExpertiseRequestData(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId" };
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            var requestId = Convert.ToInt32(parameters["RequestId"]);
            var expertiseRequestCodes = new List<string>
            {
                DicDocumentTypeCodes.DeclarantAddressMismatchRequest,
                DicDocumentTypeCodes.TranslationRequest,
                DicDocumentTypeCodes.TZ_ZAP_O_DOV,
                DicDocumentTypeCodes.PriorityRequest,
                DicDocumentTypeCodes.MatchingIconsRequest,
                DicDocumentTypeCodes.TZ_ZAP_O_IZOBR,
                DicDocumentTypeCodes.IcgsOrImageMissingRequest,
                DicDocumentTypeCodes.IcgsMismatchRequest,
                DicDocumentTypeCodes.FormalExpertizePaymentRequest,
                DicDocumentTypeCodes.EcoBioOrganicDesignationRequest,
                DicDocumentTypeCodes.StateSymbolsAndOtherRequest,
                DicDocumentTypeCodes.DesignationTranslationRequest,
                DicDocumentTypeCodes.CopyrightRequest,
                DicDocumentTypeCodes.PersonalNonPropertyRequest,
                DicDocumentTypeCodes.ObjectiveLinkRequest,
                DicDocumentTypeCodes.ConsentLetterRequest
            };
            var requestDocuments = Executor.GetQuery<GetDocumentsByRequestIdQuery>().Process(q => q.Execute(requestId));
            var result = requestDocuments.Where(rd => expertiseRequestCodes.Contains(rd.Type.Code)).Select(rd =>
                $"исх. № {rd.OutgoingNumber} от {rd.SendingDate.ToTemplateDateFormat()}");
            return string.Join(", ", result);
        }
    }
}
