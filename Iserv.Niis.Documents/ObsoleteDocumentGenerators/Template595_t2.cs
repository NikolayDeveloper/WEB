using System;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Documents;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Documents.UserInput.Abstract;
using Iserv.Niis.FileConverter.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using TemplateEngine.Docx;

namespace Iserv.Niis.Documents.ObsoleteDocumentGenerators
{
    [DocumentGenerator(5163, "006.014.3")]
    public class Template595_t2 : DocumentGeneratorBase
    {
        public Template595_t2(IExecutor executor, ITemplateFieldValueFactory templateFieldValueFactory, IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper) : base(executor,
            templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }

        protected override Content PrepareValue()
        {
            var data = GetData();
            return new Content(
                new FieldContent("DocumentNumber", data.RegisterNumber),
                BuildField(TemplateFieldName.President),
                BuildField(TemplateFieldName.CurrentUser),
                new FieldContent("ProtectionDocType", data.ProtectionDocumentType),
                new TableContent("RegisterTable",
                    Enumerable.Select(data.RegisterTable, r => new TableRowContent(
                        new FieldContent("Index", r.Index),
                        new FieldContent("PatentGosNumber", r.GosNumber),
                        new FieldContent("Barcode", r.Barcode),
                        new FieldContent("Addressee", r.Addressee),
                        new FieldContent("AddresseeAddress", r.Address),
                        new FieldContent("Note", r.Note)))
                ));
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId", DocumentGeneratorBase.UserInputFieldsParameterName };
        }

        private class ProtectionDocRegisterData
        {
            internal string ProtectionDocumentType { get; set; }
            internal string RegisterNumber { get; set; }
            internal List<RegisterTableRow> RegisterTable { get; set; }
        }

        private class RegisterTableRow
        {
            internal string Index { get; set; }
            internal string GosNumber { get; set; }
            internal string Barcode { get; set; }
            internal string Addressee { get; set; }
            internal string Address { get; set; }
            internal string Note { get; set; }
        }

        private ProtectionDocRegisterData GetData()
        {
            var result = new ProtectionDocRegisterData();

            var documentId = Convert.ToInt32((object) Parameters["DocumentId"]);
            var document = Executor.GetQuery<GetDocumentByIdQuery>().Process(q => q.Execute(documentId));

            #region Тип ОД

            var type = string.Empty;
            switch (document?.ProtectionDocType?.Code)
            {
                case DicProtectionDocTypeCodes.ProtectionDocTypeTrademarkCode:
                case DicProtectionDocTypeCodes.RequestTypeTrademarkCode:
                    type = "товарным знакам";
                    break;
                case DicProtectionDocTypeCodes.ProtectionDocTypeInventionCode:
                case DicProtectionDocTypeCodes.RequestTypeInventionCode:
                    type = "изобретениям";
                    break;
                case DicProtectionDocTypeCodes.ProtectionDocTypeUsefulModelCode:
                case DicProtectionDocTypeCodes.RequestTypeUsefulModelCode:
                    type = "полезным моделям";
                    break;
                case DicProtectionDocTypeCodes.ProtectionDocTypeIndustrialSampleCode:
                case DicProtectionDocTypeCodes.RequestTypeIndustrialSampleCode:
                    type = "промышленным образцам";
                    break;
                case DicProtectionDocTypeCodes.ProtectionDocTypeSelectionAchieveCode:
                case DicProtectionDocTypeCodes.RequestTypeSelectionAchieveCode:
                    type = "селекционным достижениям";
                    break;
                case DicProtectionDocTypeCodes.ProtectionDocTypeNameOfOriginCode:
                case DicProtectionDocTypeCodes.RequestTypeNameOfOriginCode:
                    type = "наименованиям мест происхождения товаров";
                    break;
            }
            result.ProtectionDocumentType = type;

            #endregion

            #region Данные таблицы

            result.RegisterNumber = document.DocumentNum;
            
            var tableData = new List<RegisterTableRow>();
            int i = 1;
            foreach (var protectionDocDocument in document.ProtectionDocs.OrderBy(pdd => pdd.ProtectionDoc.GosNumber))
            {
                var row = new RegisterTableRow
                {
                    Index = i++.ToString(),
                    Address = protectionDocDocument.ProtectionDoc.Addressee.Address,
                    Addressee = protectionDocDocument.ProtectionDoc.Addressee.NameRu,
                    Barcode = protectionDocDocument.ProtectionDoc.Barcode.ToString(),
                    GosNumber = protectionDocDocument.ProtectionDoc.GosNumber,
                    Note = string.Empty
                };
                tableData.Add(row);
            }
            result.RegisterTable = tableData;

            #endregion

            return result;
        }
    }
}
