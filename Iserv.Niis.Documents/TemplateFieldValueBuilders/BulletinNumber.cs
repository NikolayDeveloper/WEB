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
    [TemplateFieldName(TemplateFieldName.BulletinNumber)]
    internal class BulletinNumber : TemplateFieldValueBase
    {
        public BulletinNumber(IExecutor executor) : base(executor)
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
                    var requestId = (int) parameters["RequestId"];
                    var requestProtectionDoc = Executor.GetQuery<GetProtectionDocByRequestIdQuery>()
                        .Process(q => q.Execute(requestId))
                        .FirstOrDefault();
                    return requestProtectionDoc?.Bulletins?.FirstOrDefault()?.Bulletin?.Number ?? string.Empty;
                case Owner.Type.ProtectionDoc:
                    var protectionDoc = Executor.GetQuery<GetProtectionDocByIdQuery>()
                        .Process(q => q.Execute((int)parameters["RequestId"]));
                    return protectionDoc?.Bulletins?.FirstOrDefault()?.Bulletin?.Number ?? string.Empty;
                default:
                    return string.Empty;
            }
        }
    }
}
