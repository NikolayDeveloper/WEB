using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace NIIS.DBConverter.Entities.References
{
    [Table("SS_USER")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class SSUser
    {
        [Column("U_ID")]
        public int Id { get; set; }

        [Column("date_create")]
        public DateTime? DateCreate { get; set; }

        [Column("stamp")]
        public DateTime? DateChange { get; set; }

        [Column("GROUP_ID")]
        public int? GroupId { get; set; }

        [Column("POSITION_ID")]
        public int? PositionId { get; set; }

        [Column("NAME_ML_EN")]
        public string NameEn { get; set; }

        [Column("NAME_ML_RU")]
        public string NameRu { get; set; }

        [Column("NAME_ML_KZ")]
        public string NameKz { get; set; }

        [Column("IP_ADDRESS")]
        public string IpAddress { get; set; }

        [Column("flIin")]
        public string IIN { get; set; }

        [Column("LOGIN")]
        public string Login { get; set; }

        [Column("PASSWORD_")]
        public string Password { get; set; }

        [Column("EMAIL")]
        public string Email { get; set; }

        [Column("IS_ARCHIVE")]
        public string IsArchive { get; set; }

        [Column("DEPARTMENT_ID")]
        public int? DepartmentId { get; set; }

        [Column("VIRTUAL")]
        public string Virtual { get; set; }

        [Column("flTemplateUserName")]
        public string TemplateUserName { get; set; }
    }
}
