using System;

namespace Iserv.Niis.Domain.OldNiisEntities
{
    /// <summary>
    /// Наименование: (51) МКПО 
    /// Примечание: Связка Заявки/ОД с Международней Классификацией Промышленных Образцов.
    /// Модуль: Промышленные Образцы 
    /// </summary>
    public class RfIcis
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
        /// Наименование: Заявка /Патент 
        /// Тип данных БД: int NOT NULL
        /// </summary>
        //[ColumnName("PATENT_ID")]
        public int PatentId { get; set; }

        /// <summary>
        /// Наименование: (51) МКПО
        /// Тип данных БД: int NULL
        /// </summary>
        //[ColumnName("TYPE_ID")]
        public int? TypeId { get; set; }
    }
}