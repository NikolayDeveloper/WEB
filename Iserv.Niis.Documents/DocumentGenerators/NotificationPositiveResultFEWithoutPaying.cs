using Iserv.Niis.Common.Codes;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.UserInput.Abstract;
using Iserv.Niis.FileConverter.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using System.Collections.Generic;
using Iserv.Niis.Documents.Enums;
using TemplateEngine.Docx;


namespace Iserv.Niis.Documents.DocumentGenerators
{
    [DocumentGenerator(0, DicDocumentTypeCodes.NotificationForPaymentlessPozitiveFormalExamination)]
    public class NotificationPositiveResultFEWithoutPaying : DocumentGeneratorBase
    {
        public NotificationPositiveResultFEWithoutPaying(IExecutor executor, ITemplateFieldValueFactory templateFieldValueFactory,
       IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper) : base(executor,
       templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }


        protected override Content PrepareValue()
        {
            return new Content(
                BuildField(TemplateFieldName.RequestDate),
                BuildField(TemplateFieldName.RequestNumber),
                BuildField(TemplateFieldName.CorrespondenceContact),
                BuildField(TemplateFieldName.CorrespondenceAddress),
                BuildField(TemplateFieldName.RequestNameRu),
                BuildField(TemplateFieldName.Declarants),
                BuildField(TemplateFieldName.Authors),
                BuildField(TemplateFieldName.Priority31WithoutCode),
                BuildField(TemplateFieldName.Priority32WithoutCode),
                BuildField(TemplateFieldName.Priority33WithoutCode),
                //BuildField(TemplateFieldName.Priority85WithoutCode), TODO: implement
                BuildField(TemplateFieldName.Priority86WithoutCode),
                BuildField(TemplateFieldName.CurrentUser),
                BuildField(TemplateFieldName.RequestDateCreate),
                BuildField(TemplateFieldName.RequestDate),
                BuildField(TemplateFieldName.DeclarantsAddress),
                BuildField(TemplateFieldName.DocumentNum));
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId", "UserId" };
        }
    }
}
