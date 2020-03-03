using System;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Common;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Contracts;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Documents;
using Iserv.Niis.Documents.DocumentsBusinessLogic.ProtectionDocs;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Documents.Helpers;
using Iserv.Niis.Documents.UserInput.Abstract;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.FileConverter.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using TemplateEngine.Docx;

namespace Iserv.Niis.Documents.DocumentGenerators
{
    [DocumentGenerator(0, DicDocumentTypeCodes.IN01_vn_opis_V1_19)]
    public class InternalRegisterTemplate: DocumentGeneratorBase
    {
        public InternalRegisterTemplate(IExecutor executor,
            ITemplateFieldValueFactory templateFieldValueFactory,
            IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper)
            : base(executor, templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }

        protected override Content PrepareValue()
        {
            var templateData = GetTemplateData();
            if (!Enumerable.Any(templateData.Table))
            {
                return new Content(
                    new FieldContent(nameof(templateData.ObjectNumber), templateData.ObjectNumber),
                    new FieldContent(nameof(templateData.DateCreate), templateData.DateCreate));
            }

            return new Content(
                new FieldContent(nameof(templateData.ObjectNumber), templateData.ObjectNumber),
                new FieldContent(nameof(templateData.ObjectTypeNameRu), templateData.ObjectTypeNameRu),
                new FieldContent(nameof(templateData.DateCreate), templateData.DateCreate),
                BuildField(TemplateFieldName.CurrentUser),
                new TableContent(nameof(templateData.Table), Enumerable.Select(templateData.Table, d => new TableRowContent(
                    new FieldContent(nameof(d.Index), d.Index),
                    new FieldContent(nameof(d.TypeNameRu), d.TypeNameRu),
                    new FieldContent(nameof(d.PageCount), d.PageCount)))));
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId" };
        }

        private TemplateData GetTemplateData()
        {
            var ownerType = (Owner.Type)(int)Parameters["OwnerType"];
            var ownerId = Convert.ToInt32((object) Parameters["RequestId"]);
            List<Document> documents = new List<Document>();
            var result = new TemplateData
            {
                DateCreate = DateTimeOffset.Now.ToString("dd MMMM yyyy HH:mm:ss", CurrentCulture.CurrentCultureInfo),
                
            };
            switch (ownerType)
            {
                case Owner.Type.Request:
                    var request = Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.Execute(ownerId));
                    result.ObjectNumber = request?.RequestNum ?? request?.Barcode.ToString() ?? "Без номера";
                    result.ObjectTypeNameRu = "по заявке";
                    documents = Executor.GetQuery<GetDocumentsByRequestIdQuery>().Process(q => q.Execute(ownerId));
                    break;
                case Owner.Type.ProtectionDoc:
                    var protectionDoc = Executor.GetQuery<GetProtectionDocByIdQuery>().Process(q => q.Execute(ownerId));
                    result.ObjectNumber = protectionDoc?.GosNumber ??
                                           protectionDoc?.RegNumber ??
                                           protectionDoc?.Barcode.ToString() ?? "Без номера";
                    result.ObjectTypeNameRu = "по охранному документу";
                    documents = Executor.GetQuery<GetDocumentsByProtectionDocIdQuery>().Process(q => q.Execute(ownerId));
                    break;
                case Owner.Type.Contract:
                    var contract = Executor.GetQuery<GetContractByIdQuery>().Process(q => q.Execute(ownerId));
                    result.ObjectNumber = contract?.GosNumber ??
                                           contract?.ContractNum ?? contract?.Barcode.ToString() ?? "Без номера";
                    result.ObjectTypeNameRu = "по договору";
                    documents = Executor.GetQuery<GetDocumentsByContractIdQuery>().Process(q => q.Execute(ownerId));
                    break;
                default:
                    throw new NotImplementedException();
            }

            var i = 1;
            result.Table = documents.Select(d => new TableData
            {
                Index = $"{i++}",
                TypeNameRu = d.Type?.NameRu ?? string.Empty,
                CopyCount = $"{d.CopyCount ?? 0}",
                PageCount = $"{d.PageCount ?? 0}",
                DocCreateDate = d.DateCreate.ToTemplateDateFormat(),
                UserCreate = d.Workflows?.FirstOrDefault(w => w.CurrentStage.IsFirst)?.CurrentUser?.NameRu ?? string.Empty
            }).ToList();
            return result;
        }

        private class TemplateData
        {
            internal string ObjectTypeNameRu { get; set; }
            internal string ObjectNumber { get; set; }
            internal string DateCreate { get; set; }
            internal List<TableData> Table { get; set; }

        }

        private class TableData
        {
            internal string Index { get; set; }
            internal string TypeNameRu { get; set; }
            internal string CopyCount { get; set; }
            internal string PageCount { get; set; }
            internal string DocCreateDate { get; set; }
            internal string UserCreate { get; set; }
        }
    }
}
 