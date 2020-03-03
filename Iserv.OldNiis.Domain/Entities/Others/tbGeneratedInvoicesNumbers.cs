using System;
using System.ComponentModel.DataAnnotations;

// ReSharper disable InconsistentNaming

namespace NIIS.DBConverter.Entities.Others
{
    public class tbGeneratedInvoicesNumbers
    {
        [Key]
        public int flId { get; set; }
        public DateTime? flCreateDate { get; set; }
        public int flUserId { get; set; }
        public int? flDepartmentId { get; set; }
        public string flDepartmentCode { get; set; }
        public int flYear { get; set; }
        public int flDocumentId { get; set; }
        public int flIndex { get; set; }
        public string flInvoiceNumber { get; set; }
    }
}
