using System;

namespace Iserv.Niis.Domain.OldNiisEntities
{
    /// <summary>
    /// Наименование: (531) Венская классификация (МКИЭТЗ(Базовая)
    /// Примечание: Связь заявки с данными из справочика Международней Классификацией Изобразительных Элементов Товарных Знаков
    /// Модуль: Товарные Знаки 
    /// </summary>
    public class RfTmIcfem
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
        /// Наименование: МКИЭТЗ
        /// Тип данных БД: int NULL
        /// </summary>
        //[ColumnName("LCFEM_ID")]
        public int? LcfemId { get; set; }

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
    }
}