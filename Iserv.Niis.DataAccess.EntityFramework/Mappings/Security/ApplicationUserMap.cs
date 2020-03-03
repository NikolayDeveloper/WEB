using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Security;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Security
{
    public class ApplicationUserMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(x => x.DocumentAccessRoles);
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(x => x.DocumentAccessRoles);
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(x => x.Customer);
        }
    }
}