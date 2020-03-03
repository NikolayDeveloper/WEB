using System;

namespace Iserv.Niis.Domain.OldNiisEntities
{
    /// <summary>
    /// Наименование: Связанные Заявки / ОД
    /// Примечание: Другие заявки связанные с заявками /ОД.
    /// Модуль: Учетные данные 
    /// </summary>
    public class WtPtEarlyreg
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
        /// Наименование: Заявка / ОД
        /// Тип данных БД: int NULL
        /// </summary>
        //[ColumnName("DOC_ID")]
        public int? DocId { get; set; }

        /// <summary>
        /// Наименование: Тип
        /// Тип данных БД: int NULL
        /// </summary>
        //[ColumnName("ETYPE_ID")]
        public int? EtypeId { get; set; }

        /// <summary>
        /// Наименование: Номер Заявки
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("REQ_NUMBER")]
        public string ReqNumber { get; set; }

        /// <summary>
        /// Наименование: Страна Подачи
        /// Тип данных БД: int NULL
        /// </summary>
        //[ColumnName("REQ_COUNTRY")]
        public int? ReqCountry { get; set; }

        /// <summary>
        /// Наименование: Дата Заявки
        /// Тип данных БД: date NULL
        /// </summary>
        //[ColumnName("REQ_DATE")]
        public DateTime? ReqDate { get; set; }

        /// <summary>
        /// Наименование: Наименование СД
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("SA_NAME")]
        public string SaName { get; set; }

        /// <summary>
        /// Наименование: Дополнительное Описание
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("DESCRIPTION")]
        public string Description { get; set; }
    }
}