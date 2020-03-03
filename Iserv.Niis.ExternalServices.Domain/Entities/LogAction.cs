using System;

namespace Iserv.Niis.ExternalServices.Domain.Entities
{
    public class LogAction
    {
        public int Id { get; set; }
        public DateTimeOffset? DbDateTime { get; set; }
        public int? SystemInfoQueryId { get; set; }
        public int? SystemInfoAnswerId { get; set; }
        public string Project { get; set; }
        public string Type { get; set; }
        public string Note { get; set; }
    }
}