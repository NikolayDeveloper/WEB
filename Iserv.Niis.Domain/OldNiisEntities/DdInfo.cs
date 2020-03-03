using System;

namespace Iserv.Niis.Domain.OldNiisEntities
{
    /// <summary>
    /// Наименование: Атрибуты документа
    /// Модуль: Документооборот 
    /// </summary>
    public class DdInfo
    {
        /// <summary>
        /// Наименование: ID
        /// Тип данных БД: int NOT NULL
        /// </summary>
        //[ColumnName("U_ID")]
        public int Id { get; set; }

        /// <summary>
        /// Наименование: Дата
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
        /// Наименование: Соблюдены требования п. 4 ст. 9 Закона
        /// Тип данных БД: nvarchar(1) NOT NULL
        /// </summary>
        //[ColumnName("FLAG_NINE")]
        public string FlagNine { get; set; }

        /// <summary>
        /// Наименование: поступления дополнительных материалов к более ранней заявке (п. 3 ст. 20 Закона)
        /// Тип данных БД: nvarchar(1) NOT NULL
        /// </summary>
        //[ColumnName("FLAG_TTH")]
        public string FlagTth { get; set; }

        /// <summary>
        /// Наименование: заявитель является работодателем и соблюдены условия п.2 ст. 10 Закона
        /// Тип данных БД: nvarchar(1) NOT NULL
        /// </summary>
        //[ColumnName("FLAG_TTW")]
        public string FlagTtw { get; set; }

        /// <summary>
        /// Наименование: переуступка права работодателем или его правопреемником
        /// Тип данных БД: nvarchar(1) NOT NULL
        /// </summary>
        //[ColumnName("FLAG_TPT")]
        public string FlagTpt { get; set; }

        /// <summary>
        /// Наименование: переуступка права автором или его правопреемником
        /// Тип данных БД: nvarchar(1) NOT NULL
        /// </summary>
        //[ColumnName("FLAG_TAT")]
        public string FlagTat { get; set; }

        /// <summary>
        /// Наименование: право наследования
        /// Тип данных БД: nvarchar(1) NOT NULL
        /// </summary>
        //[ColumnName("FLAG_TN")]
        public string FlagTn { get; set; }

        /// <summary>
        /// Наименование: Коллективный товарный знак
        /// Тип данных БД: nvarchar(1) NULL
        /// </summary>
        //[ColumnName("COL_TZ")]
        public string ColTz { get; set; }

        /// <summary>
        /// Наименование: Испрашивается выставочный приоритет
        /// Тип данных БД: nvarchar(1) NULL
        /// </summary>
        //[ColumnName("AWARD_TZ")]
        public string AwardTz { get; set; }

        /// <summary>
        /// Наименование: Товарный знак в стандартном шрифтовом ис-полнении
        /// Тип данных БД: nvarchar(1) NULL
        /// </summary>
        //[ColumnName("FONT_TZ")]
        public string FontTz { get; set; }

        /// <summary>
        /// Наименование: Товарный знак объемный
        /// Тип данных БД: nvarchar(1) NULL
        /// </summary>
        //[ColumnName("D3_TZ")]
        public string D3Tz { get; set; }

        /// <summary>
        /// Наименование: Товарный знак в цветовом исполнении
        /// Тип данных БД: nvarchar(1) NULL
        /// </summary>
        //[ColumnName("COLOR_TZ")]
        public string ColorTz { get; set; }

        /// <summary>
        /// Наименование: Настоящим подтверждаю что подача на регистрацию заявляемого обозначения не нарушает права интеллектуальной собственности других лиц
        /// Тип данных БД: nvarchar(1) NULL
        /// </summary>
        //[ColumnName("REG_TZ")]
        public string RegTz { get; set; }

        /// <summary>
        /// Наименование: Транслитерация
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("TM_TRANSLIT")]
        public string TmTranslit { get; set; }

        /// <summary>
        /// Наименование: Перевод
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("TM_TRANSLATE")]
        public string TmTranslate { get; set; }

        /// <summary>
        /// Наименование: ТЗ Приоритет
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("TM_PRIORITET")]
        public string TmPrioritet { get; set; }

        /// <summary>
        /// Наименование: Селекционный номер
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("SEL_NOMER")]
        public string SelNomer { get; set; }

        /// <summary>
        /// Наименование: сорт (порода)
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("SEL_ROOT")]
        public string SelRoot { get; set; }

        /// <summary>
        /// Наименование: Род, вид
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("SEL_FAMILY")]
        public string SelFamily { get; set; }

        /// <summary>
        /// Наименование: Вид товара или конкретный товар - поле 
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("PN_GOODS")]
        public string PnGoods { get; set; }

        /// <summary>
        /// Наименование: Описание особых свойств товара
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("PN_DSC")]
        public string PnDsc { get; set; }

        /// <summary>
        /// Наименование: Место происхождения (производства) товара (с указанием границ географического объекта) 
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("PN_PLACE")]
        public string PnPlace { get; set; }
        
        /// <summary>
        /// Наименование: flBreedCountry
        /// Тип данных БД: int NULL
        /// </summary>
        //[ColumnName("flBreedCountry")]
        public int? FlBreedCountry { get; set; }

        /// <summary>
        /// Наименование: flProductSpecialProp
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("flProductSpecialProp")]
        public string FlProductSpecialProp { get; set; }

        /// <summary>
        /// Наименование: flProductPalce
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("flProductPalce")]
        public string FlProductPalce { get; set; }
    }
}