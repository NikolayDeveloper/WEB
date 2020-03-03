using System;
using System.Threading.Tasks;
using Iserv.Niis.Features.Administration.ApplicationRole;
using Iserv.Niis.Model.Models.Role;
using Iserv.Niis.Portal.Infrastructure.Extensions.Filter;
using Iserv.Niis.Portal.Infrastructure.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Single = Iserv.Niis.Features.Administration.ApplicationRole.Single;

namespace Iserv.Niis.Portal.Controllers.Administration
{
    [Produces("application/json")]
    [Route("api/Roles")]
    public class RolesController : Controller
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Roles
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

        // GET: api/Roles/5
        [HttpGet("{id}", Name = "GetRole")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new Single.Query(id));

            return Ok(result);
        }

        // POST: api/Roles
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RoleDetailsDto model)
        {
            await _mediator.Send(new Create.Command(model));

            return NoContent();
        }

        // PUT: api/Roles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]RoleDetailsDto model)
        {
            await _mediator.Send(new Update.Command { RoleDetailsDto = model, RoleId = id });

            return NoContent();
        }

        [HttpGet("select")]
        public async Task<IActionResult> Select()
        {
            var query = await _mediator.Send(new ListSelect.Query());

            return Ok(query);
        }

        [HttpGet("prm")]
        public async Task<IActionResult> Permissions()
        {
            var result = await _mediator.Send(new ListClaims.Query());

            return Ok(result);
        }

        [HttpGet("stagesTree")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStagesTree()
        {
            var result = await _mediator.Send(new GetRouteStagesTree.Query());

            return Ok(result);
        }
    }
}
