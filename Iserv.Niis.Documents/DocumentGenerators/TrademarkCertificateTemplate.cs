using System.Collections.Generic;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Documents.UserInput.Abstract;
using Iserv.Niis.FileConverter.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using TemplateEngine.Docx;

namespace Iserv.Niis.Documents.DocumentGenerators
{
    [DocumentGenerator(5154, DicDocumentTypeCodes.TrademarkCertificate)]
    public class TrademarkCertificateTemplate: DocumentGeneratorBase
    {
        public TrademarkCertificateTemplate(IExecutor executor,
            ITemplateFieldValueFactory templateFieldValueFactory,
            IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper)
            : base(executor, templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {

        }

        protected override Content PrepareValue()
        {
            return new Content(
                BuildImage(TemplateFieldName.Image),
                BuildQrCode(TemplateFieldName.QrCode),
                BuildField(TemplateFieldName.GosNumber),
                BuildField(TemplateFieldName.GosDate),
                BuildField(TemplateFieldName.BulletinDate),
                BuildField(TemplateFieldName.RequestNumber),
                BuildField(TemplateFieldName.RequestDate),
                BuildField(TemplateFieldName.Priority300),
                BuildField(TemplateFieldName.PatentOwner),
                BuildField(TemplateFieldName.PatentOwnerKz),
                BuildField(TemplateFieldName.PatentOwnerEn),
                BuildField(TemplateFieldName.ValidDate),
                BuildField(TemplateFieldName.Icgs511),
                BuildField(TemplateFieldName.ColorsKz),
                BuildField(TemplateFieldName.Colors),
                BuildField(TemplateFieldName.ColorsEn)

                /*BuildField(TemplateFieldName.GosNumber),
                BuildField(TemplateFieldName.ApplicantAddress),
                BuildField(TemplateFieldName.ApplicantAddressKz),
                BuildField(TemplateFieldName.Referat),
                BuildField(TemplateFieldName.Colors),
                BuildField(TemplateFieldName.Disclaimer),
                BuildField(TemplateFieldName.ValidDate),
                BuildField(TemplateFieldName.President),
                BuildField(TemplateFieldName.PresidentKz)*/
                );
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId" };
        }
    }
}
