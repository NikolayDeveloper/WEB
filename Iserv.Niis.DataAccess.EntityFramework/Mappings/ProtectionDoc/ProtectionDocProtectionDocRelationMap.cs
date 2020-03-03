using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.ProtectionDoc
{
    public class ProtectionDocProtectionDocRelationMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProtectionDocProtectionDocRelation>()
                .ToTable("ProtectionDoc_ProtectionDoc");
            modelBuilder.Entity<ProtectionDocProtectionDocRelation>()
                .HasOne(relation => relation.Child)
                .WithMany(pd=>pd.Parents)
                .HasForeignKey(relation => relation.ChildId);
            modelBuilder.Entity<ProtectionDocProtectionDocRelation>()
                .HasOne(relation => relation.Parent)
                .WithMany(pd => pd.Childs)
                .HasForeignKey(relation => relation.ParentId);
        }
    }
}