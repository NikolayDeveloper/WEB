using System.Collections.Generic;
using System.Linq;
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
    [TemplateFieldName(TemplateFieldName.RequestInfosOfDocument_Complex)]
    internal class RequestInfosOfDocument : TemplateFieldValueBase
    {
        public RequestInfosOfDocument(IExecutor executor) : base(executor)
        {

        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId", "DocumentId" };
        }
        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            var requests = Executor.GetQuery<GetRequestsByDocumentIdQuery>()
                .Process(q => q.Execute((int)parameters["DocumentId"]));

            return requests
                .Select(x =>
                    new RequestApplicantInfoRecord
                    {
                        RequestNum = x?.RequestNum,
                        RequestDate = x.DateCreate.LocalDateTime.Date.ToString("dd.MM.yyyy"),
                        DeclarantShortInfo = $"{GetDeclarant(x)?.NameRu ?? string.Empty} {GetDeclarant(x)?.Country?.Code ?? string.Empty}",
                        PatentName = x.NameKz ?? x.NameRu ?? x.NameEn ?? string.Empty
                    })
                .ToList();
        }
        private DicCustomer GetDeclarant(Request request)
        {
            return request?.RequestCustomers
                .FirstOrDefault(rc => rc.CustomerRole.Code == DicCustomerRole.Codes.Declarant)?.Customer;
        }
    }
}
