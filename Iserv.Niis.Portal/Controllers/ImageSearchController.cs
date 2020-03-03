using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Features.ExpertSearch;
using Iserv.Niis.Features.Request;
using Iserv.Niis.FileStorage.Abstract;
using Iserv.Niis.Model.Models.ExpertSearch;
using Iserv.Niis.Model.Models.Request;
using Iserv.Niis.Portal.Infrastructure.Extensions;
using Iserv.Niis.Portal.Infrastructure.Extensions.Filter;
using Iserv.Niis.Portal.Infrastructure.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using ContentDispositionHeaderValue = System.Net.Http.Headers.ContentDispositionHeaderValue;
using SingleImage = Iserv.Niis.Features.ExpertSearch.SingleImage;
using System.IO;

namespace Iserv.Niis.Portal.Controllers
{
	[Produces("application/json")]
	[Route("api/ImageSearch")]
	public class ImageSearchController : Controller
	{
		private readonly NiisWebContext _context;
		private readonly IFileStorage _fileStorage;
		private readonly IMediator _mediator;
		private IMapper _mapper;

		public ImageSearchController(NiisWebContext context, IMapper mapper, IMediator mediator, IMapper mapper1)
		{
			_context = context;
			_mediator = mediator;
			_mapper = mapper1;
		}

		// GET: api/ImageSearch/5/searchbyimage
		[AllowAnonymous]
		[HttpGet("{id}/searchbyimage")]
		public async Task<IActionResult> SearchByImage(int id)
		{
			const bool isPhonetic = false;
			var query = await _mediator.Send(new ImageSearchList.Query(id, isPhonetic, User.Identity.GetUserId()));
			
			return Ok(query);
		}

		// GET: api/ImageSearch/5/imageandphonetic
		[AllowAnonymous]
		[HttpGet("{id}/searchbyimageandphonetic")]
		public async Task<IActionResult> SearchByImageAndPhonetic(int id)
		{
			const bool isPhonetic = true;
			var query = await _mediator.Send(new ImageSearchList.Query(id, isPhonetic, User.Identity.GetUserId()));
			return Ok(query.ToArray());
		}
		
	}
}
