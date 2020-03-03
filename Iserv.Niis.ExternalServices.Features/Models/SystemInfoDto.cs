using System;

namespace Iserv.Niis.ExternalServices.Features.Models
{
    public class SystemInfoDto
    {
        public string MessageId { get; set; }
        public string ChainId { get; set; }
        public DateTime? MessageDate { get; set; }
        public string Sender { get; set; }
        public StatusInfoDto Status { get; set; }
        public string AdditionalInfo { get; set; }
    }
}