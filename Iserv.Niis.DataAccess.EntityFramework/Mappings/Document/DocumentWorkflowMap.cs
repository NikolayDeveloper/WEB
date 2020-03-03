using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Document;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Document
{
    public class DocumentWorkflowMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DocumentWorkflow>()
                .ToTable("DocumentWorkflows");

            modelBuilder.Entity<DocumentWorkflow>()
                .HasOne(x => x.Owner)
                .WithMany(y => y.Workflows)
                .IsRequired();

            //modelBuilder.Entity<DocumentWorkflow>()
            //    .HasQueryFilter(d => d.IsCurent)
            //    .HasOne(x => x.Owner)
            //    .WithMany(y => y.CurrentWorkflows)
            //    .IsRequired();
        }
    }
}