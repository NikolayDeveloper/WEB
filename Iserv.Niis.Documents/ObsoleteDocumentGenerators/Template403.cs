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
    [DocumentGenerator(2794, "P001_M")]
    public class Template403 : DocumentGeneratorBase
    {
        public Template403(IExecutor executor, ITemplateFieldValueFactory templateFieldValueFactory,
        IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper) : base(executor,
            templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }

        protected override Content PrepareValue()
        {
            return new Content(             
                BuildField(TemplateFieldName.DocumentNumber),
                BuildField(TemplateFieldName.CorrespondenceContact),
                BuildField(TemplateFieldName.CorrespondenceAddress),
                BuildField(TemplateFieldName.RequestNameRu),
                BuildField(TemplateFieldName.RequestNumber),            
                BuildField(TemplateFieldName.CurrentDate),
                BuildField(TemplateFieldName.CurrentUser)
                );
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId","DocumentId", DocumentGeneratorBase.UserInputFieldsParameterName };
        }
    }
}
