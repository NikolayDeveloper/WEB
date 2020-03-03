
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace NIIS.DBConverter.Entities.References
{
    [Table("tbPatentAttorneys")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class tbPatentAttorneys
    {
        [Column("U_ID")]
        public int Id { get; set; }
        [Column("flUID")]
        public int? UId { get; set; }
        [Column("flIIN")]
        public string IIN { get; set; }
        [Column("flNameLast")]
        public string Lastname { get; set; }
        [Column("flNameFirst")]
        public string Firstname { get; set; }
        [Column("flNameMiddle")]
        public string Middlename { get; set; }
        [Column("flCertNum")]
        public string CertNum { get; set; }
        [Column("flCertDate")]
        public DateTime CertDate { get; set; }
        [Column("flActive")]
        public int Active { get; set; }
        [Column("flRevalidNote")]
        public string RevalidNote { get; set; }
        [Column("flOps")]
        public string Ops { get; set; }
        [Column("flKnowledgeArea")]
        public string KnowledgeArea { get; set; }
        [Column("flLanguage")]
        public string Language { get; set; }
        [Column("flJob")]
        public string Job { get; set; }
        [Column("flCountryId")]
        public int? CountryId { get; set; }
        [Column("flLocationId")]
        public int? LocationId { get; set; }
        [Column("flAddress")]
        public string Address { get; set; }
        [Column("flPhone")]
        public string Phone { get; set; }
        [Column("flFax")]
        public string Fax { get; set; }
        [Column("flEmail")]
        public string EMail { get; set; }
        [Column("flWebSite")]
        public string WebSite { get; set; }
        [Column("flNote")]
        public string Note { get; set; }
    }
}
