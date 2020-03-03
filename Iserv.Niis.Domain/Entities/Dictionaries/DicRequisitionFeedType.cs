using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.Dictionaries
{
    /// <summary>
    /// Типы Подачи Заявки
    /// </summary>
    public class DicRequisitionFeedType : DictionaryEntity<int>, IReference, IHaveConcurrencyToken
    {
        public int? ProtectionDocTypeId { get; set; }
        public DicProtectionDocType ProtectionDocType { get; set; }
    }
}