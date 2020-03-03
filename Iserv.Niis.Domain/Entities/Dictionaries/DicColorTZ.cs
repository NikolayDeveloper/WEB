// ReSharper disable InconsistentNaming

using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.Dictionaries
{
    /// <summary>
    /// ЦВЕТА И ЦВЕТОВЫЕ СОЧЕТАНИЯ (МКИЭТЗ)
    /// </summary>
    public class DicColorTZ : DictionaryEntity<int>, IReference, IHaveConcurrencyToken
    {
    }
}