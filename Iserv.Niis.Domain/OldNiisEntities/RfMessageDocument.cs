using System;

namespace Iserv.Niis.Domain.OldNiisEntities
{
    /// <summary>
    /// Наименование: Связи Документов
    /// Примечание: Родительские и Дочернии отношения между документами.
    /// Модуль: Документооборот 
    /// </summary>
    public class RfMessageDocument
    {
        /// <summary>
        /// Наименование: ID
        /// Тип данных БД: int NOT NULL
        /// </summary>
        //[ColumnName("U_ID")]
        public int Id { get; set; }

        /// <summary>
        /// Наименование: Дата занесения в базу данных
        /// Тип данных БД: datetime NULL
        /// </summary>
        //[ColumnName("date_create")]
        public DateTime? DateCreate { get; set; }

        /// <summary>
        /// Наименование: Корреспондент
        /// Тип данных БД: int NULL
        /// </summary>
        //[ColumnName("DOCUMENT_ID")]
        public int? DocumentId { get; set; }

        /// <summary>
        /// Наименование: Респондент
        /// Тип данных БД: int NOT NULL
        /// </summary>
        //[ColumnName("REFDOCUMENT_ID")]
        public int RefdocumentId { get; set; }
    }
}