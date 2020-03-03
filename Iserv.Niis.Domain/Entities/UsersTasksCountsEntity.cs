namespace Iserv.Niis.Domain.Entities
{
    public class UsersTasksCountsEntity
    {
        public int Id { get; set; }
        public int ActiveRequests { get; set; }
        public int ActiveProtectionDocs { get; set; }
        public int ActiveContracts { get; set; }
        public int CompletedRequests { get; set; }
        public int CompletedProtectionDocs { get; set; }
        public int CompletedContracts { get; set; }
        public int ExpiredRequests { get; set; }
        public int ExpiredProtectionDocs { get; set; }
        public int ExpiredContracts { get; set; }
        public int Documents { get; set; }
        public int CompletedDocuments { get; set; }
        public int ExpiredDocuments { get; set; }
    }
}