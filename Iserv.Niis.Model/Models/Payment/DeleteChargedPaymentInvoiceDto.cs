using System.ComponentModel.DataAnnotations;

namespace Iserv.Niis.Model.Models.Payment
{
    public class DeleteChargedPaymentInvoiceDto
    {
        [Required]
        public string DeletionReason { get; set; }

		public int OwnerType { get; set; }
	}
}