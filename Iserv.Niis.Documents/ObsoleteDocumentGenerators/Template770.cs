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
    [DocumentGenerator(10017, "DK_Registry_Transfer")]
    public class Template770 : DocumentGeneratorBase
    {
        public Template770(IExecutor executor, ITemplateFieldValueFactory templateFieldValueFactory, IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper) : base(executor, templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }
        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId", "DocumentId", DocumentGeneratorBase.UserInputFieldsParameterName };
        }

        protected override Content PrepareValue()
        {
            return new Content(
                BuildField(TemplateFieldName.CurrentUser),
                BuildField(TemplateFieldName.MaterialNum),
                BuildField(TemplateFieldName.ContractType),
                BuildField(TemplateFieldName.ContractGosNumber),
                BuildField(TemplateFieldName.ContractIncomingDate),
                BuildField(TemplateFieldName.ContractIncomingNumber),
                BuildField(TemplateFieldName.ContractAddressee)
            );
        }
    }
}
