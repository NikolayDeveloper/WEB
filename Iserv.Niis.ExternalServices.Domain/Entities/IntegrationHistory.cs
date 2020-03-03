using System;

namespace Iserv.Niis.ExternalServices.Domain.Entities
{
    public class IntegrationHistory
    {
        public int Id { get; set; }
        public DateTimeOffset HistoryDate { get; set; }
        public int HistoryEntityType { get; set; }
        public int HistoryGRActionType { get; set; }
        public int? PatentId { get; set; }
        public int? PatentDocType { get; set; }
        public string PatentPublicNumber { get; set; }
        public DateTimeOffset? PatentPublicDate { get; set; }
        public DateTimeOffset? PatentSrokEndDate { get; set; }
        public string PatentName { get; set; }
        public int? PatentType { get; set; }
        public DateTimeOffset? PatentGRRegDate { get; set; }
        public int? PatentEndReason { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerRnn { get; set; }
        public string CustomerXin { get; set; }
        public string CustomerName { get; set; }
        public int? LinkId { get; set; }
        public int? LinkType { get; set; }
    }
}