using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Other
{
    public class RoleProtectionDocTypeRelationMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleProtectionDocTypeRelation>()
                .ToTable("Role_ProtectionDocType")
                .HasKey(x => new {x.RoleId, x.ProtectionDocTypeId});
            modelBuilder.Entity<RoleProtectionDocTypeRelation>()
                .HasOne(x => x.Role)
                .WithMany(m => m.ProtectionDocTypes)
                .HasForeignKey(x => x.RoleId)
                .IsRequired();
            modelBuilder.Entity<RoleProtectionDocTypeRelation>()
                .HasOne(x => x.ProtectionDocType);
        }
    }
}