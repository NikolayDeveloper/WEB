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
    [TemplateFieldName(TemplateFieldName.RequestNumber)]
    internal class RequestNumber : TemplateFieldValueBase
    {
        public RequestNumber(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] {"RequestId"};
        }

        protected override object GetInternal(Dictionary<string, object> parameters)
        {
            var ownerType = (Owner.Type)(int)parameters["OwnerType"];
            string number;
            switch (ownerType)
            {
                case Owner.Type.Request:
                    var request = Executor.GetQuery<GetRequestByIdQuery>()
                        .Process(q => q.Execute((int)parameters["RequestId"]));
                    number = request?.RequestNum ?? string.Empty;
                    break;
                case Owner.Type.ProtectionDoc:
                    var pdRequest = Executor.GetQuery<GetProtectionDocByIdQuery>()
                        .Process(q => q.Execute((int) parameters["RequestId"]));
                    number = pdRequest?.RegNumber ?? string.Empty;
                    break;
                default:
                    number = string.Empty;
                    break;
            }
            

            return number;
        }
    }
}