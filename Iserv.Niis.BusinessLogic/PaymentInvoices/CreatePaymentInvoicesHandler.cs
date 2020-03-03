using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicCommon;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.PaymentInvoices
{
    public class CreatePaymentInvoicesHandler : BaseHandler
    {
        private readonly static decimal defaultCoefficient = 1;
        private static readonly decimal defaultNds = 0.12m;
        private readonly static int defaultTariffCount = 1;
        private readonly static int defaultPenaltyPercent = 0;
        public async Task Execute(Owner.Type ownerType, int ownerId, string[] tariffCodes, string paymentStatusCode, int? userId, int? tariffCount = null)
        {
            foreach (var tariffCode in tariffCodes)
            {
                var tariffId = Executor.GetQuery<GetDictionaryIdByEntityNameAndCodeQuery>().Process(q => q.Execute(nameof(DicTariff), tariffCode));
                if (tariffId.HasValue == false)
                {
                    continue;
                }
                var paymentStatusId = Executor.GetQuery<GetDictionaryIdByEntityNameAndCodeQuery>().Process(q => q.Execute(nameof(DicPaymentStatus), paymentStatusCode));
                if (paymentStatusId.HasValue == false)
                {
                    continue;
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

                await Executor.GetCommand<CreatePaymentInvoiceCommand>().Process(c => c.ExecuteAsync(paymentInvoice));
            }
        }
    }
}
