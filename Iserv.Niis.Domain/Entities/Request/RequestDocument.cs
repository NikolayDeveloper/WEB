using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.Request
{
    public class RequestDocument : Entity<int>
    {
        public int RequestId { get; set; }
        public Request Request { get; set; }
        public int DocumentId { get; set; }
        public Document.Document Document { get; set; }  
    }
}
