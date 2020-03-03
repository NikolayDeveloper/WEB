namespace Iserv.Niis.Domain.Enums
{
    public enum PaymentInvoiveChangeFlag
	{
		/// <summary>
		/// 0 – новая списанная оплата;
		/// </summary>
		NewChargedPaymentInvoice = 0,
		
		/// <summary>
		/// 1 –запись о списанной оплате удалена;
		/// </summary>
		ChargedPaymentInvoiveIsDeleted = 1,

		/// <summary>
		/// 2 – дата списанной оплаты удалена;
		/// </summary>
		PaymentInvoiceChargedDateIsDeleted = 2,

		/// <summary>
		/// 3- дата списанной оплаты изменена
		/// </summary>
		PaymentInvoiceChargedDateIsChanged = 3
    }
}