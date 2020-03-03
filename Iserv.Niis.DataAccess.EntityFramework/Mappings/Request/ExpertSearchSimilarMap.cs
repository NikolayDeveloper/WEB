using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Request
{
	class ExpertSearchSimilarMap : IMapBuilder
	{
		public void Build(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ExpertSearchSimilar>()
				.ToTable("ExpertSearchSimilarities");

            modelBuilder.Entity<ExpertSearchSimilar>()
                .HasOne(x => x.SimilarRequest);

            modelBuilder.Entity<ExpertSearchSimilar>()
                .HasOne(x => x.SimilarProtectionDoc);
        }
	}
}
