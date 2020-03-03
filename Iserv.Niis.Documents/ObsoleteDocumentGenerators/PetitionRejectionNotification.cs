using System;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Documents;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Documents.Helpers;
using Iserv.Niis.Documents.UserInput.Abstract;
using Iserv.Niis.FileConverter.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using TemplateEngine.Docx;

namespace Iserv.Niis.Documents.ObsoleteDocumentGenerators
{
    [DocumentGenerator(12024, "UVED_NEPRINJATIE")]
    public class PetitionRejectionNotification: DocumentGeneratorBase
    {
        private int _requestId;

        public PetitionRejectionNotification(
            IExecutor executor, 
            ITemplateFieldValueFactory templateFieldValueFactory,
            IFileConverter fileConverter, 
            IDocxTemplateHelper docxTemplateHelper) 
            : base(executor, templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }

        protected override Content PrepareValue()
        {
            _requestId = Convert.ToInt32((object) Parameters["RequestId"]);
            var tariffCodes = new[] {"NEW_019", "NEW_080", "NEW_079"};
            var documentCodes = new[] { "001.004G.2", "001.004G_1", "001.004G_2" };
            var request = Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.Execute(_requestId));
            var document = Executor.GetQuery<GetDocumentByIdQuery>()
                .Process(q => q.Execute(Convert.ToInt32((object) Parameters["DocumentId"])));
            var petition = Executor.GetQuery<GetDocumentsByRequestIdQuery>().Process(q => q.Execute(_requestId))
                .FirstOrDefault(d => documentCodes.Contains(d.Type.Code));
            var invoice = request.PaymentInvoices.FirstOrDefault(pi => tariffCodes.Contains(pi.Tariff.Code));

            var userPosts = new string[0];
            var userNames = new string[0];
            var cwfs = document?.CurrentWorkflows;
            if (cwfs != null)
            {
                userPosts = cwfs.Select(d => d.CurrentUser?.Position?.PositionType?.NameRu ?? string.Empty).ToArray();
                userNames = cwfs.Select(d => d.CurrentUser?.NameRu ?? string.Empty).ToArray();
            }

            return new Content(
                new FieldContent("PetitionName",
                    petition?.Type?.NameRu?.Replace("Ходатайство ", string.Empty) ?? string.Empty),
                new FieldContent("PetitionIncomingNumber",
                    petition?.IncomingNumber ?? petition?.IncomingNumberFilial ?? string.Empty),
                new FieldContent("PetitionIncomingDate", petition?.DateCreate.ToTemplateDateFormat() ?? string.Empty),
                new FieldContent("TariffName", invoice?.Tariff?.NameRu ?? string.Empty),
                new FieldContent("UserPost", string.Join(", ", userPosts)),
                new FieldContent("UserName", string.Join(", ", userNames)),
                new FieldContent("Date",
                    document?.DateCreate.ToTemplateDateFormat() ?? DateTimeOffset.Now.ToTemplateDateFormat())
            );
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId", "DocumentId" };
        }
    }
}
