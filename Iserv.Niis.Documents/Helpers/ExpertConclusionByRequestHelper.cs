using System;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Administration;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Documents;
using Iserv.Niis.Documents.DocumentsBusinessLogic.ExpertSearch;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.Documents.Helpers;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.FileStorage.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using TemplateEngine.Docx;

namespace Iserv.Niis.Documents.Helpers
{
    public class ExpertConclusionByRequestHelper
    {
        private IExecutor _executor;

        private const int CustomerRoleDeclarantId = 1;
        private const int CustomerRolePatentOwnerId = 3;
        private const int UserRoleDeputyDirector = 2;
        private const int PositionHeadDepartmentId = 212;
        private IFileStorage _fileStorage;

        //public ExpertConclusionByRequestHelper(IExecutor executor, IFileStorage fileStorage)
        //{
        //    Executor = executor;
        //    _fileStorage = fileStorage;
        //}

        private Dictionary<string, object> _parameters;

        public Content FillData(IExecutor executor, IFileStorage fileStorage, Dictionary<string, object> parameters, string code)
        {
            _executor = executor;
            _fileStorage = fileStorage;
            _parameters = parameters;
            var templateData = GetTemplateData(code);

            var content = new Content(
                new FieldContent(nameof(TemplateData.Today), templateData.Today),
                new FieldContent(nameof(TemplateData.RequestNumber), templateData.RequestNumber),
                new FieldContent(nameof(TemplateData.RequestDate), templateData.RequestDate),
                new ImageContent(nameof(TemplateData.Image), templateData.Image),
                new FieldContent(nameof(TemplateData.Declarants), templateData.Declarants),
                new FieldContent(nameof(TemplateData.ICGSNumbers), templateData.ICGSNumbers),
                new FieldContent(nameof(TemplateData.ICFEMStrings), templateData.ICFEMStrings),
                new FieldContent(nameof(TemplateData.ExpertPosition), templateData.ExpertPosition),
                new FieldContent(nameof(TemplateData.Title), templateData.Title),
                new FieldContent(nameof(TemplateData.ExpertName), templateData.ExpertName));

            if (templateData.SimilarRequestsTZ.Any())
            {
                content.Tables.Add(BuildSimilarTableContent(nameof(TemplateData.SimilarRequestsTZ),
                    templateData.SimilarRequestsTZ));
            }
            else
            {
                var emptyHiddenTable = new TableContent(nameof(TemplateData.SimilarRequestsTZ));
                content.Tables.Add(emptyHiddenTable);
            }

            if (templateData.SimilarRequestsOTZ.Any())
            {
                content.Tables.Add(BuildSimilarTableContent(nameof(TemplateData.SimilarRequestsOTZ),
                    templateData.SimilarRequestsOTZ));
            }
            else
            {
                var emptyHiddenTable = new TableContent(nameof(TemplateData.SimilarRequestsOTZ));
                content.Tables.Add(emptyHiddenTable);
            }

            if (templateData.SimilarRequestsNMPT.Any())
            {
                content.Tables.Add(BuildSimilarTableContent(nameof(TemplateData.SimilarRequestsNMPT),
                    templateData.SimilarRequestsNMPT));
            }
            else
            {
                var emptyHiddenTable = new TableContent(nameof(TemplateData.SimilarRequestsNMPT));
                content.Tables.Add(emptyHiddenTable);
            }

            if (templateData.SimilarRequestsPO.Any())
            {
                content.Tables.Add(BuildSimilarTableContent(nameof(TemplateData.SimilarRequestsPO),
                    templateData.SimilarRequestsPO));
            }
            else
            {
                var emptyHiddenTable = new TableContent(nameof(TemplateData.SimilarRequestsPO));
                content.Tables.Add(emptyHiddenTable);
            }

            return content;
        }

        private TableContent BuildSimilarTableContent(string tableDataPropertyName, IEnumerable<SimilarRequest> similarRequests)
        {
            var tableContent = new TableContent(tableDataPropertyName, similarRequests.Select(r => new TableRowContent(
                new ImageContent(nameof(SimilarRequest.ItemImage), r.ItemImage),
                new FieldContent(nameof(SimilarRequest.ItemProtectionDocValidDate), r.ItemProtectionDocValidDate),
                new FieldContent(nameof(SimilarRequest.ItemProtectionDocNumber), r.ItemProtectionDocNumber),
                new FieldContent(nameof(SimilarRequest.ItemDeclarantOrPatentOwner), r.ItemDeclarantOrPatentOwner),
                new FieldContent(nameof(SimilarRequest.ItemICGSNumbers), r.ItemICGSNumbers),
                new FieldContent(nameof(SimilarRequest.ItemDisclamation), r.ItemDisclamation),
                new FieldContent(nameof(SimilarRequest.ItemRequestNumber), r.ItemRequestNumber),
                new FieldContent(nameof(SimilarRequest.ItemRequestDate), r.ItemRequestDate),
                new FieldContent(nameof(SimilarRequest.ItemGosreestr), r.ItemGosreestr),
                new FieldContent(nameof(SimilarRequest.ItemComment), r.ItemComment))));

            return tableContent;
        }

        private TemplateData GetTemplateData(string code)
        {
            var request = _executor.GetQuery<GetRequestByIdQuery>()
                .Process(q => q.Execute((int)_parameters["RequestId"]));

            var title = GetTitle(code);

            var lastRequest = _executor.GetQuery<GetLastRequestByNumberQuery>()
                .Process(q => q.Execute());

            var document = _executor.GetQuery<GetDocumentByIdQuery>()
                .Process(q => q.Execute(Convert.ToInt32(_parameters["DocumentId"])));

            var today = document?.DateCreate.ToString("dd.MM.yyyy") ?? string.Empty;

            // Получаем id юзера-эксперта, который создал документ
            var expertUserId = document.Workflows.First(dw => dw.PreviousWorkflowId == null).CurrentUserId.Value;

            var expertUser = _executor.GetQuery<GetUserByIdQuery>()
                .Process(q => q.Execute(expertUserId));

            var expertName = expertUser.NameRu ?? expertUser.NameKz ?? expertUser.NameEn ?? string.Empty;

            var expertPosition = expertUser.Position;

            var expertPositionName = expertPosition.NameRu ?? expertPosition.NameKz ?? expertPosition.NameEn ?? string.Empty;

            var deputyDirector = GetFirstDeputyDirector() ?? string.Empty;
            var headOfDepartment = GetChiefOfAuthor((int)_parameters["UserId"]) ?? string.Empty;

            var similarities = _executor.GetQuery<GetSimilaritiesByRequestIdQuery>()
                .Process(q => q.Execute((int)_parameters["RequestId"]));

            var tzTypeCodes = new[] { DicProtectionDocTypeCodes.RequestTypeTrademarkCode, DicProtectionDocTypeCodes.ProtectionDocTypeTrademarkCode };
            var otzCodes = new[] { DicProtectionDocTypeCodes.ProtectionDocTypeTrademarkGenerallyKnownCode };
            var nmptTypeCodes = new[] { DicProtectionDocTypeCodes.RequestTypeNameOfOriginCode, DicProtectionDocTypeCodes.ProtectionDocTypeNameOfOriginCode };
            var poTypeCodes = new[] { DicProtectionDocTypeCodes.RequestTypeIndustrialSampleCode, DicProtectionDocTypeCodes.ProtectionDocTypeIndustrialSampleCode };

            var similarRequestsTZ = new List<SimilarRequest>();
            var similarRequestsOTZ = new List<SimilarRequest>();
            var similarRequestsNMPT = new List<SimilarRequest>();
            var similarRequestsPO = new List<SimilarRequest>();

            foreach (var similarity in similarities)
            {
                string typeCode = null;
                string subtypeCode = null;

                switch (similarity.OwnerType)
                {
                    case Owner.Type.Request:
                        typeCode = similarity.SimilarRequest.ProtectionDocType?.Code;
                        subtypeCode = similarity.SimilarRequest.SpeciesTradeMark?.Code;
                        break;
                    case Owner.Type.ProtectionDoc:
                        typeCode = similarity.SimilarProtectionDoc.Type?.Code;
                        subtypeCode = similarity.SimilarProtectionDoc.SpeciesTradeMark?.Code;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (typeCode != null)
                {
                    if (tzTypeCodes.Contains(typeCode))
                    {
                        if (subtypeCode != null && subtypeCode == DicProtectionDocSubtypeCodes.WellKnownTradeMark)
                        {
                            similarRequestsOTZ.Add(GetSimilarRequest(similarity));
                        }
                        else
                        {
                            similarRequestsTZ.Add(GetSimilarRequest(similarity));
                        }
                    }
                    else if (otzCodes.Contains(typeCode))
                    {
                        similarRequestsOTZ.Add(GetSimilarRequest(similarity));
                    }
                    else if (nmptTypeCodes.Contains(typeCode))
                    {
                        similarRequestsNMPT.Add(GetSimilarRequest(similarity));
                    }
                    else if (poTypeCodes.Contains(typeCode))
                    {
                        similarRequestsPO.Add(GetSimilarRequest(similarity));
                    }
                }
            }

            return new TemplateData
            {
                Today = today,
                DeputyDirector = deputyDirector,
                HeadOfDepartment = headOfDepartment,
                RequestNumber = request.RequestNum ?? string.Empty,
                RequestDate = request.RequestDate.ToTemplateDateFormat(),
                Image = request.Image ?? new byte[0],
                Declarants = GetDeclarants((int)_parameters["RequestId"]),
                ICGSNumbers = string.Join(Environment.NewLine, request.ICGSRequests.Select(i => $"{i.Icgs.NameRu} - {i.Icgs.DescriptionShort}")),
                ICFEMStrings = string.Join($";{Environment.NewLine}", request.Icfems.Select(ri => $"{ri.DicIcfem.Code}: {ri.DicIcfem.NameRu}")),
                LastRequestDate = lastRequest.RequestDate.ToTemplateDateFormat(),
                LastRequestNumber = lastRequest.RequestNum ?? string.Empty,
                Disclamation = request.DisclaimerRu ?? string.Empty,
                ExpertPosition = expertPositionName,
                ExpertName = expertName,
                SimilarRequestsTZ = similarRequestsTZ,
                SimilarRequestsOTZ = similarRequestsOTZ,
                SimilarRequestsNMPT = similarRequestsNMPT,
                SimilarRequestsPO = similarRequestsPO,
                Title = title
            };
        }

        private SimilarRequest GetSimilarRequest(ExpertSearchSimilar x)
        {
            switch (x.OwnerType)
            {
                case Owner.Type.Request:

                    var itemDeclarant = string.Join(",",
                        x.SimilarRequest.RequestCustomers
                            .Where(c => c.CustomerRoleId == CustomerRoleDeclarantId)
                            .Select(c => $"{c.Customer?.NameRu} ({c.Address ?? c.Customer?.Address})")
                    );

                    var itemICGSNumbers = string.Join(", ", x.SimilarRequest.ICGSRequests.Select(i => i.Icgs.Code.Substring(0, 2)));

                    var itemDisclamation = x.SimilarRequest.DisclaimerRu ?? string.Empty;

                    var itemPatentOwner = string.Join(", ", x.SimilarRequest.RequestCustomers
                        .Where(c => c.CustomerRoleId == CustomerRolePatentOwnerId)
                        .Select(c => c.Customer.NameRu));

                    var itemRequestNumber = x.SimilarRequest.RequestNum ?? string.Empty;

                    var itemRequestDate = x.SimilarRequest.RequestDate?.ToString("dd.MM.yyyy") ?? string.Empty;

                    return new SimilarRequest
                    {
                        ItemDeclarantOrPatentOwner = itemDeclarant,
                        ItemICGSNumbers = itemICGSNumbers,
                        ItemImage = x.SimilarRequest.Image ?? new byte[0],
                        ItemDisclamation = itemDisclamation,
                        ItemPatentOwner = itemPatentOwner,
                        ItemRequestNumber = itemRequestNumber,
                        ItemRequestDate = itemRequestDate,
                        ItemGosreestr = string.Empty,
                        ItemProtectionDocNumber = string.Empty,
                        ItemProtectionDocValidDate = string.Empty,
                        ItemComment = x.ProtectionDocCategory ?? x.ProtectionDocFormula ?? string.Empty
                    };
                case Owner.Type.ProtectionDoc:
                    return new SimilarRequest
                    {
                        ItemDeclarantOrPatentOwner = GetProtectionDocCustomersWithAddress(x.SimilarProtectionDoc.ProtectionDocCustomers.ToList())
                        //string.Join(", ", x.SimilarProtectionDoc.ProtectionDocCustomers
                        //.Where(c => c.CustomerRoleId == CustomerRoleDeclarantId)
                        //.Select(c => c.Customer.NameRu))
                        ,
                        ItemProtectionDocNumber = x.SimilarProtectionDoc.RegNumber ?? string.Empty,
                        ItemICGSNumbers = string.Join(", ", x.SimilarProtectionDoc.IcgsProtectionDocs.Select(i => i.Icgs.Code)),
                        ItemProtectionDocValidDate = x.SimilarProtectionDoc.ValidDate?.ToString("dd.MM.yyyy") ?? string.Empty,
                        ItemImage = x.SimilarProtectionDoc.Image,
                        ItemDisclamation = x.SimilarProtectionDoc.DisclaimerRu ?? string.Empty,
                        ItemGosreestr = x.SimilarProtectionDoc.Gosreestr ?? string.Empty,
                        //ItemPatentOwner = string.Join(", ", x.SimilarProtectionDoc.ProtectionDocCustomers
                        //    .Where(c => c.CustomerRoleId == CustomerRolePatentOwnerId)
                        //    .Select(c => c.Customer.NameRu)),
                        ItemRequestNumber = x.SimilarProtectionDoc.Request != null
                            ? x.SimilarProtectionDoc.Request.RequestNum ?? string.Empty
                            : string.Empty,
                        ItemRequestDate = x.SimilarProtectionDoc.Request != null
                            ? x.SimilarProtectionDoc.Request.RequestDate?.ToString("dd.MM.yyyy") ?? string.Empty
                            : string.Empty,
                        ItemComment = x.ProtectionDocCategory ?? x.ProtectionDocFormula ?? string.Empty
                    };
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private string GetProtectionDocCustomersWithAddress(List<ProtectionDocCustomer> customers)
        {
            string resultDeclarants = string.Join(",",
                customers
                    .Where(c => c.CustomerRoleId == CustomerRoleDeclarantId)
                    .Select(c => $"{c.Customer?.NameRu} ({c.Address ?? c.Customer?.Address})")
                );

            string resultPatentOwner = string.Join(",",
                customers
                    .Where(c => c.CustomerRoleId == CustomerRolePatentOwnerId)
                    .Select(c => $"{c.Customer?.NameRu} ({c.Address ?? c.Customer?.Address})")
            );

            string result = string.IsNullOrEmpty(resultDeclarants) ? resultPatentOwner : resultDeclarants;


            return result;
        }

        private string GetFirstDeputyDirector()
        {
            var firstDeputyDirector = _executor.GetQuery<GetUsersByRoleIdQuery>()
                .Process(q => q.Execute(UserRoleDeputyDirector))
                .FirstOrDefault();

            return firstDeputyDirector?.NameRu ?? string.Empty;
        }

        private string GetChiefOfAuthor(int userId)
        {
            var headsOfDepartment = _executor.GetQuery<GetUsersByPositionIdQuery>()
                .Process(q => q.Execute(PositionHeadDepartmentId));

            var chiefOfAuthor = headsOfDepartment.FirstOrDefault(h => h.Department.Users.Any(u => u.Id == userId));

            return chiefOfAuthor?.NameRu ?? string.Empty;
        }

        private byte[] GetImage(int barcode)
        {
            try
            {
                return _fileStorage.GetAsync("images", $"{barcode}.jpg").Result;
            }
            catch
            {
                return new byte[0];
            }
        }

        private string GetDeclarants(int requestId)
        {
            var request = _executor.GetQuery<GetRequestByIdQuery>()
                .Process(q => q.Execute(requestId));

            var customerNames = request.RequestCustomers
                .Where(c => c.CustomerRoleId == CustomerRoleDeclarantId)
                .Select(x => x.Customer.NameRu ?? string.Empty)
                .ToArray();

            return string.Join(", ", customerNames);
        }

        private string GetTitle(string code)
        {
            string result = string.Empty;

            switch (code)
            {
                
                case "TZPOL555PR":
                    result = "о частичной регистрации";
                    break;
                case "TZPOL555PRWD":
                    result = "о частичной регистрации/с дискламацией";
                    break;
                case "TZPOL555PF":
                    result = "о  предварительном отказе";
                    break;
                default:
                    result = "о регистрации";
                    break;
            }

            return result;
        }

        private class TemplateData
        {
            public string Today { get; set; }
            public string DeputyDirector { get; set; }
            public string HeadOfDepartment { get; set; }
            public string RequestNumber { get; set; }
            public string RequestDate { get; set; }
            public byte[] Image { get; set; }
            public string Title { get; set; }
            public string Declarants { get; set; }
            public string ICGSNumbers { get; set; }
            public string ICFEMStrings { get; set; }
            public string LastRequestDate { get; set; }
            public string LastRequestNumber { get; set; }
            public string Disclamation { get; set; }
            public string ExpertPosition { get; set; }
            public string ExpertName { get; set; }
            public List<SimilarRequest> SimilarRequestsTZ { get; set; }
            public List<SimilarRequest> SimilarRequestsOTZ { get; set; }
            public List<SimilarRequest> SimilarRequestsNMPT { get; set; }
            public List<SimilarRequest> SimilarRequestsPO { get; set; }
        }

        private class SimilarRequest
        {
            public string ItemRequestNumber { set; get; }
            public string ItemRequestDate { get; set; }
            public byte[] ItemImage { get; set; }
            public string ItemProtectionDocNumber { get; set; }
            public string ItemProtectionDocValidDate { get; set; }
            public string ItemPatentOwner { get; set; }
            public string ItemDeclarantOrPatentOwner { get; set; }
            public string ItemICGSNumbers { get; set; }
            public string ItemDisclamation { get; set; }
            public string ItemGosreestr { get; set; }
            public string ItemComment { get; set; }
        }
    }
}