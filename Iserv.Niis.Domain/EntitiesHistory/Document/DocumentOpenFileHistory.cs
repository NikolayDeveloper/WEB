using System;

namespace Iserv.Niis.Domain.EntitiesHistory.Document
{
    /// <summary>
    /// Документы История(DD_DOCUMENT_HISTORY)
    /// </summary>
    public class DocumentOpenFileHistory /*: IHistoryEntity*/
    {
        //[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Display(Name = "Id истории")]
        //public int HistoryId { get; set; }
        //[Display(Name = "Дата изменения")]
        //public DateTimeOffset HistoryDate { get; set; }
        //[Display(Name = "Тип операции")]
        //public int HistoryType { get; set; }
        //[Display(Name = "Исполнитель")]
        //public int HistoryUserId { get; set; }
        //[Display(Name = "IP Адрес")]
        //public string HistoryIpAddress { get; set; }
        public int Id { get; set; }

        public DateTimeOffset DateView { get; set; }
        public int DocumentId { get; set; }
        public int UserId { get; set; }
    }
}