using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Documents.UserInput.Abstract;
using Iserv.Niis.FileConverter.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using TemplateEngine.Docx;

namespace Iserv.Niis.Documents.DocumentGenerators
{
    //Генератор шаблона-заглушки
    [DocumentGenerator(0, "Dummy")]
    public class DummyTemplate: DocumentGeneratorBase
    {
        public DummyTemplate(IExecutor executor, ITemplateFieldValueFactory templateFieldValueFactory,
            IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper) : base(executor,
            templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }


        protected override Content PrepareValue()
        {
            return new Content(new FieldContent("Test", "Test"));
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId", "UserId" };
        }
    }
}
