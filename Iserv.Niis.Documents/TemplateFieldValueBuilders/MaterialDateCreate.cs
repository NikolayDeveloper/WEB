using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Documents;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using System.Collections.Generic;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    [TemplateFieldName(TemplateFieldName.MaterialDateCreate)]
    internal class MaterialDateCreate : TemplateFieldValueBase
    {
        public MaterialDateCreate(IExecutor executor) : base(executor)
        {

        }
        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "DocumentId" };
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            var document = Executor.GetQuery<GetDocumentByIdQuery>().Process(q => q.Execute((int)parameters["DocumentId"]));
            return document?.DateCreate.ToString("dd.MM.yyyy") ?? string.Empty;
        }
    }
}
