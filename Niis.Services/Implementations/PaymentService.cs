using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Iserv.Niis.BusinessLogic.Payments;
using Iserv.Niis.BusinessLogic.PaymentsJournal;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Exceptions;
using Iserv.Niis.FileConverter;
using Iserv.Niis.Model.Models.Payment;
using Iserv.Niis.Services.Interfaces;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IMapper _mapper;
        private readonly IExecutor _executor;
        private readonly IDocumentService _documentService;

        public PaymentService(
            IMapper mapper,
            IExecutor executor,
            IDocumentService documentService)
        {
            _mapper = mapper;
            _executor = executor;
            _documentService = documentService;
        }

        public async Task<GeneratedDocument> GetStatementFromBank(int paymentUseId)
        {
            var paymentUse = await _executor
                .GetQuery<GetPaymentUseByIdQuery>()
                .Process(query => query.ExecuteAsync(paymentUseId));
            var payment = _mapper.Map<PaymentDto>(await _executor
                .GetQuery<GetPaymentByIdQuery>()
                .Process(query => query.ExecuteAsync(paymentUse.PaymentId ?? 0)));

            if (paymentUse is null)
            {
                throw new DataNotFoundException(nameof(PaymentUse), DataNotFoundException.OperationType.Read, paymentUseId);
            }

            return _documentService.GenerateDocument(DicDocumentTypeCodes.StatementFromBank,
                new Dictionary<string, object>
                {
                    {"PaymentUse", paymentUse},
                    {"Payment", payment}
                });
        }
    }
}
