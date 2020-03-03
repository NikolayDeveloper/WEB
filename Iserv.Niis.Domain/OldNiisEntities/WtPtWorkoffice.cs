using System;

namespace Iserv.Niis.Domain.OldNiisEntities
{
    /// <summary>
	/// Наименование: Документооборот
	/// Примечание: Контроль прохождения документа по этапам.
	/// Модуль: Документооборот 
	/// </summary>
    public class WtPtWorkoffice
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
        /// Наименование: Метка обновления
        /// Тип данных БД: datetime NULL
        /// </summary>
        //[ColumnName("stamp")]
        public DateTime? Stamp { get; set; }

        /// <summary>
        /// Наименование: Из Этапа
        /// Тип данных БД: int NULL
        /// </summary>
        //[ColumnName("FROM_STAGE_ID")]
        public int? FromStageId { get; set; }

        /// <summary>
        /// Наименование: От Пользователя
        /// Тип данных БД: int NULL
        /// </summary>
        //[ColumnName("FROM_USER_ID")]
        public int? FromUserId { get; set; }

        /// <summary>
        /// Наименование: Этап
        /// Тип данных БД: int NULL
        /// </summary>
        //[ColumnName("TO_STAGE_ID")]
        public int? ToStageId { get; set; }

        /// <summary>
        /// Наименование: Пользователь
        /// Тип данных БД: int NULL
        /// </summary>
        //[ColumnName("TO_USER_ID")]
        public int? ToUserId { get; set; }

        /// <summary>
        /// Наименование: Завершен
        /// Тип данных БД: nvarchar(1) NULL
        /// </summary>
        //[ColumnName("IS_COMPLETE")]
        public string IsComplete { get; set; }

        /// <summary>
        /// Наименование: Системный
        /// Тип данных БД: nvarchar(1) NULL
        /// </summary>
        //[ColumnName("IS_SYSTEM")]
        public string IsSystem { get; set; }

        /// <summary>
        /// Наименование: Описание
        /// Тип данных БД: nvarchar(max) NULL
        /// </summary>
        //[ColumnName("DESCRIPTION")]
        public string Description { get; set; }

        /// <summary>
        /// Наименование: Тип
        /// Тип данных БД: int NULL
        /// </summary>
        //[ColumnName("TYPE_ID")]
        public int? TypeId { get; set; }

        /// <summary>
        /// Наименование: Дата Контроля
        /// Тип данных БД: date NULL
        /// </summary>
        //[ColumnName("CONTROL_DATE")]
        public DateTime? ControlDate { get; set; }

        /// <summary>
        /// Наименование: Документ
        /// Тип данных БД: int NOT NULL
        /// </summary>
        //[ColumnName("DOCUMENT_ID")]
        public int DocumentId { get; set; }

        /// <summary>
        /// Наименование: Дата Получения
        /// Тип данных БД: datetime NULL
        /// </summary>
        //[ColumnName("date_reade")]
        public DateTime? DateReade { get; set; }
    }
}