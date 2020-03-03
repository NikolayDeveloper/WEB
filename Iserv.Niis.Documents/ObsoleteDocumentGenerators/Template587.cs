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
    [DocumentGenerator(31, DicDocumentTypeCodes.ExpertRefusalOpinionFinal)]
    public class Template587 : DocumentGeneratorBase
    {
        public Template587(IExecutor executor, ITemplateFieldValueFactory templateFieldValueFactory,
            IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper) : base(executor,
            templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }

        protected override Content PrepareValue()
        {
            return new Content(
                BuildField(TemplateFieldName.CurrentUser),
                BuildField(TemplateFieldName.Declarants),
                BuildField(TemplateFieldName.ApplicantAddress),
                BuildField(TemplateFieldName.Disclaimer),
                BuildImage(TemplateFieldName.Image),
                BuildField(TemplateFieldName.RequestNumber),
                BuildField(TemplateFieldName.RequestDate),
                BuildField(TemplateFieldName.Icgs511),
                BuildField(TemplateFieldName.Priority31WithoutCode),
                BuildField(TemplateFieldName.Priority32WithoutCode),
                BuildField(TemplateFieldName.Priority33WithoutCode),
                BuildField(TemplateFieldName.CorrespondenceContact),
                BuildField(TemplateFieldName.CorrespondenceAddress),
                BuildField(TemplateFieldName.CurrentYear),
                BuildField(TemplateFieldName.FailureInfoTZ),
                BuildField(TemplateFieldName.Colors));
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] {"RequestId", "UserId", DocumentGeneratorBase.UserInputFieldsParameterName};
        }
    }
}