using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Contract;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Contract
{
    public class ContractRequestICGSRequestRelationMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContractRequestICGSRequestRelation>()
                .ToTable("Contract_Request_ICGSRequest")
                .HasKey(x => new { x.Id });
            modelBuilder.Entity<ContractRequestICGSRequestRelation>()
                .HasOne(x => x.ContractRequestRelation)
                .WithMany(x => x.ContractRequestICGSRequests)
                .HasForeignKey(x => x.ContractRequestRelationId);
        }
    }
}