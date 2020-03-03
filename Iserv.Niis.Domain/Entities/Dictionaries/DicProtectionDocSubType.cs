using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.Dictionaries
{
    /// <summary>
    /// Подтипы Патентов
    /// </summary>
    public class DicProtectionDocSubType : DictionaryEntity<int>, IReference, IHaveConcurrencyToken
    {
        public string S1 { get; set; }
        public string S2 { get; set; }
        public string S1Kz { get; set; }
        public string S2Kz { get; set; }

        #region Relationships

        public int TypeId { get; set; }
        public DicProtectionDocType Type { get; set; }

        #endregion
    }
}