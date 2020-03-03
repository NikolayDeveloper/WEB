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
    [DocumentGenerator(3871, "001.001_NMPT")]
    public class Template199 : DocumentGeneratorBase
    {
        public Template199(IExecutor executor, ITemplateFieldValueFactory templateFieldValueFactory,
            IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper) : base(executor,
            templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }

        protected override Content PrepareValue()
        {
            return new Content(
              BuildField(TemplateFieldName.Declarants),
              BuildField(TemplateFieldName.DeclarantsKz),
              BuildField(TemplateFieldName.ApplicantAddress),
              BuildField(TemplateFieldName.CorrespondencePhone),
              BuildField(TemplateFieldName.ApplicantPhone),
              BuildField(TemplateFieldName.CorrespondencePhoneFax),
              BuildField(TemplateFieldName.ApplicantPhoneFax),
              BuildField(TemplateFieldName.CorrespondenceContact),
              BuildField(TemplateFieldName.CorrespondenceAddress),
              BuildField(TemplateFieldName.PatentAttorney),
              BuildField(TemplateFieldName.RequestNameRu),
              BuildField(TemplateFieldName.TypeOfGoods),
              BuildField(TemplateFieldName.PlaceOfOrigin),
              BuildField(TemplateFieldName.SpecialPropertiesOfGood)
                );
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId" };
        }
    }
}
