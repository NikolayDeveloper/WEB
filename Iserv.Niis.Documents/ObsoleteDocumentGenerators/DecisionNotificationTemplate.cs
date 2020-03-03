using System;
using System.Collections.Generic;
using Iserv.Niis.Common;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Dictionaries;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Documents;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Documents.Helpers;
using Iserv.Niis.Documents.UserInput.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.FileConverter.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using TemplateEngine.Docx;

namespace Iserv.Niis.Documents.ObsoleteDocumentGenerators
{
    [DocumentGenerator(0, DicDocumentTypeCodes.DecisionNotification)]
    public class DecisionNotificationTemplate: DocumentGeneratorBase
    {
        private DicTariff Tariff { get; set; }
        private Document Document { get; set; }
        public DecisionNotificationTemplate(IExecutor executor,
            ITemplateFieldValueFactory templateFieldValueFactory,
            IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper)
            : base(executor, templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }

        protected override Content PrepareValue()
        {
            Tariff = Executor.GetQuery<GetDicTariffByCodeQuery>()
                .Process(q => q.Execute(DicTariffCodes.TrademarNmptRegistrationAndPublishing));
            GetDocument();

            return new Content(BuildField(TemplateFieldName.CorrespondenceAddress),
                BuildField(TemplateFieldName.CorrespondenceContact),
                BuildField(TemplateFieldName.RequestNumber),
                BuildField(TemplateFieldName.DivisionHeadPost),
                BuildField(TemplateFieldName.DivisionHeadPostKz),
                BuildField(TemplateFieldName.HeadName),
                BuildField(TemplateFieldName.ExpertName),
                BuildField(TemplateFieldName.ExpertPhone),
                new FieldContent("TariffPrice", GetTariffPrice().ToString(CurrentCulture.CurrentCultureInfo)),
                new FieldContent("TariffNds", GetNds().ToString(CurrentCulture.CurrentCultureInfo)),
                new FieldContent("CheckNumber", GetDocumentNumber()),
                new FieldContent("CheckDate", GetDocumentSendDate()));
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId" };
        }

        private decimal GetTariffPrice()
        {
            return Tariff?.Price ?? 0;
        }

        private decimal GetNds()
        {
            return (Tariff?.Price ?? 0) * (decimal) 0.12;
        }

        private void GetDocument()
        {
            var documentId = Convert.ToInt32((object) Parameters["DocumentId"]);
            Document = Executor.GetQuery<GetDocumentByIdQuery>().Process(q => q.Execute(documentId));
        }

        private string GetDocumentNumber()
        {
            return Document?.NumberForPayment ?? string.Empty;
        }

        private string GetDocumentSendDate()
        {
            return Document?.PaymentDate?.ToTemplateDateFormat() ?? string.Empty;
        }
    }
}
