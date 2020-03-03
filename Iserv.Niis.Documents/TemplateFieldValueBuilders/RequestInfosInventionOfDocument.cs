using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Documents.Models;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    [TemplateFieldName(TemplateFieldName.RequestInfosInventionOfDocument_Complex)]
    internal class RequestInfosInventionOfDocument : TemplateFieldValueBase
    {
        public RequestInfosInventionOfDocument(IExecutor executor) : base(executor)
        {

        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId", "DocumentId" };
        }
        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            var requests = Executor.GetQuery<GetRequestsByDocumentIdQuery>()
                .Process(q => q.Execute((int)parameters["DocumentId"]))
                .Where(r => new[] { DicProtectionDocTypeCodes.RequestTypeInventionCode }.Contains(r.ProtectionDocType?.Code ?? string.Empty));

            return requests
                .Select(x =>
                    new RequestApplicantInfoRecord
                    {
                        RequestNum = x?.RequestNum ?? string.Empty,
                        RequestDate = x.DateCreate.LocalDateTime.Date.ToString("dd.MM.yyyy"),
                        DeclarantShortInfo = $"{GetDeclarant(x)?.NameRu ?? string.Empty} {GetDeclarant(x)?.Country?.Code ?? string.Empty}",
                        PatentAttorneyShortInfo = $"{GetPatentAttorney(x)?.NameRu ?? string.Empty} {GetPatentAttorney(x)?.Country?.Code ?? string.Empty}",
                        ConfidantShortInfo = $"{GetConfidant(x)?.NameRu ?? string.Empty} {GetConfidant(x)?.Country?.Code ?? string.Empty}",
                        PatentName = x.NameKz ?? x.NameRu ?? x.NameEn ?? string.Empty
                    })
                .ToList();
        }
        private DicCustomer GetDeclarant(Request request)
        {
            return request?.RequestCustomers
                .FirstOrDefault(rc => rc.CustomerRole.Code == DicCustomerRole.Codes.Declarant)?.Customer;
        }
        private DicCustomer GetPatentAttorney(Request request)
        {
            return request?.RequestCustomers
                .FirstOrDefault(rc => rc.CustomerRole.Code == DicCustomerRole.Codes.PatentAttorney)?.Customer;
        }
        private DicCustomer GetConfidant(Request request)
        {
            return request?.RequestCustomers
                .FirstOrDefault(rc => rc.CustomerRole.Code == DicCustomerRole.Codes.Confidant)?.Customer;
        }
    }
}
