using System.Collections.Generic;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.ProtectionDocs;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    [TemplateFieldName(TemplateFieldName.Referat)]
    internal class Referat : TemplateFieldValueBase
    {
        public Referat(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId" };
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            var ownerType = (Owner.Type)(int)parameters["OwnerType"];
            var ownerId = (int)parameters["RequestId"];
            switch (ownerType)
            {
                case Owner.Type.Request:
                    var requestProtectionDoc = Executor.GetQuery<GetRequestByIdQuery>()
                        .Process(q => q.Execute(ownerId));
                    return requestProtectionDoc?.Referat ?? string.Empty;
                case Owner.Type.ProtectionDoc:
                    var protectionDoc = Executor.GetQuery<GetProtectionDocByIdQuery>()
                        .Process(q => q.Execute(ownerId));
                    return protectionDoc?.Referat ?? string.Empty;
                default:
                    return string.Empty;
            }
        }
    }
}
