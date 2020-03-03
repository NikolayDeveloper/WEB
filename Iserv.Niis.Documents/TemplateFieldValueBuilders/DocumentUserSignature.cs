using System;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Documents;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    [TemplateFieldName(TemplateFieldName.DocumentUserSignature)]
    internal class DocumentUserSignature : TemplateFieldValueBase
    {
        public DocumentUserSignature(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "DocumentId" };
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            var documentId = (int)parameters["DocumentId"];

            var document = Executor.GetQuery<GetDocumentByIdQuery>().Process(q => q.Execute(documentId));
            var wfIds = document.Workflows.Select(d => d.Id).ToList(); 
            var documentUserSignatures = Executor.GetQuery<GetDocumentUserSignatureByWorkflowIdQuery>().Process(q => q.Execute(wfIds));

            var signData = documentUserSignatures.Select(d => string.Format("{0}, {1} {2} {3}", d.User.NameRu, d.User.Position.PositionType.NameRu, Environment.NewLine, d.SignerCertificate.Replace(" ", "")));

            return string.Join(Environment.NewLine, signData);
        }
    }
}
