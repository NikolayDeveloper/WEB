using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Documents;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    [TemplateFieldName(TemplateFieldName.DeputyDirectorName)]
    public class DeputyDirectorName: TemplateFieldValueBase
    {
        public DeputyDirectorName(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "DocumentId" };
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            var code = new[] {
                RouteStageCodes.DocumentOutgoing_02_2,
                RouteStageCodes.DocumentInternal_IN01_1_0,
                RouteStageCodes.DocumentIncoming_1_2_2,
            };

            var documentId = Convert.ToInt32(parameters["DocumentId"]);
            var document = Executor.GetQuery<GetDocumentByIdQuery>().Process(q => q.Execute(documentId));
            var deputyDirector = document?.Workflows?.FirstOrDefault(w => code.Contains(w.CurrentStage.Code))
                       ?.DocumentUserSignature?.User?.NameRu ?? string.Empty;
            return deputyDirector;
        }
    }
}
