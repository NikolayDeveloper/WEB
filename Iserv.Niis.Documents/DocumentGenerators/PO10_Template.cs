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
    /// Отчет о поиске ПО
    /// </summary>
    [DocumentGenerator(0, DicDocumentTypeCodes.PO10)]
    public class PO10_Template : DocumentGeneratorBase
    {
        public PO10_Template(
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
                BuildField(TemplateFieldName.Priority31),
                BuildField(TemplateFieldName.Priority32),
                BuildField(TemplateFieldName.Priority33),
                BuildField(TemplateFieldName.Mkpo51),
                BuildField(TemplateFieldName.RequestNameRu),
                BuildField(TemplateFieldName.RequestNameKz),
                BuildField(TemplateFieldName.Declarants),
                BuildField(TemplateFieldName.PatentAuthors),
                BuildField(TemplateFieldName.DocumentDateCreate),
                BuildField(TemplateFieldName.ExpertName)
            );
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { TemplateFieldNameConstants.RequestId };
        }
    }
}