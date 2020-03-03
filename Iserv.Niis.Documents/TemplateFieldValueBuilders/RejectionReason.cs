using System;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    [TemplateFieldName(TemplateFieldName.RejectionReason)]
    public class RejectionReason: TemplateFieldValueBase
    {
        public RejectionReason(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId" };
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            var request = Executor.GetQuery<GetRequestByIdQuery>()
                .Process(q => q.Execute((int)parameters["RequestId"]));
            if (request.ICGSRequests != null && request.ICGSRequests.Any())
            {
                var icgs = request.ICGSRequests.Where(r => r.Icgs != null).Select(i => i.ReasonForPartialRefused);
                if (icgs.Any())
                {
                    return string.Join(Environment.NewLine, icgs);
                }
            }

            return "";
        }
    }
}
