using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Dictionaries
{
    public class DicDocumentTypeMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DicDocumentType>()
                .ToTable("DicDocumentTypes");
            modelBuilder.Entity<DicDocumentTypeDicProtectionDocTypeRelation>()
                .ToTable("DicDocumentTypesDicProtectionDocTypes")
                .HasKey(x => new { x.DicDocumentTypeId, x.DicProtectionDocTypeId });
            modelBuilder.Entity<DicDocumentTypeDicProtectionDocTypeRelation>()
                .HasOne(x => x.DicDocumentType)
                .WithMany(y => y.DicProtectionDocTypes)
                .HasForeignKey(y => y.DicDocumentTypeId)
                .IsRequired();
            modelBuilder.Entity<DicDocumentTypeDicProtectionDocTypeRelation>()
                .HasOne(x => x.DicProtectionDocType)
                .WithMany(y => y.DicDocumentTypes)
                .HasForeignKey(y => y.DicProtectionDocTypeId)
                .IsRequired();
        }
    }
}