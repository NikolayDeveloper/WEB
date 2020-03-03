using Iserv.Niis.BusinessLogic.Excel;
using Iserv.Niis.BusinessLogic.PaymentsJournal;
using Iserv.Niis.BusinessLogic.PaymentUses;
using Iserv.Niis.Model.Models.Payment;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Iserv.Niis.Services.Interfaces;


using GetPaymentUseByIdQuery = Iserv.Niis.BusinessLogic.PaymentUses.GetPaymentUseByIdQuery;

namespace Iserv.Niis.Api.Controllers.PaymentsJournal
{
    [Produces("application/json")]
    [Route("api/PaymentsJournal/PaymentUses")]
    public class PaymentUsesController : BaseNiisApiController
    {
        private readonly IPaymentService _paymentService;

        public PaymentUsesController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }


        [HttpGet("{paymentId}")]
        public IActionResult Get(int paymentId)
        {
            var paymentUses = Executor.GetQuery<GetPaymentUsesByPaymentIdQuery>()
                .Process(q => q.Execute(paymentId));

            return Ok(paymentUses);
        }

        [HttpGet("GetExcel/{paymentId}")]
        public IActionResult GetExcel(int paymentId)
        {
            var paymentUses = Executor.GetQuery<GetPaymentUsesByPaymentIdQuery>()
                .Process(q => q.Execute(paymentId));

            var fileStream = Executor.GetCommand<GetExcelFileCommand>().Process(q => q.Execute(paymentUses, Request));
            return File(fileStream, GetExcelFileCommand.ContentType, GetExcelFileCommand.DefaultFileName);
        }

        [HttpGet("{paymentUseId}/invoice/charged")]
        public async Task<IActionResult> IsCharged(int paymentUseId)
        {
            var paymentUse = await Executor.GetQuery<GetPaymentUseByIdQuery>()
                .Process(q => q.ExecuteAsync(paymentUseId));

            return Ok(paymentUse.PaymentInvoice.DateComplete != null);
        }

        [HttpPost("{paymentUseId}/delete")]
        public async Task<IActionResult> DeletePaymentUse(int paymentUseId, [FromBody] DeletePaymentUseRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                throw new ValidationException("Model is not valid.");
            }

            await Executor.GetCommand<DeletePaymentUseCommand>()
                .Process(c => c.ExecuteAsync(paymentUseId, requestDto));

            return Ok();
        }

        [HttpGet("{paymentUseId}/edit")]
        public async Task<IActionResult> GetPaymentUseForEdit(int paymentUseId)
        {
            var responseDto = await Executor.GetQuery<GetPaymentUseForEditQuery>()
                .Process(q => q.ExecuteAsync(paymentUseId));

            return Ok(responseDto);
        }

        [HttpPost("{paymentUseId}/edit")]
        public async Task<IActionResult> EditPaymentUse(int paymentUseId, [FromBody] EditPaymentUseRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                throw new ValidationException("Model is not valid.");
            }

            var responseDto = await Executor.GetCommand<EditPaymentUseCommand>()
                .Process(q => q.ExecuteAsync(paymentUseId, requestDto));

            return Ok(responseDto);
        }

        [HttpGet("getStatementFromBank/{paymentUseId}")]
        public async Task<IActionResult> GetStatementFromBank(int paymentUseId)
        {
            var statementFromBank = await _paymentService.GetStatementFromBank(paymentUseId);

            return File(statementFromBank.File, statementFromBank.ContentType);
        }
    }
}