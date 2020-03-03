using System;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Administration;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Documents;
using Iserv.Niis.Documents.DocumentsBusinessLogic.ExpertSearch;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Documents.Helpers;
using Iserv.Niis.Documents.UserInput.Abstract;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.FileConverter.Abstract;
using Iserv.Niis.FileStorage.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using TemplateEngine.Docx;

namespace Iserv.Niis.Documents.ConclusionGenerators
{
    [DocumentGenerator(4247, DicDocumentTypeCodes.InventionSearchReport)]
    public class InventionExpertSearchReport: DocumentGeneratorBase, IComplexDataDocumentGenerator
    {
        private const int CustomerRoleid = 1;
        private const int SupervisorId = 2;

        public InventionExpertSearchReport(IExecutor executor, ITemplateFieldValueFactory templateFieldValueFactory, IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper, IFileStorage fileStorage) : base(executor, templateFieldValueFactory, fileConverter, docxTemplateHelper)
        {
            QrCodePosition = QrCodePosition.Header;
        }

        protected override Content PrepareValue()
        {
            var templateData = GetTemplateData();

            return new Content(
                //new FieldContent(nameof(TemplateData.RequestNameRu), templateData.RequestNameRu),
                //BuildField(TemplateFieldName.CurrentUser),
                //new FieldContent(nameof(TemplateData.Declarants), templateData.Declarants),
                new FieldContent(nameof(TemplateData.RequestNumber), templateData.RequestNumber),
                new FieldContent(nameof(TemplateData.CurrentYear), templateData.CurrentYear),
                new FieldContent(nameof(TemplateData.RequestDate), templateData.RequestDate),
                new FieldContent(nameof(TemplateData.CustomerNameRu), templateData.CustomerNameRu),
                new FieldContent(nameof(TemplateData.CustomerCountryCode), templateData.CustomerCountryCode),
                new FieldContent(nameof(TemplateData.ExpertName), templateData.ExpertName),
                new FieldContent(nameof(TemplateData.ChiefName), templateData.ChiefName),
                new FieldContent(nameof(TemplateData.RegInfo), templateData.RegInfo),
                new FieldContent(nameof(TemplateData.Query), templateData.Query),
                new FieldContent(nameof(TemplateData.IpcCode), templateData.IpcCode),
                new FieldContent(nameof(TemplateData.RequestDateCreate), templateData.RequestDateCreate),
                new FieldContent(nameof(TemplateData.ReportPageCount), templateData.ReportPageCount.ToString()),
                new TableContent(nameof(TemplateData.SimilaritiesTable),
                    templateData.SimilaritiesTable.Count > 0
                        ? templateData.SimilaritiesTable
                            .Select(s => new TableRowContent(
                                new FieldContent(nameof(Similarity.ProtectionDocCountryCode),
                                    s.ProtectionDocCountryCode),
                                new FieldContent(nameof(Similarity.ProtectionDocNumber), s.ProtectionDocNumber),
                                new FieldContent(nameof(Similarity.PatentTypeCode), s.PatentTypeCode),
                                new FieldContent(nameof(Similarity.BulletinDate), s.BulletinDate),
                                new FieldContent(nameof(Similarity.CategoryCode), s.CategoryCode),
                                new FieldContent(nameof(Similarity.FormulaNumber), s.FormulaNumber)
                            ))
                        : new[]
                        {
                            new TableRowContent(
                                new FieldContent(nameof(Similarity.ProtectionDocCountryCode), string.Empty),
                                new FieldContent(nameof(Similarity.ProtectionDocNumber), string.Empty),
                                new FieldContent(nameof(Similarity.PatentTypeCode), string.Empty),
                                new FieldContent(nameof(Similarity.BulletinDate), string.Empty),
                                new FieldContent(nameof(Similarity.CategoryCode), string.Empty),
                                new FieldContent(nameof(Similarity.FormulaNumber), string.Empty))
                        }
                ),

                new TableContent(nameof(TemplateData.SimilaritiesParagraph),
                    templateData.SimilaritiesParagraph.Count > 0
                        ? templateData.SimilaritiesParagraph
                            .Select(s => new TableRowContent(
                                    new FieldContent(nameof(Similarity.ProtectionDocCountryCode),
                                        s.ProtectionDocCountryCode),
                                    new FieldContent(nameof(Similarity.ProtectionDocNumber), s.ProtectionDocNumber),
                                    new FieldContent(nameof(Similarity.PatentTypeCode), s.PatentTypeCode),
                                    new FieldContent(nameof(Similarity.BulletinDate), s.BulletinDate)
                                )
                            )
                        : new[]
                        {
                            new TableRowContent(
                                new FieldContent(nameof(Similarity.ProtectionDocCountryCode), string.Empty),
                                new FieldContent(nameof(Similarity.ProtectionDocNumber), string.Empty),
                                new FieldContent(nameof(Similarity.PatentTypeCode), string.Empty),
                                new FieldContent(nameof(Similarity.BulletinDate), string.Empty))
                        }
                ));
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId", "UserId", UserInputFieldsParameterName };
        }

        private TemplateData GetTemplateData()
        {
            var similarities = GetSimilarities();
            var document = Executor.GetQuery<GetDocumentByIdQuery>()
                .Process(q => q.Execute((int) Parameters["DocumentId"]));
            var customer = GetCustomer();
            var expert = GetExpert(document);
            var request = Executor.GetQuery<GetRequestByIdQuery>()
                .Process(q => q.Execute((int) Parameters["RequestId"]));
            return new TemplateData
                {
                    RequestNameRu = request.NameRu ?? string.Empty,
                    Declarants = string.Join(", ", request
                        .RequestCustomers
                        .Where(IsRequestCustomerDeclarant())
                        .Select(c => $"{c.Customer.NameRu} ({c.Customer.Country?.Code})")),
                    RequestNumber = request.RequestNum ?? string.Empty,
                    RequestDate = request.RequestDate.ToTemplateDateFormat(),
                    RequestDateCreate = request.DateCreate.ToTemplateDateFormat(),
                    CustomerNameRu = customer != null ? customer.NameRu ?? string.Empty : string.Empty,
                    CustomerCountryCode = customer !=null && customer.Country != null ? customer.Country.Code ?? string.Empty : string.Empty,
                    ExpertName = GetExpertName(expert),
                    RegInfo = GetRegInfo(request),
                    Query = request.ExpertSearchKeywords ?? String.Empty,
                    IpcCode = GetIpcCode(request),
                    ChiefName = GetChiefName(expert),
                    ReportPageCount = (int?) Parameters["PageCount"] ?? 0,
                    SimilaritiesTable = similarities,
                    SimilaritiesParagraph = similarities,
                    CurrentYear = DateTimeOffset.Now.Year.ToString()
                };
        }

        private static Func<RequestCustomer, bool> IsRequestCustomerDeclarant()
        {
            return customer => customer.CustomerRole.Code == DicCustomerRoleCodes.Declarant;
        }

        private static string GetIpcCode(Request r)
        {
            var ipcRequest = r.IPCRequests.FirstOrDefault();
            return ipcRequest != null ? ipcRequest?.Ipc?.Code ?? string.Empty : string.Empty;
        }

        private static string GetRegInfo(Request r)
        {
            return string.Join(", ",
                r.EarlyRegs.Select(er =>
                    $"{er?.RegCountry?.Code ?? string.Empty}, {er?.RegNumber ?? string.Empty}, {er?.RegDate?.ToTemplateDateFormat() ?? string.Empty}"));
        }

        private ApplicationUser GetExpert(Document document)
        {
            var workflow = document.Workflows.SingleOrDefault(rw => rw.CurrentStage.Code.Equals("IN01"));

            return workflow?.CurrentUser;
        }

        private static string GetExpertName(ApplicationUser expert)
        {
            if (expert != null)
            {
                return expert.NameRu ?? string.Empty;
            }

            return string.Empty;
        }

        private ApplicationUser GetChief(ApplicationUser expert)
        {
            if (expert != null)
            {
                var supervisors = Executor.GetQuery<GetUsersByRoleIdQuery>().Process(q => q.Execute(SupervisorId));
                return supervisors.FirstOrDefault(s => s.DepartmentId == expert.DepartmentId);
            }

            return null;
        }

        private string GetChiefName(ApplicationUser expert)
        {
            if (expert != null)
            {
                var chief = GetChief(expert);
                if (chief != null)
                {
                    return chief.NameRu ?? string.Empty;
                }
            }
            return string.Empty;
        }

        private DicCustomer GetCustomer()
        {
            var request = Executor.GetQuery<GetRequestByIdQuery>()
                .Process(q => q.Execute((int) Parameters["RequestId"]));

            var customer = request.RequestCustomers
                .Where(rc => rc.CustomerRoleId == CustomerRoleid)
                .Select(rc => rc.Customer)
                .FirstOrDefault();

            return customer;
        }

        private List<Similarity> GetSimilarities()
        {
            var similarities = Executor.GetQuery<GetSimilaritiesByRequestIdQuery>()
                .Process(q => q.Execute((int) Parameters["RequestId"]));
            return similarities.Select(s => new Similarity
                {
                    ProtectionDocCountryCode = GetRequestCountryCode(s),
                    ProtectionDocNumber = s.SimilarProtectionDoc?.GosNumber ?? string.Empty,
                    PatentTypeCode = s.SimilarProtectionDoc?.Type?.Code ?? string.Empty,
                    BulletinDate = s.SimilarProtectionDoc?.Bulletins?.FirstOrDefault()?.Bulletin?.PublishDate?.ToTemplateDateFormat() ?? string.Empty,
                    FormulaNumber = s.ProtectionDocFormula ?? string.Empty,
                    CategoryCode = s.ProtectionDocCategory ?? string.Empty
                })
                .ToList();
        }

        private static string GetRequestCountryCode(ExpertSearchSimilar s)
        {
            var request = s.Request;

            var earlyReg = request?.EarlyRegs.FirstOrDefault();

            if (earlyReg == null)
            {
                return string.Empty;
            }

            return earlyReg.RegCountry.Code;
        }

        private class TemplateData
        {
            internal string RequestNameRu { get; set; }
            internal string Declarants { get; set; }

            internal string RequestNumber { get; set; }
            internal string RequestDate { get; set; }
            internal string RequestDateCreate { get; set; }
            internal string CustomerNameRu { get; set; }
            internal string CustomerCountryCode { get; set; }
            internal string ExpertName { get; set; }
            internal string ChiefName { get; set; }
            internal string RegInfo { get; set; }
            internal List<Similarity> SimilaritiesTable { get; set; }
            internal List<Similarity> SimilaritiesParagraph { get; set; }
            internal string Query { get; set; }
            internal string IpcCode { get; set; }
            internal int ReportPageCount { get; set; }
            internal string CurrentYear { get; set; }
        }

        private class Similarity
        {
            internal string ProtectionDocCountryCode { get; set; }
            internal string ProtectionDocNumber { get; set; }
            internal string PatentTypeCode { get; set; }
            internal string BulletinDate { get; set; }
            internal string CategoryCode { get; set; }
            internal string FormulaNumber { get; set; }
        }
    }
}
