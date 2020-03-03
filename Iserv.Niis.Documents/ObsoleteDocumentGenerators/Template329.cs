using System.Collections.Generic;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Documents.UserInput.Abstract;
using Iserv.Niis.FileConverter.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using TemplateEngine.Docx;

namespace Iserv.Niis.Documents.ObsoleteDocumentGenerators
{
    [DocumentGenerator(3611, "PRIL_ISKL_LIC_POL_MOD")]
    public class Template329 : DocumentGeneratorBase
    {
        public Template329(IExecutor executor, ITemplateFieldValueFactory templateFieldValueFactory, IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper)
            : base(executor, templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }

        protected override Content PrepareValue()
        {
            return new Content(
                BuildField(TemplateFieldName.DocumentNumber),
                BuildField(TemplateFieldName.GosNumber),
                BuildField(TemplateFieldName.PresidentKz),
                BuildField(TemplateFieldName.President));
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] {"RequestId"};
        }
    }
}
