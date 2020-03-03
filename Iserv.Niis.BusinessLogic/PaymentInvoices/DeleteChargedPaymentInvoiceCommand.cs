using Iserv.Niis.BusinessLogic._1CIntegration;
using Iserv.Niis.BusinessLogic.PaymentInvoices;
using Iserv.Niis.BusinessLogic.Security;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models.Payment;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
//using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.PaymentUses
{
    public class DeleteChargedPaymentInvoiceCommand : BaseCommand
    {
        private readonly IExecutor _executor;

        public DeleteChargedPaymentInvoiceCommand(IExecutor executor)
        {
            _executor = executor;
        }

		public async Task ExecuteAsync(int paymentInvoiceId, DeleteChargedPaymentInvoiceDto requestDto)
		{
			//Удалить Payment Invoice
			var paymentInvoiceItem = await _executor.GetHandler<GetPaymentInvoiceByIdQuery>().Process(h =>
					h.ExecuteAsync(paymentInvoiceId));

			var user = _executor.GetQuery<GetUserByIdQuery>()
				.Process(q => q.Execute(NiisAmbientContext.Current.User.Identity.UserId));

			var deletionDate = DateTimeOffset.Now;

			paymentInvoiceItem.IsDeleted = true;
			paymentInvoiceItem.DeletedDate = deletionDate;
			paymentInvoiceItem.ReasonOfDeletingChargedPaymentInvoice = requestDto.DeletionReason;
			paymentInvoiceItem.DateOfDeletingChargedPaymentInvoice = deletionDate;
			paymentInvoiceItem.EmployeeAndPositonWhoDeleteChargedPaymentInvoice = $"{user.NameRu} {user.Position?.NameRu}";

			paymentInvoiceItem.Status = Uow.GetRepository<DicPaymentStatus>()
				.AsQueryable()
				.FirstOrDefault(q => q.Code == DicPaymentStatusCodes.Notpaid);
			paymentInvoiceItem.DateComplete = null;
			paymentInvoiceItem.WriteOffUser = null;
			paymentInvoiceItem.WriteOffUserId = null;

			_executor.GetCommand<UpdatePaymentInvoiceCommand>()
				.Process(c => c.Execute(paymentInvoiceItem));

			//Удалить все Payment Use
			var paymentUseRequestDto = new DeletePaymentUseRequestDto();
			paymentUseRequestDto.DeletionReason = requestDto.DeletionReason;
			foreach (var pu in paymentInvoiceItem.PaymentUses)
			{
				await _executor.GetCommand<DeletePaymentUseCommand>()
				.Process(c => c.ExecuteAsync(pu.Id, paymentUseRequestDto));
			}

			var ownerType = (Owner.Type)Enum.ToObject(typeof(Owner.Type), requestDto.OwnerType);

			//експорт в 1С
			var isExportedTo1C = await _executor.GetHandler<ExportPaymentTo1CHandler>().Process(r => r.ExecuteAsync(ownerType,
					paymentInvoiceId, PaymentInvoiveChangeFlag.PaymentInvoiceChargedDateIsDeleted, DicPaymentStatusCodes.Charged));

		}
    }
}