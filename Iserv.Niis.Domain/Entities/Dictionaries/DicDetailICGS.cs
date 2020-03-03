using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.Dictionaries
{
    public class DicDetailICGS : DictionaryEntity<int>, IHaveConcurrencyToken
    {
        public string NameFr { get; set; }
        public int IcgsId { get; set; }
        public DicICGS Icgs { get; set; }
    }
}