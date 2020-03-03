using System;
using System.ComponentModel.DataAnnotations;

namespace Iserv.Niis.Model.Models.Payment
{
    public class EditChargedPaymentInvoiceDto
    {
		[Required]
		public String EditDate { get; set; }
		[Required]
        public string EditReason { get; set; }
		public int OwnerType { get; set; }
	}
}