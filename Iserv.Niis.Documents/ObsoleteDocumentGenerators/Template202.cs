using System.Collections.Generic;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Documents.UserInput.Abstract;
using Iserv.Niis.FileConverter.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using TemplateEngine.Docx;

namespace Iserv.Niis.Documents.ObsoleteDocumentGenerators
{
    [DocumentGenerator(3592, DicDocumentTypeCodes.StatementTrademark)]
    public class Template202 : DocumentGeneratorBase
    {
        public Template202(IExecutor executor, ITemplateFieldValueFactory templateFieldValueFactory,
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
                BuildField(TemplateFieldName.ApplicantPhone),
                BuildField(TemplateFieldName.ApplicantPhoneFax),
                BuildField(TemplateFieldName.ApplicantEmail),
                BuildField(TemplateFieldName.PatentAttorney),
                BuildField(TemplateFieldName.CorrespondenceContact),
                BuildField(TemplateFieldName.CorrespondenceAddress),
                BuildField(TemplateFieldName.CorrespondencePhone),
                BuildField(TemplateFieldName.CorrespondencePhoneFax),
                BuildField(TemplateFieldName.CorrespondenceEmail),
                BuildField(TemplateFieldName.Priority31),
                BuildField(TemplateFieldName.Priority32),
                BuildField(TemplateFieldName.Priority33),
                BuildField(TemplateFieldName.Colors),
                BuildField(TemplateFieldName.Icgs511),
                BuildField(TemplateFieldName.InformationOnStateRegistration),
                BuildImage(TemplateFieldName.Image)
            );
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] {"RequestId", "UserId"};
        }
    }
}