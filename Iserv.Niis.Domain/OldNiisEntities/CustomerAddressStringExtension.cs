namespace Iserv.Niis.Domain.OldNiisEntities
{
    /// <summary>
    /// Класс расширения
    /// Наименование: Контрагенты
    /// Примечание: Структура хранения информации о клиентах
    /// Модуль: Контрагенты 
    /// </summary>
    //[Table("WT_CUSTOMER", "dbo")]
    public class CustomerAddressStringExtension : WtCustomer
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

        /// <summary>
        /// Наименование: Город
        /// Тип данных БД: nvarchar(max) NOT NULL
        /// </summary>
        //[ColumnName("City")]
        public string City { get; set; }

        /// <summary>
        /// Наименование: Область
        /// Тип данных БД: nvarchar(max) NOT NULL
        /// </summary>
        //[ColumnName("Oblast")]
        public string Oblast { get; set; }

        /// <summary>
        /// Наименование: Страна
        /// Тип данных БД: nvarchar(max) NOT NULL
        /// </summary>
        //[ColumnName("Republic")]
        public string Republic { get; set; }

        /// <summary>
        /// Наименование: Район
        /// Тип данных БД: nvarchar(max) NOT NULL
        /// </summary>
        //[ColumnName("Region")]
        public string Region { get; set; }
    }
}