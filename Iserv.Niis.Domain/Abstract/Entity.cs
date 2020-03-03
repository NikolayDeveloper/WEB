using NetCoreDataAccess.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace Iserv.Niis.Domain.Abstract
{
    public class Entity<TKey> : IEntity<TKey> where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; }
        public int? ExternalId { get; set; }
        public DateTimeOffset DateCreate { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
        public DateTimeOffset DateUpdate { get; set; }
    }
}