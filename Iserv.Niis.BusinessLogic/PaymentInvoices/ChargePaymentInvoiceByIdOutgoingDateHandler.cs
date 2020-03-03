using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.BusinessLogic._1CIntegration;
using Iserv.Niis.BusinessLogic.Dictionaries.DicPaymentStatuses;
using Iserv.Niis.BusinessLogic.Security;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.PaymentInvoices
{
    public class ChargePaymentInvoiceByIdOutgoingDateHandler : BaseHandler
    {
		public async Task ExecuteAsync(Owner.Type ownerType, int paymentInvoiceId, DateTimeOffset date)
		{

			var paymentInvoice = await (Executor.GetQuery<GetPaymentInvoiceByIdQuery>()
						.Process(q => q.ExecuteAsync(paymentInvoiceId)));

			var chargedStatus = Executor.GetQuery<GetDicPaymentStatusByCodeQuery>()
				.Process(q => q.Execute(DicPaymentStatusCodes.Charged));
			
			var user = NiisAmbientContext.Current.User.Identity.UserId;

			paymentInvoice.Status = chargedStatus;
			paymentInvoice.StatusId = chargedStatus?.Id ?? 0;
			paymentInvoice.DateComplete = date;			
			paymentInvoice.WriteOffUserId = user;
			
			var isExportedTo1C = await Executor.GetHandler<ExportPaymentTo1CHandler>().Process(r => r.ExecuteAsync(ownerType, 
					paymentInvoice.Id, PaymentInvoiveChangeFlag.NewChargedPaymentInvoice, chargedStatus.Code, date));

			if (isExportedTo1C)
			{
				//Устанавливаем дату экспорта в 1С если экспорт произошол успешно.
				paymentInvoice.DateExportedTo1C = DateTimeOffset.UtcNow;
				
			}
			Executor.GetCommand<UpdatePaymentInvoiceCommand>().Process(c => c.Execute(paymentInvoice));

		}		
    }
}
