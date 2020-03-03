using System.Threading.Tasks;
using Iserv.Niis.Domain.Constants;
using Iserv.Niis.Features.Journal.StaffTasks;
using Iserv.Niis.Portal.Infrastructure.Extensions.Filter;
using Iserv.Niis.Portal.Infrastructure.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iserv.Niis.Portal.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize(Policy = KeyFor.Policy.HasAccessToJournal)]
    public class JournalController : Controller
    {

        private readonly IMediator _mediator;

        public JournalController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [Authorize(Policy = KeyFor.Policy.HasAccessToViewStaffTasks)]
        [HttpGet("staff")]
        public async Task<IActionResult> Staff()
        {
            var query = await _mediator.Send(new List.Query());
            return query
                .Filter(Request.Query)
                .Sort(Request.Query)
                .ToPagedList(Request.GetPaginationParams())
                .AsOkObjectResult(Response);
        }
    }
}
