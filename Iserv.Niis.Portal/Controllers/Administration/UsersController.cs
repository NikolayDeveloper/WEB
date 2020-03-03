using System.Threading.Tasks;
using Iserv.Niis.Features.Administration.ApplicationUser;
using Iserv.Niis.Model.Models.User;
using Iserv.Niis.Portal.Infrastructure.Extensions.Filter;
using Iserv.Niis.Portal.Infrastructure.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iserv.Niis.Portal.Controllers.Administration
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }


        // GET: api/Users
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = await _mediator.Send(new List.Query());

            return query
                .Filter(Request.Query)
                .Sort(Request.Query)
                .ToPagedList(Request.GetPaginationParams())
                .AsOkObjectResult(Response);
        }

        // GET: api/Users/list
        [HttpGet("list")]
        public async Task<IActionResult> GetList()
        {
            var query = await _mediator.Send(new List.Query());

            return Ok(query);
        }

        // GET: api/Users/5
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new Single.Query(id));

            return Ok(result);
        }

        // POST: api/Users
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserDetailsDto model)
        {
            var result = await _mediator.Send(new Create.Command(model));
            return new CreatedAtRouteResult("GetUser", result);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UserDetailsDto model)
        {
            await _mediator.Send(new Update.Command(id, model));

            return NoContent();
        }
    }
}