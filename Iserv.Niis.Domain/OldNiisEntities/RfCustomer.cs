using System;

namespace Iserv.Niis.Domain.OldNiisEntities
{
    /// <summary>
    /// Наименование: Заявка - Контрагенты
    /// Примечание: Связка физических и юридических лиц с Заявкой/ОД
    /// Модуль: Связи Контрагентов 
    /// </summary>
    public class RfCustomer
    {
        /// <summary>
        /// Наименование: ID
        /// Тип данных БД: int NOT NULL
        /// </summary>
        //[ColumnName("U_ID")]
        public int Id { get; set; }

        /// <summary>
        /// Наименование: Тип
        /// Тип данных БД: int NOT NULL
        /// </summary>
        //[ColumnName("C_TYPE")]
        public int CType { get; set; }

        /// <summary>
        /// Наименование: Заявка / ОД
        /// Тип данных БД: int NOT NULL
        /// </summary>
        //[ColumnName("DOC_ID")]
        public int DocId { get; set; }

        /// <summary>
        /// Наименование: Контрагент
        /// Тип данных БД: int NULL
        /// </summary>
        //[ColumnName("CUSTOMER_ID")]
        public int? CustomerId { get; set; }

        /// <summary>
        /// Наименование: Не Упоминание
        /// Тип данных БД: nvarchar(1) NULL
        /// </summary>
        //[ColumnName("MENTION")]
        public string Mention { get; set; }

        /// <summary>
        /// Наименование: Дата Начала Участия
        /// Тип данных БД: date NULL
        /// </summary>
        //[ColumnName("DATE_BEGIN")]
        public DateTime? DateBegin { get; set; }

        /// <summary>
        /// Наименование: Дата Завершения Участия
        /// Тип данных БД: date NULL
        /// </summary>
        //[ColumnName("DATE_END")]
        public DateTime? DateEnd { get; set; }

        /// <summary>
        /// Наименование: Адрес
        /// Тип данных БД: int NULL
        /// </summary>
        //[ColumnName("ADDRESS_ID")]
        public int? AddressId { get; set; }

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