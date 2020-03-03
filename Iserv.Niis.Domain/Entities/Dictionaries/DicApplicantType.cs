using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.Dictionaries
{
    /// <summary>
    /// Список Типов Заявителей
    /// </summary>
    public class DicApplicantType : DictionaryEntity<int>, IReference, IHaveConcurrencyToken
    {
    }
}