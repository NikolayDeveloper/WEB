using Iserv.Niis.BusinessLogic.PaymentsJournal;
using Iserv.Niis.BusinessLogic.PaymentsJournal.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Iserv.Niis.Api.Controllers.PaymentsJournal
{
    [Produces("application/json")]
    [Route("api/PaymentsJournal/PaymentInvoices")]
    public class PaymentInvoicesController : BaseNiisApiController
    {
        [HttpGet("{documentId}/{documentCategory}")]
        public IActionResult Get(int documentId, DocumentCategory documentCategory)
        {
            var paymentUses = Executor.GetQuery<GetPaymentInvoicesByDocumentQuery>().Process(q => q.Execute(documentId, documentCategory));

            return Ok(paymentUses);
        }

        [HttpPost("bound")]
        public IActionResult Bound([FromBody]BoundPaymentDto boundPaymentDto)
        {
            Executor.GetCommand<BoundPaymentCommand>().Process(c => c.Execute(boundPaymentDto));

            return Ok();
        }

        [HttpGet("LinkedPayments/{paymentInvoiceId}")]
        public IActionResult GetLinkedPayments(int paymentInvoiceId)
        {
            var payments = Executor.GetQuery<GetLinkedPaymentsQuery>().Process(q => q.Execute(paymentInvoiceId));

            return this.Ok(payments);
        }
    }
}
