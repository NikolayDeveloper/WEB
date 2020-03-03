using System.ComponentModel.DataAnnotations;

namespace Iserv.Niis.Model.Models.Payment
{
    public class PaymentReturnAmountRequestDto
    {
        public bool ReturnFullAmount { get; set; }
        public decimal ReturnAmount { get; set; }

        [Required]
        public string ReturnReason { get; set; }
    }
}