using System;

namespace Iserv.Niis.Domain.Abstract
{
    public interface IHistoryEntity
    {
        int HistoryId { get; set; }
        DateTimeOffset HistoryDate { get; set; }
        int HistoryType { get; set; }
        int? HistoryUserId { get; set; }
        string HistoryIpAddress { get; set; }
    }
}