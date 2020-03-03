using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.Dictionaries
{
    /// <summary>
    /// Статусы платежа
    /// </summary>
    public class DicPaymentStatus : DictionaryEntity<int>, IReference, IHaveConcurrencyToken
    {
        
    }
}