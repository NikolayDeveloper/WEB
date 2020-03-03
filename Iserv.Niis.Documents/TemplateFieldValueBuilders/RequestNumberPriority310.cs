using Iserv.Niis.Common.Codes;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Documents;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    [TemplateFieldName(TemplateFieldName.RequestNumberPriority310)]
    public class RequestNumberPriority310 : TemplateFieldValueBase
    {
        public RequestNumberPriority310(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId" };
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            var requestId = Convert.ToInt32(parameters["RequestId"]);
            var request = Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.Execute(requestId));
            return request.EarlyRegs.FirstOrDefault()?.RegNumber ?? string.Empty;
        }
    }
}
