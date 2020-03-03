using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.Dictionaries
{
    /// <summary>
    /// Типы Рассмотрения
    /// </summary>
    public class DicConsiderationType : DictionaryEntity<int>, IReference, IHaveConcurrencyToken
    {
        public int? ProtectionDocTypeId { get; set; }
        public DicProtectionDocType ProtectionDocType { get; set; }
    }
}