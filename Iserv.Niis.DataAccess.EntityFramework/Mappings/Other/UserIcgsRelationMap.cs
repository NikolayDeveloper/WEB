using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Other
{
    public class UserIcgsRelationMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserIcgsRelation>()
                .ToTable("AspNetUserICGSs")
                .HasKey(x => new { x.UserId, x.IcgsId});
            modelBuilder.Entity<UserIcgsRelation>()
                .HasOne(x => x.User)
                .WithMany(m => m.Icgss)
                .HasForeignKey(x => x.UserId)
                .IsRequired();
            modelBuilder.Entity<UserIcgsRelation>();
        }
    }
}