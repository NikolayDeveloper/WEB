using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries.Location;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Dictionaries.Location
{
    public class DicContinentMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DicContinent>()
                .HasMany(c => c.Childs)
                .WithOne(p => p.Parent)
                .HasForeignKey(p => p.ParentId);
        }
    }
}