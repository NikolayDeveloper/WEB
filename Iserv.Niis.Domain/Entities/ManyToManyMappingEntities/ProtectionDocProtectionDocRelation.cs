using System;
using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.ManyToManyMappingEntities
{
    public class ProtectionDocProtectionDocRelation : Entity<int>, IHaveConcurrencyToken
    {
        public int ParentId { get; set; }
        public ProtectionDoc.ProtectionDoc Parent { get; set; }
        public int ChildId { get; set; }
        public ProtectionDoc.ProtectionDoc Child { get; set; }
    }
}