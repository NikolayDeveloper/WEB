using System;

namespace Iserv.Niis.Domain.Abstract
{
    public interface IDictionaryEntity<TKey> : IHaveLocalizedNames, ISoftDeletable, IEntity<TKey> where TKey : IEquatable<TKey>
    {
        string Code { get; set; }
        string Description { get; set; }
    }
}