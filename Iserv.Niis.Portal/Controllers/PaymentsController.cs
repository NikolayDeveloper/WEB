using System;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Features.Payment.Invoices;
using Iserv.Niis.Model.Models.Payment;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using List = Iserv.Niis.Features.Payment.List;

namespace Iserv.Niis.Portal.Controllers
{
    [Produces("application/json")]
    [Route("api/Payments")]
    public class PaymentsController : Controller
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Payments/123456789009
        [HttpGet("{xin}")]
        public async Task<IActionResult> Get(string xin)
        {
            var result = await _mediator.Send(new List.Query(xin));

            return Ok(result);
        }
 
        // POST: api/Payments
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]string value)
        {
            throw new NotImplementedException();
        }

        // PUT: api/Payments/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
            throw new NotImplementedException();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            throw new NotImplementedException();
        }

        #region Invoices

        //GET: api/Payments/invoices/1
        [HttpGet("invoices/{ownerType}/{ownerId}")]
        public async Task<IActionResult> GetInvoices(int ownerId, Owner.Type ownerType)
        {
            var result = await _mediator.Send(new Features.Payment.Invoices.List.Query(ownerId, ownerType));

            return Ok(result);
        }

        [HttpPost("invoices/{ownerType}")]
        public async Task<IActionResult> Post(Owner.Type ownerType, [FromBody] PaymentInvoiceDto invoiceDto)
        {
            var result = await _mediator.Send(new Create.Command(invoiceDto, ownerType));

            return Ok(result);
        }

        #endregion

        #region Uses

        [HttpPost("uses/{ownerType}")]
        public async Task<IActionResult> Post(Owner.Type ownerType, [FromBody] PaymentUseDto useDto)
        {
            var result = await _mediator.Send(new Features.Payment.Uses.Create.Command(useDto, ownerType));

            return Ok(result);
        }

        #endregion

    }
}
