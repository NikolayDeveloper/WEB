using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Documents.Models;
using Iserv.Niis.Documents.Helpers;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    [TemplateFieldName(TemplateFieldName.RequestDocumentInfoRecords_Complex)]
    internal class RequestDocumentInfo : TemplateFieldValueBase
    {
        public RequestDocumentInfo(IExecutor executor) : base(executor)
        {

        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            var requests = Executor.GetQuery<GetRequestsByIds>()
                .Process(q => q.Execute((List<int>)parameters["SelectedRequestIds"]));

            return requests
                .Select(x =>
                    new RequestDocumentInfoRecord
                    {
                        RequestPositive = x?.RequestNum ?? string.Empty,
                        RequestNegative =
                            x?.Documents.Any(d => new[] {"PO5", "PO4_1", "PO5_KZ", "PO5_1111", "PO5_10"}.Contains(d.Document.Type.Code)) == true
                                ? "Қорғау құжаты берілетіндігі туралы"
                                : "Қорғау құжаты берілмейтіндігі туралы ",
                        RequestDate = x?.DateCreate.ToTemplateDateFormat() ?? string.Empty
                    })
                .ToList();
        }
        
        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId" };
        }
    }

}

