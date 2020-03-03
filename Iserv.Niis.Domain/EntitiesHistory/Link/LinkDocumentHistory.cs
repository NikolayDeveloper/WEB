using System;
using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.EntitiesHistory.Link
{
    /// <summary>
    /// Связи Документов История(RF_MESSAGE_DOCUMENT_HISTORY)
    /// </summary>
    public class LinkDocumentHistory : Entity<int>, IHistoryEntity
    {
        public int HistoryId { get; set; }
        public DateTimeOffset HistoryDate { get; set; }
        public int HistoryType { get; set; }
        public int? HistoryUserId { get; set; }
        public string HistoryIpAddress { get; set; }
        public int DocumentId { get; set; }
        public int DocumentChildId { get; set; }
        public bool? IsAnswer { get; set; }
    }
}