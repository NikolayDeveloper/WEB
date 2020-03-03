using System.ComponentModel.DataAnnotations;

namespace Iserv.Niis.Model.Models.Payment
{
    public class DeletePaymentUseRequestDto
    {
        [Required]
        public string DeletionReason { get; set; }
    }
}