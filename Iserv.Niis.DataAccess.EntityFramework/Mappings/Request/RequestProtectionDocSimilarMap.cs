using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Request
{
    internal class RequestProtectionDocSimilarMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RequestProtectionDocSimilar>()
                .ToTable("RequestProtectionDocSimilarities")
                .HasKey(x => new {x.ProtectionDocId, x.RequestId});
        }
    }
}