using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Dictionaries
{
    public class DicDocumentTypeGroupMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DicDocumentTypeGroup>()
                .ToTable("DicDocumentTypeGroups");
            modelBuilder.Entity<DicDocumentTypeGroup>()
              .HasMany(c => c.DocumentTypes)
              .WithOne(c => c.DocumentTypeGroup);
        }
    }
}
