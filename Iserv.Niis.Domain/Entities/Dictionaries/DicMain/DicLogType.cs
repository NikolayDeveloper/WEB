using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.Dictionaries.DicMain
{
    public class DicLogType : DictionaryEntity<int>, IReference, IHaveConcurrencyToken
    {
    }
}