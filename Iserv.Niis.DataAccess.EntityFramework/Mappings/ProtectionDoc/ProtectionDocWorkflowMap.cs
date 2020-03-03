using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.ProtectionDoc
{
    public class ProtectionDocWorkflowMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProtectionDocWorkflow>()
                .ToTable("ProtectionDocWorkflows");

            modelBuilder.Entity<ProtectionDocWorkflow>()
                .HasOne(x => x.Owner)
                .WithMany(y => y.Workflows)
                .IsRequired();
        }
    }
}