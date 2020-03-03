using System;

namespace Iserv.Niis.Domain.Entities.Integration
{
    public class IntegrationDocument
    {
        public int Id { get; set; }
        public int RequestBarcode { get; set; }
        public int DocumentBarcode { get; set; }
        public int DocumentTypeId { get; set; }
        public string InOutNumber { get; set; }
        public DateTimeOffset? InOutDate { get; set; }
        public byte[] File { get; set; }
        public string FileName { get; set; }
        public string Note { get; set; }
        public DateTimeOffset DateCreate { get; set; }
        public DateTimeOffset? DateSent { get; set; }
        public override string ToString()
        {
            return $"{nameof(Id)} = {Id}";
        }

    }
}