using System;

namespace Iserv.Niis.Domain.OldNiisEntities
{
    /// <summary>
    /// Наименование: Заявки и ОД
    /// Примечание: Заявки и Охранные Документы. Хранение всей необходимой информации о заявках и охранных документах.
    /// Модуль: Учетные данные 
    /// </summary>
    public class BtBasePatent
    {
        /// <summary>
        /// Наименование: Статус
        /// Тип данных БД: int NULL
        /// </summary>
        //[ColumnName("STATUS_ID")]
        public int? StatusId { get; set; }

        /// <summary>
        /// Наименование: Тип конвенции
        /// Тип данных БД: int NULL
        /// </summary>
        //[ColumnName("TYPEII_ID")]
        public int? TypeiiId { get; set; }

        /// <summary>
        /// Наименование: Семейство Селекционного Достижения
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("SELECTION_FAMILY")]
        public string SelectionFamily { get; set; }

        /// <summary>
        /// Наименование: Реферат
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("REF_57")]
        public string Ref57 { get; set; }

        /// <summary>
        /// Наименование: TRASLITERATION
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("TRASLITERATION")]
        public string Trasliteration { get; set; }

        /// <summary>
        /// Наименование: Номер бюллетеня
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("NBY")]
        public string Nby { get; set; }


        /// <summary>
        /// Наименование: (526) Дискламация
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("DISCLAM_RU")]
        public string DisclamRu { get; set; }
        
        /// <summary>
        /// Наименование: (526) Дискламация (kz)
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("DISCLAM_KZ")]
        public string DisclamKz { get; set; }

        /// <summary>
        /// Наименование: (85) Дата перевода междунар. заявки на нац.фазу
        /// Тип данных БД: date NULL
        /// </summary>
        //[ColumnName("DATE_85")]
        public DateTime? Date85 { get; set; }

        /// <summary>
        /// Наименование: Изображение
        /// Тип данных БД: image(2147483647) NULL
        /// </summary>
        //[ColumnName("IMAGE")]
        public byte[] Image { get; set; }

        /// <summary>
        /// Наименование: Мини копия изображения
        /// Тип данных БД: image(2147483647) NULL
        /// </summary>
        //[ColumnName("SYS_ImageSmall")]
        public byte[] SysImagesmall { get; set; }

        /// <summary>
        /// Наименование: Под Тип
        /// Тип данных БД: int NULL
        /// </summary>
        //[ColumnName("SUBTYPE_ID")]
        public int? SubtypeId { get; set; }

        /// <summary>
        /// Наименование: Дата бюллетеня
        /// Тип данных БД: date NULL
        /// </summary>
        //[ColumnName("DBY")]
        public DateTime? Dby { get; set; }

        /// <summary>
        /// Наименование: Дата публикации заявок
        /// Тип данных БД: date NULL
        /// </summary>
        //[ColumnName("PUBLICATION_DATE")]
        public DateTime? PublicationDate { get; set; }

        /// <summary>
        /// Наименование: (21) Регистрационный номер заявки
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("REQ_NUMBER_21")]
        public string ReqNumber21 { get; set; }
        
        /// <summary>
        /// Наименование: Дата ОД (11)
        /// Тип данных БД: date NULL
        /// </summary>
        //[ColumnName("GOS_DATE_11")]
        public DateTime? GosDate11 { get; set; }

        /// <summary>
        /// Наименование: (11) Номер охранного документа
        /// Тип данных БД: nvarchar(256) NULL
        /// </summary>
        //[ColumnName("GOS_NUMBER_11")]
        public string GosNumber11 { get; set; }
        
        /// <summary>
        /// Наименование: (22) Дата подачи заявки
        /// Тип данных БД: date NULL
        /// </summary>
        //[ColumnName("REQ_DATE_22")]
        public DateTime? ReqDate22 { get; set; }
        
        /// <summary>
        /// Наименование: Срок действия ОД
        /// Тип данных БД: date NULL
        /// </summary>
        //[ColumnName("STZ17")]
        public DateTime? Stz17 { get; set; }
    }
}