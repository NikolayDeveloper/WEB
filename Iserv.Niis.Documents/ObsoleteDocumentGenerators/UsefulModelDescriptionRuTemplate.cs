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
    [DocumentGenerator(6162, "OP_PAT_PM")]
    public class UsefulModelDescriptionRuTemplate: DocumentGeneratorBase
    {
        public UsefulModelDescriptionRuTemplate(IExecutor executor, ITemplateFieldValueFactory templateFieldValueFactory,
            IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper) : base(executor,
            templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }

        protected override Content PrepareValue()
        {
            return new Content(BuildField(TemplateFieldName.PatentGosNumber),
                BuildField(TemplateFieldName.RequestNumber),
                BuildField(TemplateFieldName.RequestDate),
                BuildField(TemplateFieldName.BulletinDate),
                BuildField(TemplateFieldName.BulletinNumber),
                BuildField(TemplateFieldName.Priority31),
                BuildField(TemplateFieldName.Priority32),
                BuildField(TemplateFieldName.Priority33),
                BuildField(TemplateFieldName.Priority86),
                BuildField(TemplateFieldName.PatentOwner),
                BuildField(TemplateFieldName.PatentNameRu),
                BuildField(TemplateFieldName.CurrentUser),
                BuildField(TemplateFieldName.Referat),
                BuildField(TemplateFieldName.TransferDate),
                BuildField(TemplateFieldName.IpcCodes),
                BuildField(TemplateFieldName.PatentAttorney),
                BuildField(TemplateFieldName.PatentAuthors));
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId" };
        }
    }
}
