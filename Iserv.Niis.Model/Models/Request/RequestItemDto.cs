using System;

namespace Iserv.Niis.Model.Models.Request
{
    public class RequestItemDto
    {
        public int Id { get; set; }
        public string IncomingNumber { get; set; }
        public string RequestNum { get; set; }
        public string ProtectionDocTypeName { get; set; }
        public string ProtectionDocTypeCode { get; set; }
        public DateTimeOffset? RequestDate { get; set; }
    }
}