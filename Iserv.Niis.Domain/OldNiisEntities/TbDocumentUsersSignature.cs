using System;

namespace Iserv.Niis.Domain.OldNiisEntities
{
    /// <summary>
    /// Наименование: tbDocumentUsersSignature
    /// </summary>
    public class TbDocumentUsersSignature
    {
        /// <summary>
        /// Наименование: U_ID
        /// Тип данных БД: int NOT NULL
        /// </summary>
        //[ColumnName("U_ID")]
        public int Id { get; set; }

        /// <summary>
        /// Наименование: flDocUId
        /// Тип данных БД: int NOT NULL
        /// </summary>
        //[ColumnName("flDocUId")]
        public int FlDocUId { get; set; }

        /// <summary>
        /// Наименование: flUserId
        /// Тип данных БД: int NOT NULL
        /// </summary>
        //[ColumnName("flUserId")]
        public int FlUserId { get; set; }

        /// <summary>
        /// Наименование: flFingerPrint
        /// Тип данных БД: nvarchar(max) NOT NULL
        /// </summary>
        //[ColumnName("flFingerPrint")]
        public string FlFingerPrint { get; set; }

        /// <summary>
        /// Наименование: flSignedData
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("flSignedData")]
        public string FlSignedData { get; set; }

        /// <summary>
        /// Наименование: flSignerCertificate
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("flSignerCertificate")]
        public string FlSignerCertificate { get; set; }

        /// <summary>
        /// Наименование: flSignDate
        /// Тип данных БД: datetime NULL
        /// </summary>
        //[ColumnName("flSignDate")]
        public DateTime? FlSignDate { get; set; }
    }
}