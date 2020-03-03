using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    [DocumentGenerator(0, DicDocumentTypeCodes.NotificationOfDecisionPatentGrant)]
    public class NotificationOfDecisionPatentGrant : DocumentGeneratorBase
    {
        public NotificationOfDecisionPatentGrant(
            IExecutor executor,
            ITemplateFieldValueFactory templateFieldValueFactory,
            IFileConverter fileConverter,
            IDocxTemplateHelper docxTemplateHelper) : base(executor, templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
        }

        protected override Content PrepareValue()
        {
            return new Content(
                BuildField(TemplateFieldName.RequestNumber),
                BuildField(TemplateFieldName.RequestDate),
                BuildField(TemplateFieldName.RequestNameRu),
                BuildField(TemplateFieldName.RequestNameEn),
                BuildField(TemplateFieldName.RequestNameKz),
                BuildField(TemplateFieldName.DeclarantsAddress),
                BuildField(TemplateFieldName.DocumentNum),
                BuildField(TemplateFieldName.CurrentUser),
                BuildField(TemplateFieldName.PatentAuthors),
                BuildField(TemplateFieldName.PatentAuthorsKz),
                BuildField(TemplateFieldName.PatentAuthorsEn),
                BuildField(TemplateFieldName.Declarants),
                BuildField(TemplateFieldName.DeclarantsKz),
                BuildField(TemplateFieldName.DeclarantsEn),
                BuildField(TemplateFieldName.DeclarantsAddress),
                BuildField(TemplateFieldName.CorrespondenceContact),
                BuildField(TemplateFieldName.CorrespondenceAddress),
                BuildField(TemplateFieldName.CustomerPhone),
                BuildField(TemplateFieldName.CustomerEmail));
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "DocumentId", "RequestId", UserInputFieldsParameterName };
        }
    }
}
