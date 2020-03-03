using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Model.Models.BibliographicData;
using Iserv.Niis.Model.Models.Subject;

namespace Iserv.Niis.Model.Models.ProtectionDoc
{
    public class ProtectionDocDetailsDto: BibliographicDataDto
    {
        public int? Barcode { get; set; }
        public string OutgoingNumber { get; set; }
        public DateTimeOffset? OutgoingDate { get; set; }
        public int? TypeId { get; set; }
        public int? SubTypeId { get; set; }
        public string TypeCode { get; set; }
        public SubjectDto Addressee { get; set; }
        public int? AddresseeId { get; set; }
        public int? PageCount { get; set; }
        public int? SendTypeId { get; set; }
        public DateTimeOffset? DateCreate { get; set; }
        public DateTimeOffset? PublicDate { get; set; }
        public bool? WasScanned { get; set; }
    }
}
