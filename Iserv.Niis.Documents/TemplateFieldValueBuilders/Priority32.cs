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
    [TemplateFieldName(TemplateFieldName.Priority32)]
    internal class Priority32 : TemplateFieldValueBase
    {
        public Priority32(IExecutor executor) : base(executor)
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
                        .Select(e => e.RegDate?.ToString("dd.MM.yyyy"))
                        .ToList();

                    result = string.Empty;
                    if (earlyRegs != null)
                    {
                        result = string.Join(";", earlyRegs);
                    }

                    return $"(32) {result}";
                case Owner.Type.ProtectionDoc:
                    var protectionDoc = Executor.GetQuery<GetProtectionDocByIdQuery>()
                        .Process(q => q.Execute((int)parameters["RequestId"]));
                    var protectionDocEarlyRegs = protectionDoc?.EarlyRegs
                        .Select(e => e.RegDate?.ToString("dd.MM.yyyy"))
                        .ToList();
                    result = string.Empty;
                    if (protectionDocEarlyRegs != null)
                    {
                        result = string.Join(";", protectionDocEarlyRegs);
                    }

                    return $"(32) {result}";
                default:
                    return string.Empty;
            }
        }
    }
}