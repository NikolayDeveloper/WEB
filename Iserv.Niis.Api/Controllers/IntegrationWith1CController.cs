using AutoMapper;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.BusinessLogic._1CIntegration;
using Iserv.Niis.BusinessLogic.Security;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models.IntegrationWith1C;
using Microsoft.AspNetCore.Mvc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using System;
using System.Threading.Tasks;
using Iserv.Niis.Exceptions;

namespace Iserv.Niis.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/IntegrationWith1C")]
    public class IntegrationWith1CController : BaseNiisApiController
    {
        private readonly IExecutor _executor;
        private readonly IMapper _mapper;
        private readonly IImportPaymentsFrom1CService _importService;

        public IntegrationWith1CController(IExecutor executor, IMapper mapper, IImportPaymentsFrom1CService importService)
        {
            _executor = executor;
            _mapper = mapper;
            _importService = importService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Owner.Type ownerType, int paymentInvoiceId)
        {
            var success = await Executor.GetHandler<ExportPaymentTo1CHandler>().Process(r => r.ExecuteAsync(ownerType, paymentInvoiceId));

            return new OkResult();
        }

        [HttpPost("importpayments")]
        public async Task<IActionResult> ImportPayments([FromBody] ImportPaymentsRequestDto requestDto)
        {
            var user = _executor.GetQuery<GetUserByIdQuery>()
                .Process(q => q.Execute(NiisAmbientContext.Current.User.Identity.UserId));

            try
            {
                var importedNumber = await _importService.ImportPaymentsAsync(requestDto.PaymentsDate,
                    requestDto.PaymentsDate,
                    user.Id, user.NameRu, user.Position?.NameRu);
                var responseDto = new ImportPaymentsResponseDto {ImportedNumber = importedNumber};

                return Ok(responseDto);
            }
            catch (ComException exc)
            {
                var responseDto = new ImportPaymentsResponseDto {Error = true};

                switch (exc.ExceptionType)
                {
                    case ComExceptionType.CannotCreateComConnectorInstance:
                        responseDto.ErrorType = ImportPaymentsErrorType.CannotCreateComConnectorInstance;
                        break;
                    case ComExceptionType.CannotConnectTo1CDatabase:
                        responseDto.ErrorType = ImportPaymentsErrorType.CannotConnectTo1CDatabase;
                        break;
                    case ComExceptionType.UnknownComError:
                        responseDto.ErrorType = ImportPaymentsErrorType.UnknownComError;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(exc.ExceptionType));
                }

                return Ok(responseDto);
            }
            catch
            {
                var responseDto = new ImportPaymentsResponseDto { Error = true };
                return Ok(responseDto);
            }
        }
    }
}