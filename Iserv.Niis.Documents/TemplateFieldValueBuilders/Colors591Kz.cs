using System;
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
    struct testsKz
    {
        public string str;
    }

    class testcKz
    {
        public string str;
    }
    [TemplateFieldName(TemplateFieldName.ColorsKz)]
    internal class Colors591Kz : TemplateFieldValueBase
    {
        public Colors591Kz(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId" };
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {


            var ownerType = (Owner.Type)(int)parameters["OwnerType"];
            switch (ownerType)
            {
                case Owner.Type.Request:
                    var request = Executor.GetQuery<GetRequestByIdQuery>()
                        .Process(q => q.Execute((int)parameters["RequestId"]));
                    var colorTzs = request.ColorTzs;
                    return string.Join(", ", colorTzs.Select(c => c.ColorTz.NameKz));
                case Owner.Type.ProtectionDoc:
                    var protectionDoc = Executor.GetQuery<GetProtectionDocByIdQuery>()
                        .Process(q => q.Execute((int)parameters["RequestId"]));
                    var protectionDocColorTzs = protectionDoc.ColorTzs;
                    return string.Join(", ", protectionDocColorTzs.Select(c => c.ColorTz.NameKz));
                default:
                    return string.Empty;
            }
        }
    }
}