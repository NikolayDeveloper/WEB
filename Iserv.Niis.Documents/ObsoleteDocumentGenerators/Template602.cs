using System;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Common;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Documents;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Documents.Helpers;
using Iserv.Niis.Documents.UserInput.Abstract;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.FileConverter.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using TemplateEngine.Docx;

namespace Iserv.Niis.Documents.ObsoleteDocumentGenerators
{
    [DocumentGenerator(1971, "IZ_POISK")]
    public class Template602 : DocumentGeneratorBase
    {
        private List<Document> _requestDocuments;
        private List<RequestCustomer> _requestCustomers;
        public Template602(IExecutor executor, ITemplateFieldValueFactory templateFieldValueFactory,
          IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper) : base(executor,
          templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }
        protected override Content PrepareValue()
        {
            _requestDocuments = Executor.GetQuery<GetDocumentsByRequestIdQuery>()
                .Process(q => q.Execute(Convert.ToInt32((object) Parameters["RequestId"])));
            _requestCustomers = Executor.GetQuery<GetRequestCustomersByRequestIdQuery>()
                .Process(q => q.Execute(Convert.ToInt32((object) Parameters["RequestId"])));
            var declarants = GetDeclarants();
            var descriptionPageCount = GetDocumentPageCountByCode("001.072");
            var formulaPageCount = GetDocumentPageCountByCode("001.064");
            var figurespageCount = GetDocumentPageCountByCode("001.053");
            var earlyRegs = GetEarlyRegs();
            var priority32InWords = Get32PriorityInWords(earlyRegs);
            var priority32 = Get32Priority(earlyRegs);
            var priority31 = Get31Priority(earlyRegs);
            return new Content(
                BuildField(TemplateFieldName.DocumentNumber),
                BuildField(TemplateFieldName.RequestNumber),
                BuildField(TemplateFieldName.RequestDate),
                BuildField(TemplateFieldName.DocumentDateCreate),
                BuildField(TemplateFieldName.RequestDateInWords),
                BuildField(TemplateFieldName.RequestNameRu),
                new FieldContent("Declarants", GetCustomerNames(declarants)),
                new FieldContent("DeclarantCountries", GetCustomerCountries(declarants)),
                new FieldContent("DescriptionPageCount", descriptionPageCount.ToString()),
                new FieldContent("FormulaPageCount", formulaPageCount.ToString()),
                new FieldContent("FigurePageCount", figurespageCount.ToString()),
                new FieldContent("32InWords", string.Join(Environment.NewLine, (IEnumerable<string>) priority32InWords)),
                new FieldContent("32", string.Join(Environment.NewLine, (IEnumerable<string>) priority32)),
                new FieldContent("31", string.Join(Environment.NewLine, (IEnumerable<string>) priority31))
            );
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId", "DocumentId" };
        }

        protected int GetDocumentPageCountByCode(string code)
        {
            var document = Enumerable.FirstOrDefault<Document>(_requestDocuments, d => d.Type.Code == code);
            return document?.PageCount ?? 0;
        }

        protected List<RequestEarlyReg> GetEarlyRegs()
        {
            var request = Executor.GetQuery<GetRequestByIdQuery>()
                .Process(q => q.Execute(Convert.ToInt32((object) Parameters["RequestId"])));
            var result = request.EarlyRegs.Where(er => er.EarlyRegType.Code == "20.2");

            return result.ToList();
        }

        protected List<string> Get32PriorityInWords(List<RequestEarlyReg> earlyRegs)
        {
            var i = 1;
            return earlyRegs.Select(er =>$"{i++}. {er.RegDate?.ToString("D", CurrentCulture.CurrentCultureInfo)}").ToList();
        }

        protected List<string> Get32Priority(List<RequestEarlyReg> earlyRegs)
        {
            return earlyRegs.Select(er => er.RegDate.ToTemplateDateFormat()).ToList();
        }

        protected List<string> Get31Priority(List<RequestEarlyReg> earlyRegs)
        {
            var i = 1;
            return earlyRegs.Select(er => $"{i++}. {er.RegNumber}").ToList();
        }

        protected List<RequestCustomer> GetDeclarants()
        {
            var result = Enumerable.Where<RequestCustomer>(_requestCustomers, rc => rc.CustomerRole.Code == DicCustomerRoleCodes.Declarant);

            return result.ToList();
        }

        protected string GetCustomerNames(List<RequestCustomer> requestCustomers)
        {
            return requestCustomers.Any()
                ? string.Join(Environment.NewLine, requestCustomers.Select(rc => rc.Customer.NameRu))
                : string.Empty;
        }

        protected string GetCustomerCountries(List<RequestCustomer> requestCustomers)
        {
            return requestCustomers.Any()
                ? string.Join(Environment.NewLine, requestCustomers.Select(rc => rc.Customer.Country.NameRu))
                : string.Empty;
        }
    }
}
