using System;
using System.Collections.Generic;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;

namespace Iserv.Niis.Domain.Entities.Bulletin
{
    public class Bulletin: Entity<int>
    {
        public Bulletin()
        {
            ProtectionDocs = new HashSet<ProtectionDocBulletinRelation>();
        }
        public string Number { get; set; }
        public DateTimeOffset? PublishDate { get; set; }
        public string FilePath { get; set; }
        public ICollection<ProtectionDocBulletinRelation> ProtectionDocs { get; set; }
    }
}
