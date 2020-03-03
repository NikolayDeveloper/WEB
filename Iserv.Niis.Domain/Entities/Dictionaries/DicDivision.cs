using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.Dictionaries
{
    /// <summary>
    /// Филиалы
    /// </summary>
    public class DicDivision : DictionaryEntity<int>, IReference, IHaveConcurrencyToken
    {
        public bool IsMonitoring { get; set; }
        public string IncomingNumberCode { get; set; }

        public override string ToString()
        {
            return NameRu;
        }

        #region Public codes
        
        /// <summary>
        /// РГП "НИИС"
        /// </summary>
        public static string Niis = "000001";

        #endregion
    }
}