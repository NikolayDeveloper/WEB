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
    [DocumentGenerator(3051, "PRIL_ISKL_LIC_IN_PAT")]
    public class Template328 : DocumentGeneratorBase
    {
        public Template328(IExecutor executor, ITemplateFieldValueFactory templateFieldValueFactory,
            IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper) : base(executor,
            templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }
        protected override Content PrepareValue() 
        {
            return new Content(// отсутствует <Лицо на которое зарегистрирована лицензия, Сторона 2 >
              BuildField(TemplateFieldName.DocumentNumber),
              BuildField(TemplateFieldName.GosNumber),
              BuildField(TemplateFieldName.President),
              BuildField(TemplateFieldName.PresidentKz)
              

               );
        }
        

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId"};
        }
    }
}
