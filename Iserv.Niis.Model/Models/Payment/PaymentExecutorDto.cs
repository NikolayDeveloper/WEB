using System.Collections.Generic;
using Iserv.Niis.Domain.Helpers;

namespace Iserv.Niis.Model.Models.Payment
{
    public class PaymentExecutorDto
    {
        public int OwnerId { get; set; }
        public Owner.Type OwnerType { get; set; }
        public int UserId { get; set; }
        public List<int> TariffIds { get; set; }
    }
}
