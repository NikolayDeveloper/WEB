using System.ComponentModel.DataAnnotations;

namespace Iserv.Niis.Model.Models.Payment
{
    public class PaymentBlockAmountRequestDto
    {
        public decimal BlockAmount { get; set; }

        [Required]
        public string BlockReason { get; set; }
    }
}