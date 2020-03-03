using System;

namespace Iserv.Niis.ExternalServices.Domain.Entities
{
    public class IntegrationLogSystemInfo
    {
        public int Id { get; set; }
        public string MessageId { get; set; }
        public string ChainId { get; set; }
        public DateTimeOffset? MessageDate { get; set; }
        public string Sender { get; set; }
        public string StatusCode { get; set; }
        public string StatusMessageRu { get; set; }
        public string StatusMessageKz { get; set; }
        public string AdditionalInfo { get; set; }
    }
}