using System;

namespace Iserv.Niis.Model.Models.Payment
{
	public class ChargePaymentInvoiceDto
	{
		public String ChargeDate { get; set; }
		public int PaymentInvoiceId { get; set; }
		public int OwnerType { get; set; }
	}
}