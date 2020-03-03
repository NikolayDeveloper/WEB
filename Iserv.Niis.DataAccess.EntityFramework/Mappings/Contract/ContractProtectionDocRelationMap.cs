using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Contract
{
    public class ContractProtectionDocRelationMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContractProtectionDocRelation>()
                .ToTable("Contract_ProtectionDoc")
                .HasKey(x => new {x.Id});
            modelBuilder.Entity<ContractProtectionDocRelation>()
                .HasOne(x => x.Contract)
                .WithMany(m => m.ProtectionDocs)
                .HasForeignKey(x => x.ContractId)
                .IsRequired();
            modelBuilder.Entity<ContractProtectionDocRelation>()
                .HasOne(x => x.ProtectionDoc)
                .WithMany(m => m.Contracts)
                .HasForeignKey(x => x.ProtectionDocId)
                .IsRequired();
        }
    }
}