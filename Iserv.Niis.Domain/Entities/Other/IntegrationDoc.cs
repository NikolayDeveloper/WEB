using System;

namespace Iserv.Niis.Domain.Entities.Other
{
    public class IntegrationDoc
    {
        public int DocId { get; set; }
        public int OwnerDocId { get; set; }
        public int DocUID { get; set; }
        public int DocumentType { get; set; }
        public string InOutNumber { get; set; }
        public DateTimeOffset? InOutDate { get; set; }
        public Type File { get; set; }
        public string FileName { get; set; }
        public string Note { get; set; }
        public DateTimeOffset DateCreate { get; set; }
        public DateTimeOffset? DateSent { get; set; }
    }
}