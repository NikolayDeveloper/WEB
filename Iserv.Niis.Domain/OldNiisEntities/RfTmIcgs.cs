using System;

namespace Iserv.Niis.Domain.OldNiisEntities
{
    /// <summary>
    /// Наименование: (511) МКТУ (Базовая)
    /// Примечание: Связка заявки с Международней Классификацией Товаров и Услуг.
    /// Модуль: Товарные Знаки 
    /// </summary>
    public class RfTmIcgs
    {
        /// <summary>
        /// Наименование: ID
        /// Тип данных БД: int NOT NULL
        /// </summary>
        //[ColumnName("U_ID")]
        public int Id { get; set; }

        /// <summary>
        /// Наименование: Заявка / ОД
        /// Тип данных БД: int NULL
        /// </summary>
        //[ColumnName("DOC_ID")]
        public int? DocId { get; set; }

        /// <summary>
        /// Наименование: (511) МКТУ
        /// Тип данных БД: int NULL
        /// </summary>
        //[ColumnName("ICPS_ID")]
        public int? IcpsId { get; set; }

        /// <summary>
        /// Наименование: Отказной класс
        /// Тип данных БД: nvarchar(1) NULL
        /// </summary>
        //[ColumnName("IS_NEGATIVE")]
        public string IsNegative { get; set; }

        /// <summary>
        /// Наименование: Описание
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("DSC")]
        public string Dsc { get; set; }

        /// <summary>
        /// Наименование: Описание  KZ
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("DSC_KZ")]
        public string DscKz { get; set; }

        /// <summary>
        /// Наименование: Документ
        /// Тип данных БД: int NULL
        /// </summary>
        //[ColumnName("DOCUMENT_ID")]
        public int? DocumentId { get; set; }

        /// <summary>
        /// Наименование: Дата занесения в базу данных
        /// Тип данных БД: datetime NULL
        /// </summary>
        //[ColumnName("date_create")]
        public DateTime? DateCreate { get; set; }

        /// <summary>
        /// Наименование: Метка обновления
        /// Тип данных БД: datetime NULL
        /// </summary>
        //[ColumnName("stamp")]
        public DateTime? Stamp { get; set; }

        /// <summary>
        /// Наименование: Частичный отказ
        /// Тип данных БД: nvarchar(1) NULL
        /// </summary>
        //[ColumnName("flIsNegativePartial")]
        public string FlIsNegativePartial { get; set; }

        /// <summary>
        /// Наименование: Заявленное описание
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("flDscStarted")]
        public string FlDscStarted { get; set; }

    }
}