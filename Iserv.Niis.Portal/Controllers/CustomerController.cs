using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Single = Iserv.Niis.Features.Customer.Single;

namespace Iserv.Niis.Portal.Controllers
{
    [Produces("application/json")]
    [Route("api/Customer")]
    public class CustomerController : Controller
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //GET: api/Customer/GetByXin/123456789123
        [HttpGet("GetByXin/{xin}/{isPatentAttorney}")]
        public async Task<IActionResult> GetByXin(string xin, bool? isPatentAttorney)
        {
            var result = await _mediator.Send(new Single.Query(xin, isPatentAttorney));

            return Ok(result);
        }
    }
}
