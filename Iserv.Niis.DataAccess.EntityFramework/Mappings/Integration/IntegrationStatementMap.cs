using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Integration;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Integration
{
    public class IntegrationStatementMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Statement>()
                .ToTable("Statements");
            modelBuilder.Entity<Statement>()
                .Property(x => x.Identifier)
                .IsRequired();
            modelBuilder.Entity<Statement>()
                .Property(x => x.Xin)
                .HasMaxLength(12);
        }
    }
}
