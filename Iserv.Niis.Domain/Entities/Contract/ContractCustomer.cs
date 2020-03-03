using System;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;

namespace Iserv.Niis.Domain.Entities.Contract
{
    public class ContractCustomer : Entity<int>, IHaveConcurrencyToken
    {
        public int? CustomerRoleId { get; set; }
        public DicCustomerRole CustomerRole { get; set; }
        public int? CustomerId { get; set; }
        public DicCustomer Customer { get; set; }
        public int? ContractId { get; set; }
        public Contract Contract { get; set; }
        public DateTimeOffset? DateBegin { get; set; }
        public DateTimeOffset? DateEnd { get; set; }
        public string Phone { get; set; }
        public string MobilePhone { get; set; }
        public string PhoneFax { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string AddressKz { get; set; }
        public string AddressEn { get; set; }
    }
}