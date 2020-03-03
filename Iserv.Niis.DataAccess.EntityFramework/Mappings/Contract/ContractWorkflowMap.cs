using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Contract;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Contract
{
    public class ContractWorkflowMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContractWorkflow>()
                .ToTable("ContractWorkflows");

            modelBuilder.Entity<ContractWorkflow>()
                .HasOne(x => x.Owner)
                .WithMany(y => y.Workflows)
                .IsRequired();
        }
    }
}