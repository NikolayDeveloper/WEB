using System;

namespace Iserv.Niis.Domain.OldNiisEntities
{
    /// <summary>
    /// Наименование: Документы
    /// Примечание: Справочная информация документов.
    /// Модуль: Документооборот 
    /// </summary>
    public class DdDocument : BtBasePatent
    {
        /// <summary>
        /// Наименование: Штрих код
        /// Тип данных БД: int NOT NULL
        /// </summary>
        //[ColumnName("U_ID")]
        public int Id { get; set; }

        /// <summary>
        /// Наименование: Тип Документа
        /// Тип данных БД: int NOT NULL
        /// </summary>
        //[ColumnName("DOCTYPE_ID")]
        public int DoctypeId { get; set; }

        /// <summary>
        /// Наименование: Тип Документа
        /// Тип данных БД: int NOT NULL
        /// </summary>
        //[ColumnName("DOCTYPE_ID")]
        public string DoctypeCode { get; set; }

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

        /// <summary>
        /// Наименование: Адресат
        /// Тип данных БД: int NULL
        /// </summary>
        //[ColumnName("CUSTOMER_ID")]
        public int? CustomerId { get; set; }

        /// <summary>
        /// Наименование: Количество Копий
        /// Тип данных БД: int NULL
        /// </summary>
        //[ColumnName("COPY_COUNT")]
        public int? CopyCount { get; set; }

        /// <summary>
        /// Наименование: Количество Страниц
        /// Тип данных БД: int NULL
        /// </summary>
        //[ColumnName("PAGE_COUNT")]
        public int? PageCount { get; set; }

        /// <summary>
        /// Наименование: Служба
        /// Тип данных БД: int NULL
        /// </summary>
        //[ColumnName("DEPARTMENT_ID")]
        public int? DepartmentId { get; set; }

        /// <summary>
        /// Наименование: Регистрационный номер документа
        /// Тип данных БД: nvarchar(35) NULL
        /// </summary>
        //[ColumnName("DOCUM_NUM")]
        public string DocumNum { get; set; }

        /// <summary>
        /// Наименование: Дата Документа
        /// Тип данных БД: date NULL
        /// </summary>
        //[ColumnName("DOCUM_DATE")]
        public DateTime? DocumDate { get; set; }

        /// <summary>
        /// Наименование: Исходящий номер
        /// Тип данных БД: nvarchar(110) NULL
        /// </summary>
        //[ColumnName("OUTNUM")]
        public string Outnum { get; set; }

        /// <summary>
        /// Наименование: Входящий
        /// Тип данных БД: nvarchar(30) NULL
        /// </summary>
        //[ColumnName("INOUT_NUM")]
        public string InoutNum { get; set; }

        /// <summary>
        /// Наименование: Описание (RU)
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("DESCRIPTION_ML_RU")]
        public string DescriptionMlRu { get; set; }

        /// <summary>
        /// Наименование: Описание (EN)
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("DESCRIPTION_ML_EN")]
        public string DescriptionMlEn { get; set; }

        /// <summary>
        /// Наименование: Описание (KZ)
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("DESCRIPTION_ML_KZ")]
        public string DescriptionMlKz { get; set; }

        /// <summary>
        /// Наименование: Подразделение
        /// Тип данных БД: int NULL
        /// </summary>
        //[ColumnName("DIVISION_ID")]
        public int? DivisionId { get; set; }

        /// <summary>
        /// Наименование: flDivisionId
        /// Тип данных БД: int NULL
        /// </summary>
        //[ColumnName("flDivisionId")]
        public int? FlDivisionId { get; set; }
        
        /// <summary>
        /// Наименование: Пользователь
        /// Тип данных БД: int NULL
        /// </summary>
        //[ColumnName("USER_ID")]
        public int? UserId { get; set; }

        /// <summary>
        /// Наименование: Тип Доставки Документа
        /// Тип данных БД: int NOT NULL
        /// </summary>
        //[ColumnName("SENDTYPE")]
        public int SendType { get; set; }

        /// <summary>
        /// Наименование: Входящий номер (филиал)
        /// Тип данных БД: nvarchar(20) NULL
        /// </summary>
        //[ColumnName("INNUM_ADD")]
        public string InnumAdd { get; set; }

        /// <summary>
        /// Наименование: Обработка завершена
        /// Тип данных БД: nvarchar(1) NOT NULL
        /// </summary>
        //[ColumnName("IS_COMPLETE")]
        public string IsComplete { get; set; }
    }
}