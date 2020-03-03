using System.ComponentModel.DataAnnotations;

namespace Iserv.Niis.Model.Models.Payment
{
    public class EditPaymentUseRequestDto
    {
        public decimal Amount { get; set; }

        [Required]
        public string EditReason { get; set; }
        public bool MakeCredited { get; set; }
    }
}