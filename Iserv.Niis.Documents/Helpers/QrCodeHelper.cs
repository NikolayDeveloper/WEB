using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Aspose.Words;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Dictionaries;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Documents;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.Documents.Infrastructure;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using Iserv.Niis.Common.Codes;

namespace Iserv.Niis.Documents.Helpers
{
    public class QrCodeHelper
    {
        private readonly QrCodeGenerator _qrCodeGenerator;
        private readonly DocxInsertHelper _docxInsertHelper;
        private readonly IExecutor _executor;
        private readonly string _documentCode;

        public QrCodeHelper(string documentCode, IExecutor executor)
        {
            _qrCodeGenerator = new QrCodeGenerator();
            _docxInsertHelper = new DocxInsertHelper();
            _documentCode = documentCode;
            _executor = executor;
        }

        public void InsertToHeader(MemoryStream stream, Dictionary<string, object> parameters)
        {
            if (stream == null)
                throw new ArgumentNullException();

            stream.Seek(0, SeekOrigin.Begin);

            var document = _executor.GetQuery<GetDocumentByIdQuery>()
                .Process(q => q.Execute((int)parameters["DocumentId"]));

            string data = GetDataHedear(parameters, document);

            var qrCodes = _qrCodeGenerator.BuildQrCodes(data);
            if (qrCodes == null) return;

            Document asposeDocument = new Document(stream, new LoadOptions { LoadFormat = LoadFormat.Docx });
            DocumentBuilder builder = new DocumentBuilder(asposeDocument);

            foreach (var qrCode in qrCodes)
                _docxInsertHelper.InsertImageToHeader(qrCode, builder);

            builder.InsertParagraph();
            builder.Font.Size = 10;
            builder.Writeln(document.Barcode.ToString());

            UpdateSourceStream(stream, asposeDocument);
        }

        public void InsertToFooter(MemoryStream stream, Dictionary<string, object> parameters)
        {
            if (stream == null)
                throw new ArgumentNullException();

            var dbDocument = _executor.GetQuery<GetDocumentByIdQuery>()
                .Process(q => q.Execute((int)parameters["DocumentId"]));

            string data = GetDataFooter(parameters, dbDocument);

            var qrCodes = _qrCodeGenerator.BuildQrCodes(data);

            Document document = new Document(stream);
            DocumentBuilder builder = new DocumentBuilder(document);

            foreach (var qrCode in qrCodes)
                _docxInsertHelper.InsertImageToFooter(qrCode, builder);

            document.UpdateFields();
            document.Save(stream, SaveFormat.Docx);
        }

        private bool IsCreateStage(Dictionary<string, object> parameters)
        {
            var document = _executor.GetQuery<GetDocumentByIdQuery>()
                .Process(q => q.Execute((int) parameters["DocumentId"]));

            var currentWorkflows = document?.CurrentWorkflows;

            return currentWorkflows != null &&
                currentWorkflows.Any(cwf => cwf.CurrentStage.Code == RouteStageCodes.DocumentOutgoing_01_1);
        }

        private string GetDataHedear(Dictionary<string, object> parameters, Domain.Entities.Document.Document document)
        {
            StringBuilder builder = new StringBuilder();
            
            var request = _executor.GetQuery<GetRequestByIdQuery>().Process(q => q.Execute((int)parameters["RequestId"]));
            var documentType = _executor.GetQuery<GetDocumentTypesByCodeQuery>().Process(q => q.Execute(_documentCode));
            // TODO: для каждого получаемого данного создать отдельный TemplateFieldValue
            string documentName = documentType.FirstOrDefault()?.NameRu;
            var creationDate = document?.DateCreate;
            var outgoingNumber = document?.OutgoingNumber;
            string requestNumber = request?.RequestNum;


            builder.AppendLine("Исходящий документ №" + outgoingNumber);
            builder.AppendLine("Дата: " + creationDate.GetValueOrDefault().ToString("dd.MM.yyyy"));
            builder.AppendLine(documentName);
            builder.AppendLine("Заявка №" + requestNumber);
            builder.AppendLine("cabinet.kazpatent.kz");

            return builder.ToString();
        }

        string GetDataFooter(Dictionary<string, object> parameters, Domain.Entities.Document.Document document)
        {
            StringBuilder builder = new StringBuilder();

            var request = _executor.GetQuery<GetRequestByIdQuery>().Process(q => q.Execute((int)parameters["RequestId"]));
            var documentType = _executor.GetQuery<GetDocumentTypesByCodeQuery>().Process(q => q.Execute(_documentCode));
            // TODO: для каждого получаемого данного создать отдельный TemplateFieldValue
            string documentName = documentType.FirstOrDefault()?.NameRu;
            var creationDate = document?.DateCreate;
            var outgoingNumber = document?.OutgoingNumber;
            string requestNumber = request?.RequestNum;


            builder.AppendLine("Исходящий документ №" + outgoingNumber);
            builder.AppendLine("Дата: " + creationDate.GetValueOrDefault().ToString("dd.MM.yyyy"));
            builder.AppendLine(documentName);
            builder.AppendLine("Заявка №" + requestNumber);
            builder.AppendLine("cabinet.kazpatent.kz");

            return builder.ToString();
        }

        private void UpdateSourceStream(MemoryStream stream, Document document)
        {
            using (var outputMemoryStream = new MemoryStream())
            {
                document.Save(outputMemoryStream, SaveFormat.Docx);
                stream.Seek(0, SeekOrigin.Begin);
                stream.Write(outputMemoryStream.ToArray(), 0, (int)outputMemoryStream.Length);
                stream.SetLength(stream.Position);
            }
        }
    }
}
