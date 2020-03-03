using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Dictionaries.DicMain
{
    public class DicConventionTypeMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DicConsiderationType>()
                .ToTable("DicConsiderationTypes");
        }
    }
}