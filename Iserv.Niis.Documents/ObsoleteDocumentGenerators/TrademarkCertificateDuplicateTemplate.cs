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
    [DocumentGenerator(5154, "GR_TZ_SVID_DUPLIKAT")]
    public class TrademarkCertificateDuplicateTemplate: DocumentGeneratorBase
    {
        public TrademarkCertificateDuplicateTemplate(IExecutor executor,
            ITemplateFieldValueFactory templateFieldValueFactory,
            IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper)
            : base(executor, templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }

        protected override Content PrepareValue()
        {
            return new Content(BuildField(TemplateFieldName.GosNumber),
                BuildField(TemplateFieldName.PatentOwner),
                BuildField(TemplateFieldName.PatentOwnerKz),
                BuildField(TemplateFieldName.ApplicantAddress),
                BuildField(TemplateFieldName.ApplicantAddressKz),
                BuildField(TemplateFieldName.Priority31WithoutCode),
                BuildField(TemplateFieldName.Priority32WithoutCode),
                BuildField(TemplateFieldName.Priority33WithoutCode),
                BuildField(TemplateFieldName.Mktu511),
                BuildField(TemplateFieldName.Referat),
                BuildField(TemplateFieldName.Colors),
                BuildField(TemplateFieldName.Disclaimer),
                BuildField(TemplateFieldName.RequestNumber),
                BuildField(TemplateFieldName.RequestDate),
                BuildField(TemplateFieldName.ValidDate),
                BuildField(TemplateFieldName.President),
                BuildField(TemplateFieldName.PresidentKz));
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId" };
        }
    }
}
