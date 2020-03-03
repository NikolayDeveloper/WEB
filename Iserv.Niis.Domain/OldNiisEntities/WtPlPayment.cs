using System;

namespace Iserv.Niis.Domain.OldNiisEntities
{
    /// <summary>
    /// Наименование: Платежи
    /// Примечание: Поступившие оплаты. Загружаются из 1С.
    /// Модуль: Оплаты 
    /// </summary>
    public class WtPlPayment
    {
        /// <summary>
		/// Наименование: ID
		/// Тип данных БД: int NOT NULL
		/// </summary>
		//[ColumnName("U_ID")]
        public int Id { get; set; }

        /// <summary>
        /// Наименование: Дата Создания Записи
        /// Тип данных БД: datetime NULL
        /// </summary>
        //[ColumnName("date_create")]
        public DateTime? DateCreate { get; set; }

        /// <summary>
        /// Наименование: Контрагент
        /// Тип данных БД: int NULL
        /// </summary>
        //[ColumnName("CUSTOMER_ID")]
        public int? CustomerId { get; set; }

        /// <summary>
        /// Наименование: Номер платежа
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("PAYMENT_TYPE")]
        public string PaymentType { get; set; }

        /// <summary>
        /// Наименование: Сумма
        /// Тип данных БД: decimal NULL
        /// </summary>
        //[ColumnName("PAYMENT_AMOUNT")]
        public decimal? PaymentAmount { get; set; }

        /// <summary>
        /// Наименование: Дата платежа
        /// Тип данных БД: date NULL
        /// </summary>
        //[ColumnName("PAYMENT_DATE")]
        public DateTime? PaymentDate { get; set; }

        /// <summary>
        /// Наименование: Номер документа 1С
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("PAYMENT_NUMB")]
        public string PaymentNumb { get; set; }

        /// <summary>
        /// Наименование: Авансовый
        /// Тип данных БД: nvarchar(1) NULL
        /// </summary>
        //[ColumnName("IS_AVANS")]
        public string IsAvans { get; set; }

        /// <summary>
        /// Наименование: Назначение
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("DSC")]
        public string Dsc { get; set; }

        /// <summary>
        /// Наименование: Описание распределения
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("USE_DSC")]
        public string UseDsc { get; set; }

        /// <summary>
        /// Наименование: flValSum
        /// Тип данных БД: decimal NULL
        /// </summary>
        //[ColumnName("flValSum")]
        public decimal? FlValSum { get; set; }

        /// <summary>
        /// Наименование: flExchangeRate
        /// Тип данных БД: decimal NULL
        /// </summary>
        //[ColumnName("flExchangeRate")]
        public decimal? FlExchangeRate { get; set; }

        /// <summary>
        /// Наименование: flValType
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("flValType")]
        public string FlValType { get; set; }
    }
}