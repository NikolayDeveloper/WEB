using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Other
{
    public class UserIpcRelationMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserIpcRelation>()
                .ToTable("AspNetUserIpcs")
                .HasKey(x => new { x.UserId, x.IpcId });
            modelBuilder.Entity<UserIpcRelation>()
                .HasOne(x => x.User)
                .WithMany(x => x.Ipcs)
                .HasForeignKey(x => x.UserId)
                .IsRequired();
        }
    }
}
