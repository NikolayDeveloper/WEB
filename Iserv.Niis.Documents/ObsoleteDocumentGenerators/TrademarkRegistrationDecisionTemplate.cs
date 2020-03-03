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
    [DocumentGenerator(0, DicDocumentTypeCodes.TrademarkRegistrationDecision)]
    public class TrademarkRegistrationDecisionTemplate: DocumentGeneratorBase
    {
        public TrademarkRegistrationDecisionTemplate(IExecutor executor,
            ITemplateFieldValueFactory templateFieldValueFactory,
            IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper)
            : base(executor, templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }

        protected override Content PrepareValue()
        {
            return new Content(
                BuildField(TemplateFieldName.RequestDate),
                BuildField(TemplateFieldName.RequestNumber),
                BuildField(TemplateFieldName.DeputyDirectorName),
                BuildField(TemplateFieldName.Priority31),
                BuildField(TemplateFieldName.Priority32),
                BuildField(TemplateFieldName.Priority33),
                BuildField(TemplateFieldName.IcgsIndices),
                BuildField(TemplateFieldName.Disclaimer),
                BuildImage(TemplateFieldName.Image),
                BuildField(TemplateFieldName.Colors),
                BuildField(TemplateFieldName.DeclarantsAndAddress),
                BuildField(TemplateFieldName.Icgs511),
                BuildField(TemplateFieldName.HeadName),
                BuildField(TemplateFieldName.ExpertName));
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId" };
        }
    }
}
