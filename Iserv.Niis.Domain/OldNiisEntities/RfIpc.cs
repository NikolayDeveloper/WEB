using System;

namespace Iserv.Niis.Domain.OldNiisEntities
{
    /// <summary>
    /// Наименование: (51) МПК
    /// Примечание: Связка Заявки/ОД с Международней Патентной Классификацией.
    /// Модуль: Изобретения 
    /// </summary>
    public class RfIpc
    {
        /// <summary>
        /// Наименование: ID
        /// Тип данных БД: int NOT NULL
        /// </summary>
        //[ColumnName("U_ID")]
        public int Id { get; set; }

        /// <summary>
        /// Наименование: Заявка / Патент
        /// Тип данных БД: int NOT NULL
        /// </summary>
        //[ColumnName("PATENT_ID")]
        public int PatentId { get; set; }

        /// <summary>
        /// Наименование: (51) МПК
        /// Тип данных БД: int NULL
        /// </summary>
        //[ColumnName("TYPE_ID")]
        public int? TypeId { get; set; }

        /// <summary>
        /// Наименование: Дата создания
        /// Тип данных БД: datetime NULL
        /// </summary>
        //[ColumnName("date_create")]
        public DateTime? DateCreate { get; set; }

        /// <summary>
        /// Наименование: Штамп
        /// Тип данных БД: datetime NULL
        /// </summary>
        //[ColumnName("stamp")]
        public DateTime? Stamp { get; set; }

        /// <summary>
        /// Наименование: Основной
        /// Тип данных БД: nvarchar(1) NULL
        /// </summary>
        //[ColumnName("flIsMain")]
        public string FlIsMain { get; set; }
    }
}