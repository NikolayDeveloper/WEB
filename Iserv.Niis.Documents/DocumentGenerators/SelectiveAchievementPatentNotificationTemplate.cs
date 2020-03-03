using System.Collections.Generic;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Constants;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Documents.UserInput.Abstract;
using Iserv.Niis.FileConverter.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using TemplateEngine.Docx;

namespace Iserv.Niis.Documents.DocumentGenerators
{
    /// <summary>
    /// 4.1_Уведомление о выдаче патента на селекционное достижение
    /// </summary>
    [DocumentGenerator(0, DicDocumentTypeCodes.SelectiveAchievementPatentNotification)]
    public class SelectiveAchievementPatentNotificationTemplate : DocumentGeneratorBase
    {
        public SelectiveAchievementPatentNotificationTemplate(
            IExecutor executor,
            ITemplateFieldValueFactory templateFieldValueFactory,
            IFileConverter fileConverter,
            IDocxTemplateHelper docxTemplateHelper)
            : base(executor, templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
        }

        protected override Content PrepareValue()
        {
            return new Content(
                BuildField(TemplateFieldName.RequestNumber),
                BuildField(TemplateFieldName.RequestDate),
                BuildField(TemplateFieldName.CorrespondenceContact),
                BuildField(TemplateFieldName.CorrespondenceAddress),
                BuildField(TemplateFieldName.CorrespondencePhone),
                BuildField(TemplateFieldName.CorrespondenceEmail),
                BuildField(TemplateFieldName.RequestNameRu),
                BuildField(TemplateFieldName.RequestNameKz),
                BuildField(TemplateFieldName.RequestNameEn),
                BuildField(TemplateFieldName.ExpertName),
                BuildField(TemplateFieldName.Declarants),
                BuildField(TemplateFieldName.DeclarantsAddress),
                BuildField(TemplateFieldName.DocumentNumber),
                BuildField(TemplateFieldName.DocumentDateCreate),
                BuildField(TemplateFieldName.PatentAuthors),
                BuildField(TemplateFieldName.PatentAuthorsKz),
                BuildField(TemplateFieldName.PatentAuthorsEn),
                BuildField(TemplateFieldName.PatentOwner),
                BuildField(TemplateFieldName.PatentOwnerEn),
                BuildField(TemplateFieldName.PatentOwnerKz)
            );
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { TemplateFieldNameConstants.RequestId };
        }
    }
}