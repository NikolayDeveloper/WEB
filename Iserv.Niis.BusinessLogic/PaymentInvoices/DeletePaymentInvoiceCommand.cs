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
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.PaymentUses
{
    public class DeletePaymentInvoiceCommand : BaseCommand
    {
        private readonly IExecutor _executor;

        public DeletePaymentInvoiceCommand(IExecutor executor)
        {
            _executor = executor;
        }

		public async Task<DeletePaymentInvoiceResponseDto> ExecuteAsync(int paymentInvoiceId, DeletePaymentInvoiceDto requestDto)
		{
			var response = new DeletePaymentInvoiceResponseDto();
			response.Success = false;
			response.Message = "Невозможно удалить выставленную оплату, т.к. имеются зачтённые суммы.";
			var systemUser = _executor.GetQuery<GetUserByXinQuery>().Process(q => q.Execute(UserConstants.SystemUserXin));

			var paymentInvoiceItem = await _executor.GetHandler<GetPaymentInvoiceByIdQuery>().Process(h =>
					h.ExecuteAsync(paymentInvoiceId));

			if (paymentInvoiceItem.CreateUserId != systemUser.Id)
			{
				if (!paymentInvoiceItem.PaymentUses.Any())
				{
					_executor.GetCommand<DeletePhisicallyPaymentInvoiceCommand>().Process(c => c.Execute(paymentInvoiceItem));
					response.Success = true;
					response.Message = "Платеж был удален физически.";
				}			
			}
			else
			{
				if (!paymentInvoiceItem.PaymentUses.Any())
				{
					paymentInvoiceItem.IsDeleted = true;
					paymentInvoiceItem.DeletedDate = DateTimeOffset.Now;
					_executor.GetCommand<UpdatePaymentInvoiceCommand>().Process(c => c.Execute(paymentInvoiceItem));
					response.Success = true;
					response.Message = "Платеж был удален.";
				}
			}
			return response;


		}
    }
}