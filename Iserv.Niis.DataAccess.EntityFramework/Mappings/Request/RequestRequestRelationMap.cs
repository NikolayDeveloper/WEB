using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Request
{
    public class RequestRequestRelationMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RequestRequestRelation>()
                .ToTable("Request_Request")
                .HasKey(x => new {x.ChildId, x.ParentId});
            modelBuilder.Entity<RequestRequestRelation>()
                .HasOne(x => x.Parent)
                .WithMany(y => y.ChildsRequests)
                .HasForeignKey(y => y.ParentId)
                .IsRequired();
            modelBuilder.Entity<RequestRequestRelation>()
                .HasOne(x => x.Child)
                .WithMany(y => y.ParentRequests)
                .HasForeignKey(y => y.ChildId)
                .IsRequired();
        }
    }
}