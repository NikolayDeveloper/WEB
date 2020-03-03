using System;

namespace Iserv.Niis.Domain.Abstract
{
    public abstract class DictionaryEntity<TKey> : Entity<TKey>, ISoftDeletable, IDictionaryEntity<TKey> where TKey : IEquatable<TKey>
    {
        public string Code { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public string NameEn { get; set; }
        public string Description { get; set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(NameRu))
                return NameRu;
            if (!string.IsNullOrEmpty(NameKz))
                return NameKz;
            if (!string.IsNullOrEmpty(NameEn))
                return NameEn;
            if (!string.IsNullOrEmpty(Code))
                return Code;
            return Id + string.Empty;
        }
    }
}