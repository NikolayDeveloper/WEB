using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;

namespace Iserv.Niis.Domain.Entities.Document
{
    public class DocumentProperty : Entity<int>, IHaveConcurrencyToken
    {
        public decimal? DocumentWeigth { get; set; }
        public int? SendTypeId { get; set; }
        public DicSendType SendType { get; set; }
        public decimal? SendAmount { get; set; }
        public string DocumentTicketNumber { get; set; }
    }
}