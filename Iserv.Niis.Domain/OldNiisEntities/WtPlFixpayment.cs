using System;

namespace Iserv.Niis.Domain.OldNiisEntities
{
    /// <summary>
    /// Наименование: Выставленные счета
    /// Примечание: Данный справочник являеться атрибутом документа - счет.
    /// Модуль: Оплаты 
    /// </summary>
    public class WtPlFixpayment
    {
        /// <summary>
        /// Наименование: ID
        /// Тип данных БД: int NOT NULL
        /// </summary>
        //[ColumnName("U_ID")]
        public int Id { get; set; }

        /// <summary>
        /// Наименование: Наименование услуг
        /// Тип данных БД: int NOT NULL
        /// </summary>
        //[ColumnName("TARIFF_ID")]
        public int TariffId { get; set; }

        /// <summary>
        /// Наименование: Коэффициент
        /// Тип данных БД: decimal NOT NULL
        /// </summary>
        //[ColumnName("FINE_PERCENT")]
        public decimal FinePercent { get; set; }

        /// <summary>
        /// Наименование: НДС (%)
        /// Тип данных БД: decimal NOT NULL
        /// </summary>
        //[ColumnName("VAT_PERCENT")]
        public decimal VatPercent { get; set; }

        /// <summary>
        /// Наименование: Заявка / ОД
        /// Тип данных БД: int NOT NULL
        /// </summary>
        //[ColumnName("APP_ID")]
        public int AppId { get; set; }

        /// <summary>
        /// Наименование: Дата выставления
        /// Тип данных БД: datetime NULL
        /// </summary>
        //[ColumnName("date_create")]
        public DateTime? DateCreate { get; set; }

        /// <summary>
        /// Наименование: Дата изменения
        /// Тип данных БД: datetime NULL
        /// </summary>
        //[ColumnName("stamp")]
        public DateTime? Stamp { get; set; }

        /// <summary>
        /// Наименование: Штраф (%)
        /// Тип данных БД: decimal NOT NULL
        /// </summary>
        //[ColumnName("PENI_PERCENT")]
        public decimal PeniPercent { get; set; }

        /// <summary>
        /// Наименование: Количество
        /// Тип данных БД: int NULL
        /// </summary>
        //[ColumnName("TARIFF_COUNT")]
        public int? TariffCount { get; set; }

        /// <summary>
        /// Наименование: Действие выполнено
        /// Тип данных БД: nvarchar(1) NULL
        /// </summary>
        //[ColumnName("IS_COMPLETE")]
        public string IsComplete { get; set; }

        /// <summary>
        /// Наименование: Дата просрочки
        /// Тип данных БД: date NULL
        /// </summary>
        //[ColumnName("DATE_LIMIT")]
        public DateTime? DateLimit { get; set; }

        /// <summary>
        /// Наименование: Фактическая дата
        /// Тип данных БД: date NULL
        /// </summary>
        //[ColumnName("DATE_FACT")]
        public DateTime? DateFact { get; set; }

        /// <summary>
        /// Наименование: Дата списания
        /// Тип данных БД: date NULL
        /// </summary>
        //[ColumnName("DATE_COMPLETE")]
        public DateTime? DateComplete { get; set; }

        /// <summary>
        /// Наименование: Пользователь
        /// Тип данных БД: int NULL
        /// </summary>
        //[ColumnName("flCreateUserId")]
        public int? FlCreateUserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? FixPayRemainder { get; set; }
    }
}