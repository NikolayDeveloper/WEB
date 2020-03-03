using System;

namespace Iserv.Niis.Domain.Abstract
{
    public interface IEntity<TKey> where TKey : IEquatable<TKey>
    {
        TKey Id { get; set; }
        int? ExternalId { get; set; }
        DateTimeOffset DateCreate { get; set; }

        byte[] Timestamp { get;set;}
    }
}