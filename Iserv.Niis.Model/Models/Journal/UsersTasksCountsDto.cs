namespace Iserv.Niis.Model.Models.Journal
{
    public class UsersTasksCountsDto
    {
        public int Id { get; set; }
        public int ActiveTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int ExpiredTasks { get; set; }
        public int Documents { get; set; }
        public int CompletedDocuments { get; set; }
        public int ExpiredDocuments { get; set; }
    }
}