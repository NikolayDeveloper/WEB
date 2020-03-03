using System.Threading.Tasks;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Features.BibliographicData;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iserv.Niis.Portal.Controllers
{
    [Route("api/BibliographicData")]
    public class BibliographicDataController : Controller
    {
        private readonly IMediator _mediator;

        public BibliographicDataController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{ownertype}/{ownerId}")]
        public async Task<IActionResult> Get(int ownerId, Owner.Type ownerType)
        {
            var result = await _mediator.Send(new List.Query(ownerId, ownerType));

            return Ok(result);
        }
    }
}