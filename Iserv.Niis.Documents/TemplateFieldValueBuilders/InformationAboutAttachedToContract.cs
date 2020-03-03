using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Contracts;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Documents.Helpers;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    [TemplateFieldName(TemplateFieldName.InformationAboutAttachedToContract)]
    public class InformationAboutAttachedToContract : TemplateFieldValueBase
    {
        public InformationAboutAttachedToContract(IExecutor executor) : base(executor)
        {

        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId" };
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            var contract = Executor.GetQuery<GetContractByIdQuery>().Process(q => q.Execute((int)parameters["RequestId"]));
            var protectionDocInfos = contract?.ProtectionDocs
                .Select(cp => $"№ {cp.ProtectionDoc?.GosNumber ?? string.Empty} (заявка № {cp.ProtectionDoc?.Request?.RequestNum ?? string.Empty} от {cp.ProtectionDoc?.Request?.RequestDate?.ToTemplateDateFormat() ?? string.Empty })")
                .ToList();
            var requestInfos = contract?.RequestsForProtectionDoc
                .Select(cr => $"№ {cr.Request?.RequestNum ?? string.Empty}")
                .ToList();

            var commonInfos = protectionDocInfos.Concat(requestInfos).ToList();
            var result = string.Join(", ", commonInfos);

            return result;
        }
    }
}
