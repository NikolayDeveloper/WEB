using Iserv.Niis.BusinessLogic.PaymentInvoices;
using Iserv.Niis.BusinessLogic.Payments;
using Iserv.Niis.BusinessLogic.Security;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Model.Models.Payment;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.PaymentUses
{
    public class DeletePaymentUseCommand : BaseCommand
    {
        private readonly IExecutor _executor;

        public DeletePaymentUseCommand(IExecutor executor)
        {
            _executor = executor;
        }

        public async Task ExecuteAsync(int paymentUseId, DeletePaymentUseRequestDto requestDto)
        {
            var paymentUse = await _executor.GetQuery<GetPaymentUseByIdQuery>()
                .Process(q => q.ExecuteAsync(paymentUseId));

            if (paymentUse.PaymentInvoice.DateComplete != null)
            {
                throw new ValidationException("Cannot delete Charged PaymentUse.");
            }

            var user = _executor.GetQuery<GetUserByIdQuery>()
                .Process(q => q.Execute(NiisAmbientContext.Current.User.Identity.UserId));

            var deletionDate = DateTimeOffset.Now;

            paymentUse.IsDeleted = true;
            paymentUse.DeletedDate = deletionDate;
            paymentUse.DeletionClearedPaymentReason = requestDto.DeletionReason;
            paymentUse.DeletionClearedPaymentDate = deletionDate;
            paymentUse.DeletionClearedPaymentEmployeeName = $"{user.NameRu} {user.Position?.NameRu}";

            _executor.GetCommand<UpdatePaymentUseCommand>()
                .Process(c => c.Execute(paymentUse));

            var paymentInvoice = paymentUse.PaymentInvoice;

            paymentInvoice.Status = Uow.GetRepository<DicPaymentStatus>()
                .AsQueryable()
                .FirstOrDefault(q => q.Code == DicPaymentStatusCodes.Notpaid);
            paymentInvoice.DateFact = null;
            paymentInvoice.WhoBoundUserId = null;

            _executor.GetCommand<UpdatePaymentInvoiceCommand>()
                .Process(c => c.Execute(paymentInvoice));

            _executor.GetCommand<UpdatePaymentStatusCommand>()
                .Process(c => c.Execute(paymentUse.PaymentId.Value));
        }
    }
}