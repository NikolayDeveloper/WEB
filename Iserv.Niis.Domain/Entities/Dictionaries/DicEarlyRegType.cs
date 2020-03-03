using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.Dictionaries
{
    /// <summary>
    /// Типы Связанных Заявок
    /// </summary>
    public class DicEarlyRegType : DictionaryEntity<int>, IReference, IHaveConcurrencyToken
    {
        public int? ProtectionDocTypeId { get; set; }
        public DicProtectionDocType ProtectionDocType { get; set; }
    }
}