using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.Dictionaries
{
    /// <summary>
    /// Свзят старифа и типа объекта, один ко многим
    /// </summary>
    public class DicTariffProtectionDocType : DictionaryEntity<int>, IReference, IHaveConcurrencyToken
    {
        /// <summary>
        /// Тариф
        /// </summary>
        public int TariffId { get; set; }
        public DicTariff Tariff { get; set; }
    
        /// <summary>
        /// Тип объекта
        /// </summary>
        public int ProtectionDocTypeId { get; set; }
        public DicProtectionDocType ProtectionDocType { get; set; }
    }
}
