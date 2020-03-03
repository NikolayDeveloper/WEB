namespace Iserv.Niis.Domain.OldNiisEntities
{
    /// <summary>
    /// Наименование: Заявка - Контрагенты
    /// Примечание: Связка физических и юридических лиц с Заявкой/ОД
    /// Модуль: Связи Контрагентов 
    /// </summary>
    public class RfCustomerAddressExtention : RfCustomer
    {
        /// <summary>
        /// Наименование: Адрес Ru
        /// Тип данных БД: nvarchar(max) NOT NULL
        /// </summary>
        //[ColumnName("AddresNameRu")]
        public string AddresNameRu { get; set; }

        /// <summary>
        /// Наименование: Адрес Kz
        /// Тип данных БД: nvarchar(max) NOT NULL
        /// </summary>
        //[ColumnName("AddresNameKz")]
        public string AddresNameKz { get; set; }

        /// <summary>
        /// Наименование: Адрес En
        /// Тип данных БД: nvarchar(max) NOT NULL
        /// </summary>
        //[ColumnName("AddresNameEn")]
        public string AddresNameEn { get; set; }
    }
}