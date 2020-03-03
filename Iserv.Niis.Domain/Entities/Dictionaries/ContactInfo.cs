using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.Dictionaries
{
    public class ContactInfo : Entity<int>
    {
        public int TypeId { get; set; }
        public DicContactInfoType Type { get; set; }
        public string Info { get; set; }
    }

    public class DicContactInfoType : DictionaryEntity<int>
    {
        public static class Codes
        {
            public const string Email = "email";
            public const string MobilePhone = "mobilePhone";
            public const string Phone = "phone";
            public const string Fax = "fax";
        }
    }
}
