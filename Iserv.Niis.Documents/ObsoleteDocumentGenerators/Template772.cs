using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Documents.Models;
using Iserv.Niis.Documents.UserInput.Abstract;
using Iserv.Niis.FileConverter.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using TemplateEngine.Docx;

namespace Iserv.Niis.Documents.ObsoleteDocumentGenerators
{
    [DocumentGenerator(10022, "006.014.1.SA")]
    public class Template772 : DocumentGeneratorBase
    {
        public Template772(IExecutor executor, ITemplateFieldValueFactory templateFieldValueFactory,
         IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper)
         : base(executor, templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }
        protected override Content PrepareValue()
        {
            return new Content(
                BuildField(TemplateFieldName.CurrentUser),
                BuildField(TemplateFieldName.MaterialNum),
                BuildField(TemplateFieldName.MaterialDateCreate),
                GetRequestInfosTableContent()
                );
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId", "DocumentId" };
        }

        private TableContent GetRequestInfosTableContent()
        {
            var rows = (IList<RequestApplicantInfoRecord>)TemplateFieldValueFactory
              .Create(TemplateFieldName.RequestInfosOfDocument_Complex).Get(Parameters);

            var tableContent = new TableContent(nameof(RequestApplicantInfoRecord));
            if (rows.Any())
            {
                foreach (var row in rows)
                {
                    tableContent.AddRow(
                        new FieldContent(nameof(RequestApplicantInfoRecord.RequestNum), row.RequestNum),
                        new FieldContent(nameof(RequestApplicantInfoRecord.PatentName), row.PatentName),
                        new FieldContent(nameof(RequestApplicantInfoRecord.DeclarantShortInfo), row.DeclarantShortInfo)
                    );
                }
            }
            else
            {
                tableContent.AddRow(
                       new FieldContent(nameof(RequestApplicantInfoRecord.RequestNum), string.Empty),
                       new FieldContent(nameof(RequestApplicantInfoRecord.PatentName), string.Empty),
                       new FieldContent(nameof(RequestApplicantInfoRecord.DeclarantShortInfo), string.Empty)
                   );
            }


            return tableContent;
        }
    }
}
