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
    [DocumentGenerator(5166, "PAT_AVT_CD_RASTENIE_D")]
    public class AgriculturalSelectiveAchievementsAuthorsCertificateDuplicateTemplate: DocumentGeneratorBase
    {
        public AgriculturalSelectiveAchievementsAuthorsCertificateDuplicateTemplate(IExecutor executor,
            ITemplateFieldValueFactory templateFieldValueFactory,
            IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper)
            : base(executor, templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }

        protected override Content PrepareValue()
        {
            return new Content(BuildField(TemplateFieldName.PatentAuthors),
                BuildField(TemplateFieldName.PatentAuthorsKz),
                BuildField(TemplateFieldName.AuthorNumber),
                BuildField(TemplateFieldName.GosNumber),
                BuildField(TemplateFieldName.PatentNameKz),
                BuildField(TemplateFieldName.PatentNameRu),
                BuildField(TemplateFieldName.PatentOwnerKz),
                BuildField(TemplateFieldName.PatentOwner),
                BuildField(TemplateFieldName.RequestNumber),
                BuildField(TemplateFieldName.RequestDate),
                BuildField(TemplateFieldName.President),
                BuildField(TemplateFieldName.PresidentKz));
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId" };
        }
    }
}
