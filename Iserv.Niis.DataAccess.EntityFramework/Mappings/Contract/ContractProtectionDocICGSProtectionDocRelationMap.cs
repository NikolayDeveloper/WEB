using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Contract;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Contract
{
    public class ContractProtectionDocICGSProtectionDocRelationMap: IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContractProtectionDocICGSProtectionDocRelation>()
                .ToTable("Contract_ProtectionDoc_ICGSProtectionDoc")
                .HasKey(x => new { x.Id });
            modelBuilder.Entity<ContractProtectionDocICGSProtectionDocRelation>()
                .HasOne(x => x.ContractProtectionDocRelation)
                .WithMany(x => x.ContractProtectionDocICGSProtectionDocs)
                .HasForeignKey(x => x.ContractProtectionDocRelationId);
        }
    }
}