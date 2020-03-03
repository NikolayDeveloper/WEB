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
    /// 35_Уведомление о принятии решения о выдаче патента на ПМ (УВ-Кпм)
    /// </summary>
    [DocumentGenerator(0, DicDocumentTypeCodes.UV_KPM)]
    public class UsefulModelPatentDecisionNotice : DocumentGeneratorBase
    {
        public UsefulModelPatentDecisionNotice(
            IExecutor executor,
            ITemplateFieldValueFactory templateFieldValueFactory,
            IFileConverter fileConverter,
            IDocxTemplateHelper docxTemplateHelper) 
            : base(executor, templateFieldValueFactory, fileConverter, docxTemplateHelper)
        { }

        protected override Content PrepareValue()
        {
            return new Content(
                BuildField(TemplateFieldName.RequestNumber),
                BuildField(TemplateFieldName.RequestDate),
                BuildField(TemplateFieldName.CorrespondenceContact),
                BuildField(TemplateFieldName.CorrespondenceAddress),
                BuildField(TemplateFieldName.CustomerPhone),
                BuildField(TemplateFieldName.CustomerEmail),
                BuildField(TemplateFieldName.RequestNameRu),
                BuildField(TemplateFieldName.RequestNameKz),
                BuildField(TemplateFieldName.Declarants),
                BuildField(TemplateFieldName.DeclarantsAddress),
                BuildField(TemplateFieldName.DocumentNumber),
                BuildField(TemplateFieldName.CurrentUser),
                BuildField(TemplateFieldName.RequestNameEn),
                BuildField(TemplateFieldName.PatentAuthorsEn),
                BuildField(TemplateFieldName.PatentAuthorsKz),
                BuildField(TemplateFieldName.PatentAuthors),
                BuildField(TemplateFieldName.PatentOwner),
                BuildField(TemplateFieldName.PatentOwnerKz),
                BuildField(TemplateFieldName.PatentOwnerEn));
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { TemplateFieldNameConstants.RequestId };
        }
    }
}
