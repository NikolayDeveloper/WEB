using System;
using System.Threading.Tasks;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.Features.Journal.IntellectualProperties;
using Iserv.Niis.Features.Request;
using Iserv.Niis.Model.Models.Request;
using Iserv.Niis.Portal.Infrastructure.Extensions;
using Iserv.Niis.Portal.Infrastructure.Extensions.Filter;
using Iserv.Niis.Portal.Infrastructure.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Single = Iserv.Niis.Features.Request.Single;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Iserv.Niis.Portal.Controllers
{
    [Route("api/[controller]")]
    public class RequestsController : Controller
    {
        private readonly IMediator _mediator;
        
        /*
        public RequestsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        */

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = await _mediator.Send(new List.Query { UserId = User.Identity.GetUserId() });

            return query
                .Filter(Request.Query)
                .Sort(Request.Query)
                .ToPagedList(Request.GetPaginationParams())
                .AsOkObjectResult(Response);
        }

        [HttpGet("shortInformation/{protectionDocCode}")]
        public async Task<IActionResult> GetList(string protectionDocCode)
        {
            var query = await _mediator.Send(new ShortInformationList.Query(protectionDocCode));

            return Ok(query);
        }

        [HttpPost("icgsRequests")]
        public async Task<IActionResult> GetICGSRequestsList([FromBody] int[] requestIds)
        {
            var query = await _mediator.Send(new ICGSRequestListByRequestIds.Query(requestIds));

            return Ok(query);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new Single.Query(User.Identity.GetUserId(), r => r.Id == id));

            return Ok(result);
        }

        [HttpGet("bycode/{code}")]
        public async Task<IActionResult> Get(string code)
        {
            var result = await _mediator.Send(new ListByCode.Query(code, User.Identity.GetUserId()));

            return Ok(result);
        }

        [HttpGet("bynumber")]
        public async Task<IActionResult> GetByNumber()
        {
            var paramString = Request.QueryString.Value.Replace("?", "").Replace("=", "").Split('$');
            if (paramString.Length < 2)
            {
                throw new DataNotFoundException(nameof(Domain.Entities.Request.Request),
                    DataNotFoundException.OperationType.Read, Request.QueryString.Value);
            }
            var requestnum = paramString[0];
            var protectionDocTypeId = Convert.ToInt32(paramString[1]);
            var result = await _mediator.Send(new Single.Query(User.Identity.GetUserId(), r =>
                r.RequestNum.Equals(requestnum) &&
                r.ProtectionDocTypeId == protectionDocTypeId));

            return Ok(result);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RequestDetailDto detailDto)
        {
            var result = await _mediator.Send(new Create.Command(detailDto, User.Identity.GetUserId()));

            return Ok(result);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] RequestDetailDto detailDto)
        {
            var result = await _mediator.Send(new Update.Command(id, detailDto));

            return Ok(result);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new Delete.Command(id));

            return NoContent();
        }

        [HttpPost("{id}/upload")]
        public async Task<IActionResult> Upload(int id)
        {
            var result = await _mediator.Send(new UploadImage.Command(id, Request.Form.Files[0]));

            return Ok(new { url = result });
        }

        [AllowAnonymous]
        [HttpGet("{id}/image/{isPreview?}")]
        public async Task<IActionResult> Image(int id, bool isPreview = false)
        {
            var result = await _mediator.Send(new SingleImage.Command(id, isPreview));

            return File(result, "image/png");
        }

        [HttpGet("generateNumber/{id}")]
        public async Task<IActionResult> GenerateRequestNumber(int id)
        {
            var result = await _mediator.Send(new GenerateRequestNumber.Command(id));
            return Ok(new { number = result });
        }
    }
}