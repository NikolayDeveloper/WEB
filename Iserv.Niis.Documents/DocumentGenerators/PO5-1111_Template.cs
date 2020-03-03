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
    /// Решение и заключение о выдаче патента на промышленный образец.
    /// </summary>
    [DocumentGenerator(0, DicDocumentTypeCodes.PO5_1111)]
    public class PO5_1111 : DocumentGeneratorBase
    {
        public PO5_1111(
            IExecutor executor,
            ITemplateFieldValueFactory templateFieldValueFactory,
            IFileConverter fileConverter,
            IDocxTemplateHelper docxTemplateHelper) : base(executor, templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
        }

        protected override Content PrepareValue()
        {
            return new Content(
                BuildField(TemplateFieldName.DocumentDescription),
                BuildField(TemplateFieldName.RequestNumber),
                BuildField(TemplateFieldName.RequestDate),
                BuildField(TemplateFieldName.ExpertName),
                //BuildField(TemplateFieldName.Authors),
                BuildImage(TemplateFieldName.Image),
                BuildField(TemplateFieldName.Priority31WithoutCode),
                BuildField(TemplateFieldName.Priority32WithoutCode),
                BuildField(TemplateFieldName.Priority33WithoutCode),
                BuildField(TemplateFieldName.Declarants),
                BuildField(TemplateFieldName.PatentAuthors),
                BuildField(TemplateFieldName.PatentAuthorsEn),
                BuildField(TemplateFieldName.Mkpo51),
                BuildField(TemplateFieldName.RequestNameRu),
                BuildField(TemplateFieldName.RequestNameKz)
            );
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { TemplateFieldNameConstants.RequestId };
        }
    }
}
