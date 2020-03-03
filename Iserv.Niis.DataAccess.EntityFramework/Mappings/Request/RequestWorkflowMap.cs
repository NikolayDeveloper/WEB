using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Request
{
    public class RequestWorkflowMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RequestWorkflow>()
                .ToTable("RequestWorkflows");
            modelBuilder.Entity<RequestWorkflow>()
                .HasMany(x => x.PaymentCharges)
                .WithOne(x => x.Workflow)
                .HasForeignKey(x => x.WorkflowId);
        }
    }
}