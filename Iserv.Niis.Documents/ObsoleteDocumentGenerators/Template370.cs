using System.Collections.Generic;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Documents;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Documents.Models;
using Iserv.Niis.Documents.UserInput.Abstract;
using Iserv.Niis.FileConverter.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using TemplateEngine.Docx;

namespace Iserv.Niis.Documents.ObsoleteDocumentGenerators
{
    [DocumentGenerator(4148, "006.014.0")]
    public class Template370 : DocumentGeneratorBase
    {

        public Template370(IExecutor executor, ITemplateFieldValueFactory templateFieldValueFactory,
            IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper)
            : base(executor, templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }

        protected override Content PrepareValue()
        {

            return new Content(
                BuildField(TemplateFieldName.CurrentUser),
                BuildField(TemplateFieldName.CurrentUserPhoneNumber),
                BuildField(TemplateFieldName.NSurnameLeader),
                BuildField(TemplateFieldName.CurrentDate),
                GetDocumentNumber(),
                TableTemplateContent(),
                GetTableTemplateContentCount()
                
            );
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId" };
        }

       

        private FieldContent GetTableTemplateContentCount()
        {
            var requestC = (IList<RequestApplicantInfoRecord>)TemplateFieldValueFactory
                .Create(TemplateFieldName.RequestApplicantInfoRecords_Complex).Get(Parameters);

            return new FieldContent("RequestCount", requestC.Count.ToString());
        }

        private FieldContent GetDocumentNumber()
        {
            var document = Executor.GetQuery<GetDocumentByIdQuery>()
                .Process(q => q.Execute((int) Parameters["DocumentId"]));

             return new FieldContent("DocumentNumber", document.DocumentNum);
        }

        // Метод для строки
        private TableContent TableTemplateContent()
        {
            var rows = (IList<RequestApplicantInfoRecord>)TemplateFieldValueFactory
                .Create(TemplateFieldName.RequestApplicantInfoRecords_Complex).Get(Parameters);

            var tableContent = new TableContent("RequestNTable");


            //List<string> rowsQ = new List<string>();
            //rowsQ.Add(TemplateFieldValueFactory
            //    .Create(TemplateFieldName.RequestNumber).Get(Parameters));
            //var rowIndex = 0;
            //foreach (var tableRow in rows)
            //{
            //    tableContent.AddRow(
            //        new FieldContent("RequestNum", tableRow.RequestNum),
            //        new FieldContent("Declarants", tableRow.Declarant),
            //        new FieldContent("RequestDate", tableRow.RequestDate.ToString()),
            //        new FieldContent("RowIncrement", (++rowIndex).ToString())
            //    ); 
            //} 


            for (var i = 0; i < rows.Count; i++)

            {
                var tableRow = rows[i];


                tableContent.AddRow(
                    new FieldContent("RequestNum", tableRow.RequestNum),
                    new FieldContent("DeclarantShortInfo", tableRow.DeclarantShortInfo),
                    new FieldContent("RowIncrement", (i + 1).ToString()),
                    new FieldContent("RequestDate", tableRow.RequestDate)
            

               );
            }

            return tableContent;
        }
    }
}