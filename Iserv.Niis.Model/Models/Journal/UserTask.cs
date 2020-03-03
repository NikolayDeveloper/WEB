using System;

namespace Iserv.Niis.Model.Models.Journal
{
    /// <summary>
    /// Представляет задачу пользователя для грида в дневнике
    /// </summary>
    public class UserTask
    {
        public int Id { get; set; }
        public DateTimeOffset DateCreate { get; set; }
        public string Template { get; set; }
        public string Stage { get; set; }
        public string State { get; set; }
        public DateTimeOffset DateReviewAll { get; set; }
        public DateTimeOffset DateReviewStage { get; set; }
        public bool IsRead { get; set; }
    }
}
