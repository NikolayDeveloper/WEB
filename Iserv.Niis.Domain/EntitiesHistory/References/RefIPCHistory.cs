using System;
using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.EntitiesHistory.References
{
    /// <summary>
    /// МПК История(CL_IPC_HISTORY)
    /// </summary>
    public class RefIPCHistory : Entity<int>, IHistoryEntity
    {
        public int HistoryId { get; set; }
        public DateTimeOffset HistoryDate { get; set; }
        public int HistoryType { get; set; }
        public int? HistoryUserId { get; set; }
        public string HistoryIpAddress { get; set; }
        public string Code { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public string NameEn { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }
        public int RevisionNumber { get; set; }
    }
}