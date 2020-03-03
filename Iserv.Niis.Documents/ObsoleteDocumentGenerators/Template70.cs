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
    [DocumentGenerator(2173, "TZ_VN_IZM_NAIMEN_ZAYAV")]
    public class Template70 : DocumentGeneratorBase
    {
        public Template70(IExecutor executor, ITemplateFieldValueFactory templateFieldValueFactory,
            IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper) : base(executor,
            templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }

        protected override Content PrepareValue()
        {
            return new Content(
                BuildField(TemplateFieldName.CorrespondenceAddress),
                BuildField(TemplateFieldName.DeclarantsAndAddress),
                BuildField(TemplateFieldName.RequestNumber),
                BuildField(TemplateFieldName.RequestDate),
                BuildField(TemplateFieldName.Priority300),
                BuildField(TemplateFieldName.Declarants),
                BuildField(TemplateFieldName.Mktu511),
                BuildField(TemplateFieldName.HeadName),
                BuildField(TemplateFieldName.ExpertName),
                BuildField(TemplateFieldName.ExpertPhone));
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] {"UserId", "RequestId"};
        }
    }
}