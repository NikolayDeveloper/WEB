using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Documents;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Documents.Infrastructure;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    /// <summary>
    /// Шаблон QR-кода.
    /// </summary>
    [TemplateFieldName(TemplateFieldName.QrCode)]
    public class QrCode : TemplateFieldValueBase
    {
        private const string DocumentIdParameter = "DocumentId";
        private const string RequestIdParameter = "RequestId";

        private static readonly string[] QrCodeRequiredParameters = { RequestIdParameter, DocumentIdParameter };


        private readonly QrCodeGenerator _qrCodeGenerator;

        public QrCode(IExecutor executor) : base(executor)
        {
            _qrCodeGenerator = new QrCodeGenerator();
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return QrCodeRequiredParameters;
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            var documentId = (int) parameters[DocumentIdParameter];

            var document = Executor
                .GetQuery<GetDocumentByIdQuery>()
                .Process(query => query.Execute(documentId));

            var data = GetQrCodeData(parameters, document);
            var qrCodes = _qrCodeGenerator.BuildQrCodes(data);

            using (var memoryStream = new MemoryStream())
            {
                foreach (var qrCode in qrCodes)
                {
                    qrCode.Save(memoryStream, ImageFormat.Png);
                }

                return memoryStream.ToArray();
            }
        }

        private string GetQrCodeData(Dictionary<string, object> parameters, Domain.Entities.Document.Document document)
        {
            var builder = new StringBuilder();

            var request = Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.Execute((int)parameters["RequestId"]));

            var creationDate = document?.DateCreate;
            var outgoingNumber = document?.OutgoingNumber;
            var requestNumber = request?.RequestNum;

            builder.AppendLine("Исходящий документ №" + outgoingNumber);
            builder.AppendLine("Дата: " + creationDate.GetValueOrDefault().ToString("dd.MM.yyyy"));
            builder.AppendLine(string.Empty); // TODO: В том месте, откуда я взял код, было постоянно не берущееся null название документа. Поэтому просто пустая строка.
            builder.AppendLine("Заявка №" + requestNumber);
            builder.AppendLine("cabinet.kazpatent.kz");

            return builder.ToString();
        }
    }
}
