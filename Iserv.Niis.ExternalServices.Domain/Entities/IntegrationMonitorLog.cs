using System;

namespace Iserv.Niis.ExternalServices.Domain.Entities
{
    public class IntegrationMonitorLog
    {
        public int Id { get; set; }
        public DateTimeOffset DbDateTime { get; set; }
        public bool Error { get; set; }
        public string Note { get; set; }
    }
}