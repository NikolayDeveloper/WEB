using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.ProtectionDocs;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Documents.Helpers;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    [TemplateFieldName(TemplateFieldName.RequestDate)]
    internal class RequestDate : TemplateFieldValueBase
    {
        public RequestDate(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] {"RequestId"};
        }

        protected override object GetInternal(Dictionary<string, object> parameters)
        {

            var ownerType = (Owner.Type)(int)parameters["OwnerType"];
            string requestDate;
            switch (ownerType)
            {
                case Owner.Type.Request:
                    var request = Executor.GetQuery<GetRequestByIdQuery>()
                        .Process(q => q.Execute((int)parameters["RequestId"]));
                    requestDate = request?.RequestDate.ToTemplateDateFormat() ?? string.Empty;
                    break;
                case Owner.Type.ProtectionDoc:
                    var pdRequest = Executor.GetQuery<GetProtectionDocByIdQuery>()
                        .Process(q => q.Execute((int)parameters["RequestId"]));
                    requestDate = pdRequest?.RegDate.ToTemplateDateFormat() ?? string.Empty;
                    break;
                case Owner.Type.Contract:
                    var contractId = (int)parameters["RequestId"];
                    var contractProtectionDoc = Executor.GetQuery<GetProtectionDocsByContractIdQuery>()
                        .Process(q => q.Execute(contractId))
                        .FirstOrDefault();
                    requestDate = contractProtectionDoc?.RegDate?.ToTemplateDateFormat() ?? string.Empty;
                    break;
                default:
                    requestDate = string.Empty;
                    break;
            }

            return requestDate;
        }
    }
}