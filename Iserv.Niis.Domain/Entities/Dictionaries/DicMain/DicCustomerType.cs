using System;
using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.Dictionaries.DicMain
{
    public class DicCustomerType : DictionaryEntity<int>, IReference, IHaveConcurrencyToken
    {
    }
}