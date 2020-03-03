using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;

namespace Iserv.Niis.Domain.Entities.Dictionaries
{
    /// <summary>
    /// Тип контракторв
    /// </summary>
    public class DicContractType : DictionaryEntity<int>, IReference, IHaveConcurrencyToken
    {
        /// <summary>
        /// Роль стороны один
        /// </summary>
        public int? StageOneId { get; set; }
        public DicCustomerRole StageOne { get; set; }

        /// <summary>
        /// Роль стороны два
        /// </summary>
        public int? StageTwoId { get; set; }
        public DicCustomerRole StageTwo { get; set; }
    }
}
