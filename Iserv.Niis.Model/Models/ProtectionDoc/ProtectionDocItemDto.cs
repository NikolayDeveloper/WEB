using System;

namespace Iserv.Niis.Model.Models.ProtectionDoc
{
    public class ProtectionDocItemDto
    {
        public int Id { get; set; }
        public string ProtectionDocNum { get; set; }
        public string ProtectionDocTypeName { get; set; }
        public string ProtectionDocTypeCode { get; set; }
        public DateTimeOffset ProtectionDocDate { get; set; }
    }
}