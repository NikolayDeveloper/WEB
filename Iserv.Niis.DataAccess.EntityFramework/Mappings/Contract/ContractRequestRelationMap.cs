using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Contract
{
    public class ContractRequestRelationMap: IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContractRequestRelation>()
                .ToTable("Contract_Request")
                .HasKey(x => new {x.Id});
            modelBuilder.Entity<ContractRequestRelation>()
                .HasOne(x => x.Contract)
                .WithMany(x => x.RequestsForProtectionDoc)
                .HasForeignKey(x => x.ContractId);
            modelBuilder.Entity<ContractRequestRelation>()
                .HasOne(x => x.Request)
                .WithMany(x => x.Contracts)
                .HasForeignKey(x => x.RequestId);
        }
    }
}