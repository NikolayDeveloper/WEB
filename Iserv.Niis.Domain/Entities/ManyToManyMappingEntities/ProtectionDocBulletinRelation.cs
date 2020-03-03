using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.ManyToManyMappingEntities
{
    public class ProtectionDocBulletinRelation: Entity<int>
    {
        public int ProtectionDocId { get; set; }
        public ProtectionDoc.ProtectionDoc ProtectionDoc { get; set; }
        public int BulletinId { get; set; }
        public Bulletin.Bulletin Bulletin { get; set; }
        //todo заглушка для основного бюллетеня для публикации
        public bool IsPublish { get; set; }
    }
}
