using Iserv.Niis.Common.Codes;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Documents;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    [TemplateFieldName(TemplateFieldName.ExecutorPhone)]
    public class ExecutorPhone : TemplateFieldValueBase
    {
        public ExecutorPhone(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "DocumentId" };
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            var code = new[] {
                RouteStageCodes.DocumentOutgoing_01_1,
            };
            var documentId = Convert.ToInt32(parameters["DocumentId"]);
            var document = Executor.GetQuery<GetDocumentByIdQuery>().Process(q => q.Execute(documentId));
            return document?.Workflows?.FirstOrDefault(w => code.Contains(w.CurrentStage.Code))
                       ?.DocumentUserSignature?.User?.PhoneNumber ?? string.Empty;
        }
    }
}
