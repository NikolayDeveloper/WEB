using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicCommon;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Exceptions;

namespace Iserv.Niis.WorkflowBusinessLogic.PaymentInvoices
{
    public class CreatePaymentInvoiceHandler : BaseHandler
    {
        private readonly static decimal defaultCoefficient = 1;
        private static readonly decimal defaultNds = 0.12m;
        private readonly static int defaultTariffCount = 1;
        private readonly static int defaultPenaltyPercent = 0;
        public int Execute(Owner.Type ownerType, int ownerId, string tariffCode, string paymentStatusCode, int? userId, int? tariffCount = null)
        {
            var tariffId = Executor.GetQuery<GetDictionaryIdByEntityNameAndCodeQuery>().Process(q => q.Execute(nameof(DicTariff), tariffCode));
            if (tariffId.HasValue == false)
            {
                throw new DataNotFoundException(nameof(DicTariff),
                   DataNotFoundException.OperationType.Read, $"Parameters: {nameof(tariffCode)}");
            }
            var paymentStatusId = Executor.GetQuery<GetDictionaryIdByEntityNameAndCodeQuery>().Process(q => q.Execute(nameof(DicPaymentStatus), paymentStatusCode));
            if (paymentStatusId.HasValue == false)
            {
                throw new DataNotFoundException(nameof(DicPaymentStatus),
                   DataNotFoundException.OperationType.Read, $"Parameters: {nameof(paymentStatusCode)}");
            }
            PaymentInvoice paymentInvoice = new PaymentInvoice
            {
                Coefficient = defaultCoefficient,
                Nds = defaultNds,
                TariffCount = tariffCount ?? defaultTariffCount,
                PenaltyPercent = defaultPenaltyPercent,
                TariffId = tariffId.Value,
                StatusId = paymentStatusId.Value,
                CreateUserId = userId
            };
            switch (ownerType)
            {
                case Owner.Type.Request:
                    paymentInvoice.RequestId = ownerId;
                    break;
                case Owner.Type.Contract:
                    paymentInvoice.ContractId = ownerId;
                    break;

                case Owner.Type.ProtectionDoc:
                    paymentInvoice.ProtectionDocId = ownerId;
                    break;
            }

            Executor.GetCommand<CreatePaymentInvoiceCommand>().Process(c => c.Execute(paymentInvoice));
            return paymentInvoice.Id;
        }
    }
}
