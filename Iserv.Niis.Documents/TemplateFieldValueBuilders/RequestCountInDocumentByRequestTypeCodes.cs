using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    [TemplateFieldName(TemplateFieldName.RequestCountInDocumentByRequestTypeInventionAndUsefulModel)]
    internal class RequestCountInDocumentByRequestTypeInventionAndUsefulModel :  TemplateFieldValueBase
    {
        public RequestCountInDocumentByRequestTypeInventionAndUsefulModel(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId", "DocumentId" };
        }

        protected override object GetInternal(Dictionary<string, object> parameters)
        {
            var requests = Executor.GetQuery<GetRequestsByDocumentIdQuery>()
                .Process(q => q.Execute((int)parameters["DocumentId"]));
            var requestCount = requests
                .Where(r => new []{ DicProtectionDocTypeCodes.RequestTypeInventionCode, DicProtectionDocTypeCodes.RequestTypeUsefulModelCode }.Contains(r.ProtectionDocType?.Code ?? string.Empty))
                .Count();
            return requestCount.ToString();
        }
    }
}
