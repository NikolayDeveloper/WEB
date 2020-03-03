using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.Dictionaries.Location;

namespace Iserv.Niis.Domain.Entities.Document
{
    // TODO: Возможно лишняя таблица
    public class DocumentCustomer : Entity<int>, IHaveConcurrencyToken
    {
        public int CustomerRoleId { get; set; }
        public DicCustomerRole CustomerRole { get; set; }
        public int? CustomerId { get; set; }
        public DicCustomer Customer { get; set; }
        public int DocumentId { get; set; }
        public Document Document { get; set; }
        public int? AddressId { get; set; }
        public DicAddress Address { get; set; }
        public bool? IsMention { get; set; }
    }
}