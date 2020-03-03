using System;

namespace Iserv.Niis.Domain.OldNiisEntities
{
    /// <summary>
    /// Наименование: Распределение Оплат
    /// Примечание: Покрытие выставленных оплат поступившими оплатами.
    /// Модуль: Оплаты 
    /// </summary>
    public class WtPlPaymentUse
    {
        /// <summary>
        /// Наименование: ID
        /// Тип данных БД: int NOT NULL
        /// </summary>
        //[ColumnName("U_ID")]
        public int Id { get; set; }

        /// <summary>
        /// Наименование: Оплаты
        /// Тип данных БД: int NULL
        /// </summary>
        //[ColumnName("PAYMENT_ID")]
        public int? PaymentId { get; set; }

        /// <summary>
        /// Наименование: Назначение
        /// Тип данных БД: int NULL
        /// </summary>
        //[ColumnName("FIX_ID")]
        public int? FixId { get; set; }

        /// <summary>
        /// Наименование: Сумма
        /// Тип данных БД: decimal NOT NULL
        /// </summary>
        //[ColumnName("AMOUNT")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Наименование: Дата распределения
        /// Тип данных БД: datetime NULL
        /// </summary>
        //[ColumnName("date_create")]
        public DateTime? DateCreate { get; set; }

        /// <summary>
        /// Наименование: Описание
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("DSC")]
        public string Dsc { get; set; }
    }
}