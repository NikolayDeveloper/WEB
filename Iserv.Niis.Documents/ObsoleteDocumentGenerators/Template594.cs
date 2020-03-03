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
    [DocumentGenerator(32, DicDocumentTypeCodes.ExpertTmRegisterRefusalOpinion)]
    public class Template594 : DocumentGeneratorBase
    {
        public Template594(IExecutor executor, ITemplateFieldValueFactory templateFieldValueFactory,
            IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper) : base(executor,
            templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }

        protected override Content PrepareValue()
        {
            return new Content(
                BuildField(TemplateFieldName.CorrespondenceContact),
                BuildField(TemplateFieldName.CorrespondenceAddress),
                BuildImage(TemplateFieldName.Image),
                BuildField(TemplateFieldName.CurrentYear),
                BuildField(TemplateFieldName.RequestNumber),
                BuildField(TemplateFieldName.RequestDate),
                BuildField(TemplateFieldName.Priority31WithoutCode),
                BuildField(TemplateFieldName.Priority32WithoutCode),
                BuildField(TemplateFieldName.Priority33WithoutCode),
                BuildField(TemplateFieldName.Declarants),
                BuildField(TemplateFieldName.ApplicantAddress),
                BuildField(TemplateFieldName.ApplicantAddressKz),
                BuildField(TemplateFieldName.Icgs511),
                BuildField(TemplateFieldName.Colors),
                BuildField(TemplateFieldName.Disclaimer),
                BuildField(TemplateFieldName.FailureInfoTZ),
                BuildField(TemplateFieldName.CurrentUser)
            );
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] {"RequestId", "UserId"};
        }
    }
}