using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.ProtectionDocs;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    [TemplateFieldName(TemplateFieldName.Priority31WithoutCode)]
    internal class Priority31WithoutCode : TemplateFieldValueBase
    {
        public Priority31WithoutCode(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] {"RequestId"};
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            var ownerType = (Owner.Type)(int)parameters["OwnerType"];
            string result;
            switch (ownerType)
            {
                case Owner.Type.Request:
                    var requestId = (int)parameters["RequestId"];
                    var request = Executor.GetQuery<GetRequestByIdQuery>()
                        .Process(q => q.Execute(requestId));

                    var earlyRegs = request?.EarlyRegs
                        .Select(e => "(310) Номер приоритетной заявки \r\n " + e.RegNumber)
                        .ToList();

                    result = string.Empty;
                    if (earlyRegs != null)
                    {
                        result = string.Join(";", earlyRegs);
                    }

                    return result;
                case Owner.Type.ProtectionDoc:
                    var protectionDoc = Executor.GetQuery<GetProtectionDocByIdQuery>()
                        .Process(q => q.Execute((int)parameters["RequestId"]));
                    var protectionDocEarlyRegs = protectionDoc?.EarlyRegs
                        .Select(e => "(310) Номер приоритетной заявки \r\n " + e.RegNumber)
                        .ToList();
                    result = string.Empty;
                    if (protectionDocEarlyRegs != null)
                    {
                        result = string.Join(";", protectionDocEarlyRegs);
                    }

                    return result;
                default:
                    return string.Empty;
            }
        }
    }
}