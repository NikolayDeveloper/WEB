using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.Features.ExpertSearch;
using Iserv.Niis.Features.ExpertSearch.Similarities;
using Iserv.Niis.Features.Search;
using Iserv.Niis.Model.Models.ExpertSearch;
using Iserv.Niis.Portal.Infrastructure.Extensions.Filter;
using Iserv.Niis.Portal.Infrastructure.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using List = Iserv.Niis.Features.Search.List;

namespace Iserv.Niis.Portal.Controllers
{
	[Produces("application/json")]
	[Route("api/Search")]
	public class SearchController : Controller
	{
		private readonly IFileExporter _fileExporter;
		private readonly IMediator _mediator;
		private const string ExcelfieldsKey = "excelFields";

		public SearchController(IMediator mediator, IFileExporter fileExporter)
		{
			_mediator = mediator;
			_fileExporter = fileExporter;
		}

		#region Simple search

		// GET: api/Search
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var queryInstance = new List.Query();

			return await GetListResult(queryInstance);
		}

		// GET: api/Search/GetExcel
		[HttpGet("GetExcel")]
		public async Task<IActionResult> GetExcel()
		{
			var queryInstance = new List.Query();

			return await GetExcelInternal(queryInstance);
		}

	    [HttpGet("intellectualProperty")]
	    public async Task<IActionResult> GetIntellectualProperty()
	    {
	        var result = await _mediator.Send(new IntellectualPropertyList.Query());

	        return Ok(result);
	    }

	    #endregion

        #region Advanced search - request

        // GET: api/Search/request
        [HttpGet("request")]
		public async Task<IActionResult> GetRequest()
		{
			var queryInstance = new RequestList.Query();

			return await GetListResult(queryInstance);
		}

		// GET: api/Search/request/GetExcel
		[HttpGet("request/GetExcel")]
		public async Task<IActionResult> GetRequestExcel()
		{
			var queryInstance = new RequestList.Query();

			return await GetExcelInternal(queryInstance);
		}

		#endregion

		#region Advanced search - protectionDoc

		// GET: api/Search/protectionDoc
		[HttpGet("protectionDoc")]
		public async Task<IActionResult> GetProtectionDoc()
		{
			var queryInstance = new ProtectionDocList.Query();

			return await GetListResult(queryInstance);
		}

		// GET: api/Search/protectionDoc/GetExcel
		[HttpGet("protectionDoc/GetExcel")]
		public async Task<IActionResult> GetProtectionDocExcel()
		{
			var queryInstance = new ProtectionDocList.Query();

			return await GetExcelInternal(queryInstance);
		}

		#endregion

		#region Advanced search - contract

		// GET: api/Search/contract
		[HttpGet("contract")]
		public async Task<IActionResult> GetContract()
		{
			var queryInstance = new ContractList.Query();

			return await GetListResult(queryInstance);
		}

		// GET: api/Search/contract/GetExcel
		[HttpGet("contract/GetExcel")]
		public async Task<IActionResult> GetContractExcel()
		{
			var queryInstance = new ContractList.Query();

			return await GetExcelInternal(queryInstance);
		}

		#endregion

		#region Advanced search - document

		// GET: api/Search/document
		[HttpGet("document")]
		public async Task<IActionResult> GetDocument()
		{
			var queryInstance = new DocumentList.Query();

			return await GetListResult(queryInstance);
		}

		// GET: api/Search/document/GetExcel
		[HttpGet("document/GetExcel")]
		public async Task<IActionResult> GetDocumentExcel()
		{
			var queryInstance = new DocumentList.Query();

			return await GetExcelInternal(queryInstance);
		}

		#endregion

		#region Export search - trademark

		// GET: api/Search/expert/trademark
		[HttpGet("expert/trademark")]
		public async Task<IActionResult> GetTrademark()
		{
			var queryInstance = new TrademarkList.Query();

			return await GetListResult(queryInstance);
		}

		// GET: api/Search/expert/trademark/GetExcel
		[HttpGet("expert/trademark/GetExcel")]
		public async Task<IActionResult> GetTrademarkExcel()
		{
			var queryInstance = new TrademarkList.Query();

			return await GetExcelInternal(queryInstance);
		}

		#endregion

		#region Export search - invention

		// GET: api/Search/expert/invention
		[HttpGet("expert/invention")]
		public async Task<IActionResult> GetInvention()
		{
			var queryInstance = new InventionList.Query();

			return await GetListResult(queryInstance);
		}

		// GET: api/Search/expert/invention/GetExcel
		[HttpGet("expert/invention/GetExcel")]
		public async Task<IActionResult> GetInventionExcel()
		{
			var queryInstance = new InventionList.Query();

			return await GetExcelInternal(queryInstance);
		}

		#endregion

		#region Export search - usefulModel

		// GET: api/Search/expert/usefulModel
		[HttpGet("expert/usefulModel")]
		public async Task<IActionResult> GetUsefulModel()
		{
			var queryInstance = new UsefulModelList.Query();

			return await GetListResult(queryInstance);
		}

		// GET: api/Search/expert/usefulModel/GetExcel
		[HttpGet("expert/usefulModel/GetExcel")]
		public async Task<IActionResult> GetUsefulModelExcel()
		{
			var queryInstance = new UsefulModelList.Query();

			return await GetExcelInternal(queryInstance);
		}

		#endregion

		#region Export search - industrialDesign

		// GET: api/Search/expert/industrialdesign
		[HttpGet("expert/industrialdesign")]
		public async Task<IActionResult> GetIndustrialDesign()
		{
			var queryInstance = new IndustrialDesignList.Query();

			return await GetListResult(queryInstance);
		}

		// GET: api/Search/expert/industrialdesign/GetExcel
		[HttpGet("expert/industrialdesign/GetExcel")]
		public async Task<IActionResult> GetIndustrialDesignExcel()
		{
			var queryInstance = new IndustrialDesignList.Query();

			return await GetExcelInternal(queryInstance);
		}

		#endregion

		#region Expert search - similarResults

		// POST: api/Search/expert/{id}/putSimilarResults
		[HttpPost("expert/{id}/similarResults")]
		public async Task<IActionResult> CreateSimilarSearchResults(int id,
			[FromBody] ExpertSearchSimilarDto[] expertSearchSimilarDtos)
		{
			//if (expertSearchSimilarDtos.Length > 0)
		//	{
				await _mediator.Send(new Create.Command(id, expertSearchSimilarDtos));
		//	}
			//return NoContent();
			var query = await _mediator.Send(new Features.ExpertSearch.Similarities.List.Query(id));
			return Ok(query.ToArray());
		}

	    [HttpPut("expert/{ownerType}/{ownerId}/similarResults/{keywords}")]
	    public async Task<IActionResult> UpdateSimilarSearchResults(int ownerId, Owner.Type ownerType, string keywords,
	        [FromBody] ExpertSearchSimilarDto[] similarities)
	    {
	        var query = await _mediator.Send(new Update.Command(similarities, ownerType, ownerId, keywords));

	        return Ok(query);
	    }

	    // POST: api/Search/expert/{id}/deleteSimilarResults
        [HttpDelete("expert/{ownerId}/similarResults/{similarityIds}")]
		public async Task<IActionResult> DeleteSimilarSearchResults(int ownerId, string similarityIds)
        {
            var ids = similarityIds.Split(';').Select(s => Convert.ToInt32(s));
			await _mediator.Send(new Delete.Command(ids));
			//return NoContent();
			var query = await _mediator.Send(new Features.ExpertSearch.Similarities.List.Query(ownerId));
			return Ok(query.ToArray());
		}

		// GET: api/Search/expert/{id}/getSimilarResults
		[HttpGet("expert/{id}/similarResults")]
		public async Task<IActionResult> GetSimilarSearchResults(int id)
		{
			var query = await _mediator.Send(new Features.ExpertSearch.Similarities.List.Query(id));

			return Ok(query.ToArray());
		}

		#endregion

		#region Helpers

		private async Task<IActionResult> GetListResult<T>(IRequest<IQueryable<T>> queryInstance)
		{
			var listInternal = await GetListInternal(queryInstance);

			return listInternal
				 .ToPagedList(Request.GetPaginationParams())
				 .AsOkObjectResult(Response);
		}

		private async Task<IQueryable<T>> GetListInternal<T>(IRequest<IQueryable<T>> queryInstance)
		{
			var query = await _mediator.Send(queryInstance);

			return query
				 .Filter(Request.Query)
				 .Sort(Request.Query);
		}

		private async Task<IActionResult> GetExcelInternal<T>(IRequest<IQueryable<T>> queryInstance)
		{
			var listInternal = await GetListInternal(queryInstance);
			var fileStream = _fileExporter.Export(listInternal, FileType.Xlsx, GetExcelFields());

			return File(fileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "file.xlsx");
		}

		private string[] GetExcelFields()
		{
			try
			{
				if (!Request.Query.TryGetValue(ExcelfieldsKey, out var values) || values.Count <= 0)
					return new string[0];

				if (values.Count > 1)
				{
					throw new Exception($"Allowed only one {ExcelfieldsKey} parameter");
				}

				return values.Single().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			}
			catch (Exception e)
			{
				Debug.WriteLine(e);
				throw new Exception("Excel export error! See inner exception for details.", e);
			}
		}

		#endregion
	}
}