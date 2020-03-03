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
    [DocumentGenerator(2811, "PRIL_NEISKL_LIC_IN_PAT")]
    public class Template333 : DocumentGeneratorBase
    {
        public Template333(IExecutor executor, ITemplateFieldValueFactory templateFieldValueFactory,
              IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper) : base(executor,
              templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }
        protected override Content PrepareValue()
        {// Oтсутствует <Лицо на которое зарегистрирована лицензия, Сторона 2>
         //ContactRegistrationDate
            return new Content(
                BuildField(TemplateFieldName.DocumentNumber),
                BuildField(TemplateFieldName.GosNumber),
                BuildField(TemplateFieldName.President),
                BuildField(TemplateFieldName.PresidentKz)                

              );          
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId" };
        }
    }
}
