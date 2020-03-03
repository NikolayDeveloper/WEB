using System;
using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.EntitiesHistory.Document
{
    /// <summary>
    /// Документы История(DD_DOCUMENT_HISTORY)
    /// </summary>
    public class DocumentHistory : Entity<int>, IHistoryEntity
    {
        public int HistoryId { get; set; }
        public DateTimeOffset HistoryDate { get; set; }
        public int HistoryType { get; set; }
        public int? HistoryUserId { get; set; }
        public string HistoryIpAddress { get; set; }
        public string Code { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public string NameEn { get; set; }
        public string Description { get; set; }
        public int DocumentTypeId { get; set; }
        public string IncomingNumber { get; set; }
        public string IncomingNumberFilial { get; set; }
        public int? ReceiveTypeId { get; set; }
        public int? PatentId { get; set; }
        public int? CustomerId { get; set; }
        public int? AddressId { get; set; }
        public int? UserId { get; set; }
        public int? DivisionId { get; set; }
        public int? FlDivisionId { get; set; }
        public bool? IsControl { get; set; }
        public bool? IsOutControl { get; set; }
        public string ControlText { get; set; }
        public DateTimeOffset? ControlDate { get; set; }
        public DateTimeOffset? ControlEndDate { get; set; }
        public string DocumentNum { get; set; }
        public DateTimeOffset? DocumentDate { get; set; }
        public DateTimeOffset? DocumentInterval { get; set; }
        public int? DepartmentId { get; set; }
        public int? CopyCount { get; set; }
        public int? PageCount { get; set; }
        public string AdditionalText { get; set; }
        public int? DocumentPropertyId { get; set; }
        public decimal? DocumentWeigth { get; set; }
        public string DocumentTicketNumber { get; set; }
        public int? SendTypeId { get; set; }
        public decimal? SendAmmount { get; set; }
        public string OutgoingNumber { get; set; }
        public decimal? EnvelopeCost { get; set; }
        public bool IsComplete { get; set; }
        public int? ExtensionYear { get; set; }
        public string CustomerOwner { get; set; }
        public string GosNumber { get; set; }
        public string NumberCertificate { get; set; }
        public DateTimeOffset? ExtensionDate { get; set; }
        public bool? IsScan { get; set; }
    }
}