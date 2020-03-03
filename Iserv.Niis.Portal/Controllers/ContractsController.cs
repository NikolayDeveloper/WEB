using System.Threading.Tasks;
using Iserv.Niis.Features.Contract;
using Iserv.Niis.Model.Models.Contract;
using Iserv.Niis.Portal.Infrastructure.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iserv.Niis.Portal.Controllers
{
    [Route("api/[controller]")]
    public class ContractsController : Controller
    {
        private readonly IMediator _mediator;

        public ContractsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            throw new System.NotImplementedException();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new Single.Query(User.Identity.GetUserId(), r => r.Id == id));

            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ContractDetailDto detailDto)
        {
            var contract = await _mediator.Send(new Create.Command(detailDto, User.Identity.GetUserId()));
            return Ok(contract);
        }

        [HttpPut("register/{id}")]
        public async Task<IActionResult> RegisterContract(int id, [FromBody] ContractDetailDto detailDto)
        {
            var contract = await _mediator.Send(new Register.Command(id, detailDto, User.Identity.GetUserId()));
            return Ok(contract);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ContractDetailDto detailDto)
        {
            var contract = await _mediator.Send(new Update.Command(id, detailDto));
            return Ok(contract);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new Delete.Command(id));

            return NoContent();
        }
    }
}