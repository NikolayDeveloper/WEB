using System;

namespace Iserv.Niis.ExternalServices.Domain.Entities
{
    public class IntegrationLogAction
    {
        public int Id { get; set; }
        public int ActionTypeId { get; set; }
        public DateTimeOffset? ActionDate { get; set; }
        public int? SystemInfoQueryId { get; set; }
        public int? SystemInfoAnswerId { get; set; }
        public int? DigitalSignatureId { get; set; }
        public int? BinListId { get; set; }
        public int? RnnListId { get; set; }
        public DateTimeOffset? DateFrom { get; set; }
        public DateTimeOffset? DateTo { get; set; }
        public int? HistoryId { get; set; }
    }
}