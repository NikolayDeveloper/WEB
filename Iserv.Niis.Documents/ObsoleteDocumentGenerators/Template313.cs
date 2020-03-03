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
    [DocumentGenerator(2354, "PRIL_VOSST_IZ_POL_MOD")]
    public class Template313 : DocumentGeneratorBase
    {
        public Template313(IExecutor executor, ITemplateFieldValueFactory templateFieldValueFactory, IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper) 
            : base(executor, templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }

        protected override Content PrepareValue()
        {
            return new Content(
                BuildField(TemplateFieldName.President),
                BuildField(TemplateFieldName.PresidentKz),
                BuildField(TemplateFieldName.GosNumber),
                BuildField(TemplateFieldName.DocumentNumber));
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] {"RequestId", DocumentGeneratorBase.UserInputFieldsParameterName};
        }
    }
}
