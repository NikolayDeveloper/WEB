namespace Iserv.Niis.Domain.OldNiisEntities
{
    /// <summary>
    /// Класс расширение для 
    /// </summary>
    /// <summary>
    /// Наименование: Документы
    /// Примечание: Справочная информация документов.
    /// Модуль: Документооборот 
    /// </summary>
    public class DdDocumentExtension : DdDocument
    {
        /// <summary>
        /// Наименование: Код
        /// Тип данных БД: nvarchar(max) NOT NULL
        /// </summary>
        //[ColumnName("WorkTypeCode")]
        public string WorkTypeCode { get; set; }
    }
}