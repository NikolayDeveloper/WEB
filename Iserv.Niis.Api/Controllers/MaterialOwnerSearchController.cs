using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Iserv.Niis.BusinessLogic.Contracts;
using Iserv.Niis.BusinessLogic.ProtectionDocs;
using Iserv.Niis.BusinessLogic.Requests;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Infrastructure.Extensions.Filter;
using Iserv.Niis.Infrastructure.Pagination;
using Iserv.Niis.Model.Models.Search;
using Microsoft.AspNetCore.Mvc;

namespace Iserv.Niis.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/MaterialOwnerSearch")]
    public class MaterialOwnerSearchController : BaseNiisApiController
    {
        [HttpGet]
        public IActionResult GetIntellectualProperty()
        {
            var filteredQuery = GetFilteredItems();

            var sortedQuery = filteredQuery;
            if (Request.Query.ContainsKey("_sort"))
            {
                sortedQuery = sortedQuery.Sort(Request.Query);
            }
            else
            {
                sortedQuery = sortedQuery.OrderByDescending(r => r.DateCreate);
            }

            var getPaginationParams = Request.GetPaginationParams();

            var result = sortedQuery.ToPagedList(getPaginationParams);

            return result.AsOkObjectResult(Response);
        }

        private IQueryable<IntellectualPropertySearchDto> GetFilteredItems()
        {
            IQueryable<IntellectualPropertySearchDto> query = null;

            if (Request.Query.ContainsKey("type_eq"))
            {
                var documentType = int.Parse(Request.Query["type_eq"]);
                switch (documentType)
                {
                    case 1:
                        query = Executor.GetQuery<GetRequestsQuery>().Process(q => q.Execute())
                            .ProjectTo<IntellectualPropertySearchDto>();
                        break;
                    case 2:
                        query = Executor.GetQuery<GetProtectionDocsQuery>().Process(q => q.Execute())
                            .ProjectTo<IntellectualPropertySearchDto>();
                        break;
                    case 3:
                        query = Executor.GetQuery<GetContractsQuery>().Process(q => q.Execute())
                            .ProjectTo<IntellectualPropertySearchDto>();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            else if (Request.Query.ContainsKey("requestTypeId_eq"))
            {
                var typeId = int.Parse(Request.Query["requestTypeId_eq"]);
                query = Executor.GetQuery<GetRequestsQuery>().Process(q => q.ExecuteByProtectionDocTypeId(typeId))
                            .ProjectTo<IntellectualPropertySearchDto>();
            }
            else if (Request.Query.ContainsKey("typeId_eq"))
            {
                var typeId = int.Parse(Request.Query["typeId_eq"]);
                query = Executor.GetQuery<GetProtectionDocsQuery>().Process(q => q.ExecuteByRequestTypeId(typeId))
                            .ProjectTo<IntellectualPropertySearchDto>();
            }
            else
            {
                throw new ArgumentException("В веб-запросе отсутствует ключ фильтрации по типам объектов!!!");
            }

            if (Request.Query.ContainsKey("number_eq"))
            {
                var regNumbers = Request.Query["number_eq"].ToString();
                var existNumbersArray = new List<string>();
                foreach (var regNumber in regNumbers.Split(';', ',').Where(d => !string.IsNullOrEmpty(d)))
                {
                    existNumbersArray.Add(regNumber.Trim());
                }

                if (query != null)
                    query = query.Where(d => existNumbersArray.Any(rn => rn == d.Number));
            }

            var filteredQuery = query.Filter(Request.Query);

            return filteredQuery;
        }

        [HttpPost("getOwners")]
        public IActionResult GetSelectedOwners([FromBody]IntellectualPropertySearchDto[] dtos)
        {
            var filteredQuery = GetFilteredItems();

            var allDtos = filteredQuery.ToArray();

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
                dtos = allDtos;
            }
            else
            {
                if (selectionMode == SelectionMode.Except)
                {
                    dtos = allDtos.Where(r => !dtos.Any(d => r.Id == d.Id && r.Type == d.Type)).ToArray();
                }
            }

            return Ok(dtos);
        }
    }
}