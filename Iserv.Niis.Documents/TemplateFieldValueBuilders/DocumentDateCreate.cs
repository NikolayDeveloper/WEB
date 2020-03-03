using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Documents;
using Iserv.Niis.Documents.DocumentsBusinessLogic.ProtectionDocs;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Documents.Helpers;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    [TemplateFieldName(TemplateFieldName.DocumentDateCreate)]
    internal class DocumentDateCreate : TemplateFieldValueBase
    {
        public DocumentDateCreate(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "DocumentId" };
        }

        protected override object GetInternal(Dictionary<string, object> parameters)
        {
            var document = Executor.GetQuery<GetDocumentByIdQuery>()
                .Process(q => q.Execute(Convert.ToInt32(parameters["DocumentId"])));

            return document?.DateCreate.ToTemplateDateFormat() ?? string.Empty;
        }
    }
}
