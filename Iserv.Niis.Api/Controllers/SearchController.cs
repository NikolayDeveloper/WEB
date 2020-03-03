using AutoMapper.QueryableExtensions;
using Iserv.Niis.BusinessLogic.Contracts;
using Iserv.Niis.BusinessLogic.Dictionaries.DicProtectionDocTypes;
using Iserv.Niis.BusinessLogic.Documents;
using Iserv.Niis.BusinessLogic.ProtectionDocs;
using Iserv.Niis.BusinessLogic.Requests;
using Iserv.Niis.BusinessLogic.Search;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Infrastructure.Extensions.Filter;
using Iserv.Niis.Infrastructure.Pagination;
using Iserv.Niis.Model.Models.ExpertSearch;
using Iserv.Niis.Model.Models.Search;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.BusinessLogic.Excel;
using Iserv.Niis.Services.Interfaces;

namespace Iserv.Niis.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Search")]
    public class SearchController : BaseNiisApiController
    {
        private readonly ISearchService _searchService;
        
        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        #region Simple search

        // GET: api/Search
        [HttpGet]
        public IActionResult Get()
        {
            var result = Executor.GetQuery<GetSearchViewEntitiesQuery>().Process(q => q.Execute())
                .ProjectTo<SearchDto>();

            return GetListResult(result);
        }

        // GET: api/Search/GetExcel
        [HttpGet("GetExcel")]
        public IActionResult GetExcel()
        {
            var result = Executor.GetQuery<GetSearchViewEntitiesQuery>().Process(q => q.Execute())
                .ProjectTo<SearchDto>();
            return GetExcelInternal(result);
        }

        [HttpGet("intellectualProperty")]
        public IActionResult GetIntellectualProperty()
        {
            var requests = Executor.GetQuery<GetRequestsQuery>().Process(q => q.Execute());
            var contracts = Executor.GetQuery<GetContractsQuery>().Process(q => q.Execute());
            var protectionDocs = Executor.GetQuery<GetProtectionDocsQuery>().Process(q => q.Execute());
            var requestDtos = Mapper.Map<IntellectualPropertySearchDto[]>(requests);
            var contractDtos = Mapper.Map<IntellectualPropertySearchDto[]>(contracts);
            var protectionDocDtos = Mapper.Map<IntellectualPropertySearchDto[]>(protectionDocs);
            var unionQuery = requestDtos.AsQueryable()
                .Union(contractDtos.AsQueryable()
                .Union(protectionDocDtos.AsQueryable()));
            var filteredQuery = unionQuery.Filter(Request.Query);
            var sortedQuery = filteredQuery.Sort(Request.Query);

            return Ok(sortedQuery);
        }

        [HttpPost("getOwners")]
        public IActionResult GetSelectedOwners([FromBody]IntellectualPropertySearchDto[] dtos)
        {
            var isAllSelected = Convert.ToBoolean(Request.Query["isAllSelected"].ToString());
            SelectionMode selectionMode;
            switch (Request.Query["selectionMode"].ToString())
            {
                case "0":
                    selectionMode = SelectionMode.Including;
                    break;
                case "1":
                    selectionMode = SelectionMode.Except;
                    break;
                default:
                    throw new NotImplementedException();
            }
            if (isAllSelected)
            {
                var requests = Executor.GetQuery<GetRequestsQuery>().Process(q => q.Execute())
                    .ProjectTo<IntellectualPropertySearchDto>();
                var contracts = Executor.GetQuery<GetContractsQuery>().Process(q => q.Execute())
                    .ProjectTo<IntellectualPropertySearchDto>();
                var protectionDocs = Executor.GetQuery<GetProtectionDocsQuery>().Process(q => q.Execute())
                    .ProjectTo<IntellectualPropertySearchDto>();
                dtos = requests.Concat(contracts.Concat(protectionDocs)).ToArray();
            }
            else
            {
                if (selectionMode == SelectionMode.Except)
                {
                    var dtosLocal = dtos;
                    var requests = Executor.GetQuery<GetRequestsQuery>().Process(q => q.Execute())
                        .ProjectTo<IntellectualPropertySearchDto>()
                        .Where(r => !dtosLocal.Any(d => r.Id == d.Id && r.Type == d.Type));
                    var contracts = Executor.GetQuery<GetContractsQuery>().Process(q => q.Execute())
                        .ProjectTo<IntellectualPropertySearchDto>()
                        .Where(r => !dtosLocal.Any(d => r.Id == d.Id && r.Type == d.Type));
                    var protectionDocs = Executor.GetQuery<GetProtectionDocsQuery>().Process(q => q.Execute())
                        .ProjectTo<IntellectualPropertySearchDto>()
                        .Where(r => !dtosLocal.Any(d => r.Id == d.Id && r.Type == d.Type));
                    dtos = requests.Concat(contracts.Concat(protectionDocs)).ToArray();
                }
            }

            return Ok(dtos);
        }

        #endregion

        #region Advanced search - request

        // GET: api/Search/request
        [HttpGet("request")]
        public IActionResult GetRequest()
        {
            var requests = Executor.GetQuery<GetRequestsQuery>().Process(q => q.Execute());
            var result = Mapper.Map<IQueryable<RequestSearchDto>>(requests);
            return GetListResult(result);
        }

        // GET: api/Search/request/GetExcel
        [HttpGet("request/GetExcel")]
        public IActionResult GetRequestExcel()
        {
            var requests = Executor.GetQuery<GetRequestsQuery>().Process(q => q.Execute());
            var result = Mapper.Map<IQueryable<RequestSearchDto>>(requests);
            return GetExcelInternal(result);
        }

        #endregion

        #region Advanced search - protectionDoc

        // GET: api/Search/protectionDoc
        [HttpGet("protectionDoc")]
        public IActionResult GetProtectionDoc()
        {
            var protectionDocs = Executor.GetQuery<GetProtectionDocsQuery>().Process(q => q.Execute());
            var result = Mapper.Map<IQueryable<ProtectionDocSearchDto>>(protectionDocs);
            return GetListResult(result);
        }

        // GET: api/Search/protectionDoc/GetExcel
        [HttpGet("protectionDoc/GetExcel")]
        public IActionResult GetProtectionDocExcel()
        {
            var protectionDocs = Executor.GetQuery<GetProtectionDocsQuery>().Process(q => q.Execute());
            var result = Mapper.Map<IQueryable<ProtectionDocSearchDto>>(protectionDocs);
            return GetExcelInternal(result);
        }

        #endregion

        #region Advanced search - contract

        // GET: api/Search/contract
        [HttpGet("contract")]
        public IActionResult GetContract()
        {
            var contracts = Executor.GetQuery<GetContractsQuery>().Process(q => q.Execute());
            var result = Mapper.Map<IQueryable<ContractSearchDto>>(contracts);
            return GetListResult(result);
        }

        // GET: api/Search/contract/GetExcel
        [HttpGet("contract/GetExcel")]
        public IActionResult GetContractExcel()
        {
            var contracts = Executor.GetQuery<GetContractsQuery>().Process(q => q.Execute());
            var result = Mapper.Map<IQueryable<ContractSearchDto>>(contracts);
            return GetExcelInternal(result);
        }

        #endregion

        #region Advanced search - document

        // GET: api/Search/document
        [HttpGet("document")]
        public IActionResult GetDocument()
        {
            var documents = Executor.GetQuery<GetDocumentsQuery>().Process(q => q.Execute());
            var result = Mapper.Map<IQueryable<DocumentSearchDto>>(documents);
            return GetListResult(result);
        }

        // GET: api/Search/document/GetExcel
        [HttpGet("document/GetExcel")]
        public IActionResult GetDocumentExcel()
        {
            var documents = Executor.GetQuery<GetDocumentsQuery>().Process(q => q.Execute());
            var result = Mapper.Map<IQueryable<DocumentSearchDto>>(documents);
            return GetExcelInternal(result);
        }

        #endregion


        #region Expert search - trademark
        /// <summary>
        /// Осуществляет экспертный поиск по товарным знакам.
        /// </summary>
        /// <returns></returns>
        // GET: api/Search/expert/trademark
        [HttpGet("expert/trademark")]
        public async Task<IActionResult> GetTrademark()
        {
            var trademarkTypeCodes = new[] {
                DicProtectionDocTypeCodes.RequestTypeTrademarkCode,
                DicProtectionDocTypeCodes.ProtectionDocTypeTrademarkCode
            };

            var nmptTypeCodes = new[]
            {
                DicProtectionDocTypeCodes.RequestTypeNameOfOriginCode,
                DicProtectionDocTypeCodes.ProtectionDocTypeNameOfOriginCode
            };

            var otzTypeCodes = new[]
            {
                DicProtectionDocTypeCodes.ProtectionDocTypeTrademarkGenerallyKnownCode
            };

            var industrialSampleTypeCodes = new[]
            {
                DicProtectionDocTypeCodes.RequestTypeIndustrialSampleCode,
                DicProtectionDocTypeCodes.ProtectionDocTypeIndustrialSampleCode
            };

            //Если на форме был выбран combobox: "НМПТ" ("PN").
            if (Request.Query.ContainsKey("and_isNmpt_eq"))
            {
                trademarkTypeCodes = trademarkTypeCodes.Concat(nmptTypeCodes).ToArray();
            }
            
            //Если на форме был выбран combobox: "ОТЗ" ("Well-known trade mark").
            if (Request.Query.ContainsKey("and_isIndustrialSample_eq"))
            {
                trademarkTypeCodes = trademarkTypeCodes.Concat(industrialSampleTypeCodes).ToArray();
            }

            //Если на форме был выбран combobox: "ПО" ("Industrial Sample").
            if (Request.Query.ContainsKey("and_isWellKnown_eq"))
            {
                trademarkTypeCodes = trademarkTypeCodes.Concat(otzTypeCodes).ToArray();
            }

            //Получаем id типов охранных документов по кодам охранных документов.
            var trademarkTypeIds = Executor.GetQuery<GetProtectionDocTypeByCodesQuery>()
                .Process(q => q.Execute(trademarkTypeCodes))
                .Select(r => r.Id);

            var trademarkViewEntities = Executor.GetQuery<GetExpertSearchViewByHttpRequestQuery>()
                .Process(q => q.Execute(Request))
                .Where(t => trademarkTypeIds.Contains(t.ProtectionDocTypeId));

            var result = trademarkViewEntities.ProjectTo<TrademarkSearchDto>();
            var paged = result.ToPagedList(Request.GetPaginationParams());

            foreach (var trademarkSearchDto in paged.Items)
            {
                Request request = null;
                ProtectionDoc protectionDoc = null;
                Request protectionDocRequest = null;
                switch (trademarkSearchDto.OwnerType)
                {
                    case Owner.Type.Request:
                        request = await Executor.GetQuery<GetRequestByIdQuery>()
                            .Process(q => q.ExecuteAsync(trademarkSearchDto.Id));
                        break;
                    case Owner.Type.ProtectionDoc:
                        protectionDoc = await Executor.GetQuery<GetProtectionDocByIdQuery>()
                            .Process(q => q.ExecuteAsync(trademarkSearchDto.Id));
                        protectionDocRequest = await Executor.GetQuery<GetRequestByIdQuery>()
                            .Process(q => q.ExecuteAsync(protectionDoc?.RequestId ?? 0));
                        break;
                }
                trademarkSearchDto.RegNumber = GetRegNumber(request, protectionDoc);
                trademarkSearchDto.RegDate = GetRegDate(request, protectionDoc);
                trademarkSearchDto.GosNumber = GetGosNumber(protectionDoc);
                trademarkSearchDto.GosDate = GetGosDate(protectionDoc);
                trademarkSearchDto.Colors = GetIcfem(request, protectionDocRequest, true);
                trademarkSearchDto.Icfem = GetIcfem(request, protectionDocRequest, false);
                trademarkSearchDto.Icgs = GetIcgs(request, protectionDocRequest);
                trademarkSearchDto.Disclamation = GetDisclamation(request, protectionDocRequest);
                trademarkSearchDto.DeclarantName = GetCustomerNameByRoleCode(request, protectionDocRequest, DicCustomerRoleCodes.Declarant);
                trademarkSearchDto.ValidDate = GetValidDate(protectionDoc);
                trademarkSearchDto.PreviewImage = GetPreviewImage(request, protectionDocRequest);
            }

            return paged.AsOkObjectResult(Response);
        }

        // GET: api/Search/expert/trademark/GetExcel
        [HttpGet("expert/trademark/GetExcel")]
        public async Task<IActionResult> GetTrademarkExcel()
        {
            var trademarkTypeCodes = new[] {
                DicProtectionDocTypeCodes.RequestTypeTrademarkCode,
                DicProtectionDocTypeCodes.ProtectionDocTypeTrademarkCode
            };
            var nmptTypeCodes = new[]
            {
                DicProtectionDocTypeCodes.RequestTypeNameOfOriginCode,
                DicProtectionDocTypeCodes.ProtectionDocTypeNameOfOriginCode
            };
            var otzTypeCodes = new[]
            {
                DicProtectionDocTypeCodes.ProtectionDocTypeTrademarkGenerallyKnownCode
            };
            var industrialSampleTypeCodes = new[]
            {
                DicProtectionDocTypeCodes.RequestTypeIndustrialSampleCode,
                DicProtectionDocTypeCodes.ProtectionDocTypeIndustrialSampleCode
            };
            if (Request.Query.ContainsKey("and_isNmpt_eq"))
            {
                trademarkTypeCodes = trademarkTypeCodes.Concat(nmptTypeCodes).ToArray();
            }
            if (Request.Query.ContainsKey("and_isWellKnown_eq"))
            {
                trademarkTypeCodes = trademarkTypeCodes.Concat(otzTypeCodes).ToArray();
            }
            if (Request.Query.ContainsKey("and_isIndustrialSample_eq"))
            {
                trademarkTypeCodes = trademarkTypeCodes.Concat(industrialSampleTypeCodes).ToArray();
            }

            var trademarkTypeIds = Executor.GetQuery<GetProtectionDocTypeByCodesQuery>()
                .Process(q => q.Execute(trademarkTypeCodes))
                .Select(r => r.Id);

            var trademarkViewEntities = Executor.GetQuery<GetExpertSearchViewByHttpRequestQuery>()
                .Process(q => q.Execute(Request))
                .Where(t => trademarkTypeIds.Contains(t.ProtectionDocTypeId));

            var result = trademarkViewEntities.ProjectTo<TrademarkSearchDto>();
            var paged = result.ToPagedList(Request.GetPaginationParams());

            foreach (var trademarkSearchDto in paged.Items)
            {
                Request request = null;
                ProtectionDoc protectionDoc = null;
                Request protectionDocRequest = null;
                switch (trademarkSearchDto.OwnerType)
                {
                    case Owner.Type.Request:
                        request = await Executor.GetQuery<GetRequestByIdQuery>()
                            .Process(q => q.ExecuteAsync(trademarkSearchDto.Id));
                        break;
                    case Owner.Type.ProtectionDoc:
                        protectionDoc = await Executor.GetQuery<GetProtectionDocByIdQuery>()
                            .Process(q => q.ExecuteAsync(trademarkSearchDto.Id));
                        protectionDocRequest = await Executor.GetQuery<GetRequestByIdQuery>()
                            .Process(q => q.ExecuteAsync(protectionDoc?.RequestId ?? 0));
                        break;
                }
                trademarkSearchDto.RegNumber = GetRegNumber(request, protectionDoc);
                trademarkSearchDto.RegDate = GetRegDate(request, protectionDoc);
                trademarkSearchDto.GosNumber = GetGosNumber(protectionDoc);
                trademarkSearchDto.GosDate = GetGosDate(protectionDoc);
                trademarkSearchDto.Colors = GetIcfem(request, protectionDocRequest, true);
                trademarkSearchDto.Icfem = GetIcfem(request, protectionDocRequest, false);
                trademarkSearchDto.Icgs = GetIcgs(request, protectionDocRequest);
                trademarkSearchDto.Disclamation = GetDisclamation(request, protectionDocRequest);
                trademarkSearchDto.DeclarantName = GetCustomerNameByRoleCode(request, protectionDocRequest, DicCustomerRoleCodes.Declarant);
                trademarkSearchDto.OwnerName = GetCustomerNameByRoleCode(request, protectionDocRequest, DicCustomerRoleCodes.Owner);
                trademarkSearchDto.ValidDate = GetValidDate(protectionDoc);
                trademarkSearchDto.PreviewImage = GetPreviewImage(request, protectionDocRequest);
            }

            return GetExelByItems(paged.Items);
        }

        #endregion

        #region Expert search - invention

        // GET: api/Search/expert/invention
        [HttpGet("expert/invention")]
        public async Task<IActionResult> GetInvention()
        {
            var searchResults = await _searchService.SearchInventions(Request);

            return searchResults.AsOkObjectResult(Response);

        }

        // GET: api/Search/expert/invention/GetExcel
        [HttpGet("expert/invention/GetExcel")]
        public IActionResult GetInventionExcel()
        {
            var inventionTypeIds = Executor.GetQuery<GetProtectionDocTypeByCodesQuery>()
                .Process(q => q.Execute(new[] { DicProtectionDocTypeCodes.RequestTypeInventionCode }))
                .Select(r => r.Id);

            var inventionViewEntities = Executor.GetQuery<GetExpertSearchViewByHttpRequestQuery>()
                .Process(q => q.Execute(Request))
                .Where(t => inventionTypeIds.Contains(t.ProtectionDocTypeId));

            var result = inventionViewEntities.ProjectTo<InventionDto>();

            var paged = result.ToPagedList(Request.GetPaginationParams());

            return GetExelByItems(paged.Items);
        }

        #endregion

        #region Expert search - usefulModel

        // GET: api/Search/expert/usefulModel
        [HttpGet("expert/usefulModel")]
        public IActionResult GetUsefulModel()
        {
            var usefulModelTypeCodes = new[] { DicProtectionDocTypeCodes.RequestTypeUsefulModelCode };
            var usefulModelTypeIds = Executor.GetQuery<GetProtectionDocTypeByCodesQuery>()
                .Process(q => q.Execute(usefulModelTypeCodes))
                .Select(r => r.Id);

            var inventionViewEntities = Executor.GetQuery<GetExpertSearchViewByHttpRequestQuery>()
                .Process(q => q.Execute(Request))
                .Where(t => usefulModelTypeIds.Contains(t.ProtectionDocTypeId));

            var result = inventionViewEntities.ProjectTo<UsefulModelDto>();

            var paged = result.ToPagedList(Request.GetPaginationParams());

            return paged.AsOkObjectResult(Response);
        }

        // GET: api/Search/expert/usefulModel/GetExcel
        [HttpGet("expert/usefulModel/GetExcel")]
        public IActionResult GetUsefulModelExcel()
        {
            var usefulModelTypeIds = Executor.GetQuery<GetProtectionDocTypeByCodesQuery>()
                .Process(q => q.Execute(new[] { DicProtectionDocTypeCodes.RequestTypeUsefulModelCode }))
                .Select(r => r.Id);

            var inventionViewEntities = Executor.GetQuery<GetExpertSearchViewByHttpRequestQuery>()
                .Process(q => q.Execute(Request))
                .Where(t => usefulModelTypeIds.Contains(t.ProtectionDocTypeId));

            var result = inventionViewEntities.ProjectTo<UsefulModelDto>();

            var paged = result.ToPagedList(Request.GetPaginationParams());

            return GetExelByItems(paged.Items);
        }

        #endregion

        #region Expert search - industrialDesign

        // GET: api/Search/expert/industrialdesign
        [HttpGet("expert/industrialdesign")]
        public IActionResult GetIndustrialDesign()
        {
            var industrialDesignTypeIds = Executor.GetQuery<GetProtectionDocTypeByCodesQuery>()
                .Process(q => q.Execute(new[] { DicProtectionDocTypeCodes.RequestTypeIndustrialSampleCode, DicProtectionDocTypeCodes.ProtectionDocTypePreliminaryIndustrialSampleCode }))
                .Select(r => r.Id);

            var industrialDesignViewEntities = Executor.GetQuery<GetExpertSearchViewByHttpRequestQuery>()
                .Process(q => q.Execute(Request))
                .Where(t => industrialDesignTypeIds.Contains(t.ProtectionDocTypeId));

            var result = industrialDesignViewEntities.ProjectTo<IndustrialDesignDto>();

            var paged = result.ToPagedList(Request.GetPaginationParams());

            return paged.AsOkObjectResult(Response);
        }

        // GET: api/Search/expert/industrialdesign/GetExcel
        [HttpGet("expert/industrialdesign/GetExcel")]
        public IActionResult GetIndustrialDesignExcel()
        {
            var industrialDesignTypeIds = Executor.GetQuery<GetProtectionDocTypeByCodesQuery>()
                .Process(q => q.Execute(new[] { DicProtectionDocTypeCodes.RequestTypeIndustrialSampleCode, DicProtectionDocTypeCodes.ProtectionDocTypePreliminaryIndustrialSampleCode }))
                .Select(r => r.Id);

            var industrialDesignViewEntities = Executor.GetQuery<GetExpertSearchViewByHttpRequestQuery>()
                .Process(q => q.Execute(Request))
                .Where(t => industrialDesignTypeIds.Contains(t.ProtectionDocTypeId));

            var result = industrialDesignViewEntities.ProjectTo<IndustrialDesignDto>();

            var paged = result.ToPagedList(Request.GetPaginationParams());

            return GetExelByItems(paged.Items);
        }

        #endregion

        #region Expert search - similarResults

        // POST: api/Search/expert/{id}/putSimilarResults
        [HttpPost("expert/{id}/similarResults")]
        public async Task<IActionResult> CreateSimilarSearchResults(int id,
            [FromBody] ExpertSearchSimilarDto[] expertSearchSimilarDtos)
        {
            Executor.CommandChain()
                //.AddCommand<ClearExpertSearchSimilarsCommand>(c => c.Execute(id))
                .AddCommand<CreateExpertSearchSimilarsCommand>(c => c.Execute(expertSearchSimilarDtos))
                .ExecuteAllWithTransaction();
            var expertSearchSimilars = await Executor.GetQuery<GetExpertSearchSimilarsByRequestIdQuery>()
                .Process(q => q.Execute(id));

            var expertSearchDtos = await GetMappedExpertSearchDtos(expertSearchSimilars);

            return Ok(expertSearchDtos);
        }

        [HttpPut("expert/{ownerType}/{ownerId}/similarResults/{keywords}")]
        public async Task<IActionResult> UpdateSimilarSearchResults(int ownerId, Owner.Type ownerType, string keywords,
            [FromBody] ExpertSearchSimilarDto[] similarities)
        {
            var mappedSimilars = GetMappedSimilars(similarities);

            Executor.GetCommand<UpdateExpertSearchSimilarsCommand>()
                .Process(c => c.Execute(mappedSimilars));

            switch (ownerType)
            {
                case Owner.Type.Request:
                    var request = await Executor.GetQuery<GetRequestByIdQuery>()
                        .Process(q => q.ExecuteAsync(ownerId));
                    request.ExpertSearchKeywords = keywords;
                    await Executor.GetCommand<UpdateRequestCommand>()
                        .Process(q => q.ExecuteAsync(request));
                    break;
            }

            return NoContent();
        }

        // POST: api/Search/expert/{id}/deleteSimilarResults
        [HttpDelete("expert/{ownerId}/similarResults/{similarityIds}")]
        public async Task<IActionResult> DeleteSimilarSearchResults(int ownerId, string similarityIds)
        {
            var similarIds = similarityIds.Split(';').Select(s => Convert.ToInt32(s)).ToList();

            var expertSearchSimilarsForDelete = Executor.GetQuery<GetExpertSearchSimilarByIdRangeQuery>()
                .Process(q => q.Execute(similarIds));

            Executor.GetCommand<DeleteExpertSearchSimilarRangeCommand>()
                .Process(c => c.Execute(expertSearchSimilarsForDelete));

            var expertSearchSimilars = await Executor.GetQuery<GetExpertSearchSimilarsByRequestIdQuery>()
                .Process(q => q.Execute(ownerId));

            var expertSearchDtos = await GetMappedExpertSearchDtos(expertSearchSimilars);

            return Ok(expertSearchDtos);
        }

        // GET: api/Search/expert/{id}/getSimilarResults
        [HttpGet("expert/{id}/similarResults")]
        public async Task<IActionResult> GetSimilarSearchResults(int id)
        {
            var expertSearchSimilars = await Executor.GetQuery<GetExpertSearchSimilarsByRequestIdQuery>()
                .Process(q => q.Execute(id));

            var expertSearchDtos = await GetMappedExpertSearchDtos(expertSearchSimilars);

            return Ok(expertSearchDtos);
        }

        #endregion

        private FileStreamResult GetExelByItems<T>(IEnumerable<T> items)
        {
            var fileStream = Executor.GetCommand<GetExcelFileCommand>().Process(q => q.Execute(items, Request));
            return File(fileStream, GetExcelFileCommand.ContentType, GetExcelFileCommand.DefaultFileName);
        }

        #region Helpers

        private IActionResult GetListResult<T>(IQueryable<T> queryable)
        {
            var listInternal = GetListInternal(queryable);

            return listInternal
                .ToPagedList(Request.GetPaginationParams())
                .AsOkObjectResult(Response);
        }

        private IQueryable<T> GetListInternal<T>(IQueryable<T> queryable)
        {
            return queryable
                .Filter(Request.Query)
                .Sort(Request.Query);
        }

        private IActionResult GetExcelInternal<T>(IQueryable<T> queryable)
        {
            var listInternal = GetListInternal(queryable);

            var fileStream = Executor.GetCommand<GetExcelFileCommand>().Process(q => q.Execute(listInternal, Request));
            return File(fileStream, GetExcelFileCommand.ContentType, GetExcelFileCommand.DefaultFileName);
        }

        private async Task<List<SimilarProtectionDocDto>> GetMappedExpertSearchDtos(List<ExpertSearchSimilar> expertSearchSimilars)
        {
            var dtos = new List<SimilarProtectionDocDto>();

            foreach (var similar in expertSearchSimilars)
            {
                var dto = new SimilarProtectionDocDto();

                Request request = null;
                ProtectionDoc protectionDoc = null;
                Request protectionDocRequest = null;

                switch (similar.OwnerType)
                {
                    case Owner.Type.Request:
                        request = await Executor.GetQuery<GetRequestByIdQuery>()
                            .Process(q => q.ExecuteAsync(similar.SimilarRequestId ?? 0));

                        if (similar.SimilarRequestId != null)
                            dto.Id = similar.SimilarRequestId.Value;
                        
                        dto.NameRu = request.NameRu;
                        dto.NameKz = request.NameKz;
                        dto.NameEn = request.NameEn;

                        break;
                    case Owner.Type.ProtectionDoc:
                        protectionDoc = await Executor.GetQuery<GetProtectionDocByIdQuery>()
                            .Process(q => q.ExecuteAsync(similar.SimilarProtectionDocId ?? 0));
                        protectionDocRequest = await Executor.GetQuery<GetRequestByIdQuery>()
                            .Process(q => q.ExecuteAsync(protectionDoc?.RequestId ?? 0));

                        if (similar.SimilarProtectionDocId != null)
                            dto.Id = similar.SimilarProtectionDocId.Value;
                        
                        dto.NameRu = protectionDoc.NameRu;
                        dto.NameKz = protectionDoc.NameKz;
                        dto.NameEn = protectionDoc.NameEn;
                        dto.ExtensionDateTz = protectionDoc.ExtensionDateTz;
                        dto.Gosreestr = protectionDoc.Gosreestr;

                        break;
                }

                dto.ExpertSearchSimilarId = similar.Id;
                dto.ImageSimilarity = similar.ImageSimilarity;
                dto.PhonSimilarity = similar.PhonSimilarity;
                dto.SemSimilarity = similar.SemSimilarity;
                dto.ProtectionDocCategory = similar.ProtectionDocCategory;

                dto.StatusNameRu = GetStatus(request, protectionDoc);
                dto.PreviewImage = GetPreviewImage(request, protectionDocRequest);
                dto.GosNumber = GetGosNumber(protectionDoc);
                dto.GosDate = GetGosDate(protectionDoc);
                dto.RegNumber = GetRegNumber(request, protectionDoc);
                dto.RegDate = GetRegDate(request, protectionDoc);
                dto.Declarant = GetCustomerNameByRoleCode(request, protectionDocRequest, DicCustomerRoleCodes.Declarant);
                dto.OwnerName = GetCustomerNameByRoleCode(request, protectionDocRequest, DicCustomerRoleCodes.Owner);
                dto.Icgs = GetIcgs(request, protectionDocRequest);
                dto.Icfems = GetIcfem(request, protectionDocRequest, false);
                dto.Colors = GetIcfem(request, protectionDocRequest, true);
                dto.ValidDate = GetValidDate(protectionDoc);
                dto.DisclaimerRu = GetDisclamationRu(request, protectionDocRequest);
                dto.DisclaimerKz = GetDisclamationKz(request, protectionDocRequest);

                dtos.Add(dto);
            }

            return dtos;
        }

        private List<ExpertSearchSimilar> GetMappedSimilars(ExpertSearchSimilarDto[] similarities)
        {
            return similarities.Select(s =>
            {
                var expertSearchSimilar = Executor.GetQuery<GetExpertSearchSimilarByIdQuery>()
                    .Process(q => q.Execute(s.Id));

                expertSearchSimilar.ProtectionDocFormula = s.ProtectionDocFormula;
                expertSearchSimilar.ProtectionDocCategory = s.ProtectionDocCategory;
                return expertSearchSimilar;
            }).ToList();
        }

        private string GetRegNumber(Request request, ProtectionDoc protectionDoc)
        {
            return request?.RequestNum ?? protectionDoc?.RegNumber ?? string.Empty;
        }

        private DateTimeOffset? GetRegDate(Request request, ProtectionDoc protectionDoc)
        {
            return request?.RequestDate ?? protectionDoc?.RegDate;
        }

        private string GetGosNumber(ProtectionDoc protectionDoc)
        {
            return protectionDoc?.GosNumber ?? string.Empty;
        }

        private DateTimeOffset? GetGosDate(ProtectionDoc protectionDoc)
        {
            return protectionDoc?.GosDate;
        }

        private DateTimeOffset? GetValidDate(ProtectionDoc protectionDoc)
        {
            return protectionDoc?.ValidDate;
        }

        private string GetCustomerNameByRoleCode(Request request, Request protectionDocRequest, string roleCode)
        {
            List<DicCustomer> result = null;

            if (request != null)
            {
                result = request.RequestCustomers?.Where(rc => rc.CustomerRole.Code == roleCode)
                    .Select(rc => rc.Customer)
                    .ToList();
            }

            if (protectionDocRequest != null)
            {
                result = protectionDocRequest.RequestCustomers?.Where(rc => rc.CustomerRole.Code == roleCode)
                    .Select(rc => rc.Customer)
                    .ToList();
            }

            if (result == null || result.Count == 0)
            {
                return null;
            }

            return string.Join("; ", result.Select(c => c.NameRu ?? c.NameKz ?? c.NameEn ?? string.Empty));
        }

        private List<string> GetIcgs(Request request, Request protectionDocRequest)
        {
            List<string> result = null;

            if (request != null)
            {
                result = request.ICGSRequests?.Select(i => $"{i.Icgs.Code} - {i.ClaimedDescription}")
                    .ToList();
            }

            if (protectionDocRequest != null)
            {
                result = protectionDocRequest.ICGSRequests?.Select(i => $"{i.Icgs.Code} - {i.ClaimedDescription}")
                    .ToList();
            }

            if (result == null || result.Count == 0)
            {
                return null;
            }

            return result;
        }

        private string GetDisclamation(Request request, Request protectionDocRequest)
        {
            return request?.DisclaimerRu ?? request?.DisclaimerKz ?? request?.DisclaimerEn ??
                protectionDocRequest?.DisclaimerRu ?? protectionDocRequest?.DisclaimerKz ??
                protectionDocRequest?.DisclaimerEn ?? string.Empty;
        }

        private string GetDisclamationRu(Request request, Request protectionDocRequest)
        {
            return request?.DisclaimerRu ?? protectionDocRequest?.DisclaimerRu ?? string.Empty;
        }

        private string GetDisclamationKz(Request request, Request protectionDocRequest)
        {
            return request?.DisclaimerKz ?? protectionDocRequest?.DisclaimerKz ?? string.Empty;
        }

        private string GetIcfem(Request request, Request protectionDocRequest, bool isColor)
        {
            List<string> result = null;

            if (isColor)
            {
                if (request != null)
                {
                    result = request.Icfems?.Where(i => i.DicIcfem.Code.StartsWith(DicICFEMCodes.Colors))
                        .Select(i => i.DicIcfem.Code)
                        .ToList();
                }
                if (protectionDocRequest != null)
                {
                    result = protectionDocRequest.Icfems?.Where(i => i.DicIcfem.Code.StartsWith(DicICFEMCodes.Colors))
                        .Select(i => i.DicIcfem.Code)
                        .ToList();
                }
            }
            else
            {
                if (request != null)
                {
                    result = request.Icfems?.Where(i => !i.DicIcfem.Code.StartsWith(DicICFEMCodes.Colors))
                        .Select(i => i.DicIcfem.Code)
                        .ToList();
                }
                if (protectionDocRequest != null)
                {
                    result = protectionDocRequest.Icfems?.Where(i => !i.DicIcfem.Code.StartsWith(DicICFEMCodes.Colors))
                        .Select(i => i.DicIcfem.Code)
                        .ToList();
                }
            }

            if (result == null || result.Count == 0)
            {
                return null;
            }

            return string.Join("; ", result);
        }

        private string GetPreviewImage(Request request, Request protectionDocRequest)
        {
            if (request?.PreviewImage != null)
            {
                return $"data:image/png;base64,{Convert.ToBase64String(request.PreviewImage)}";
            }

            if (protectionDocRequest?.PreviewImage != null)
            {
                return $"data:image/png;base64,{Convert.ToBase64String(protectionDocRequest.PreviewImage)}";
            }

            return null;
        }
        
        private string GetStatus(Request request, ProtectionDoc protectionDoc)
        {
            if (request != null)
            {
                if (request.StatusId != null)
                {
                    return request.Status.NameRu;
                }
            }

            if (protectionDoc != null)
            {
                if (protectionDoc.StatusId != null)
                {
                    return protectionDoc.Status.NameRu;
                }
            }

            return null;
        }
        
        #endregion
    }
}