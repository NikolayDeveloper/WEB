using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.ProtectionDoc
{
    public class AdditionalDocMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdditionalDoc>()
                .ToTable("AdditionalDocs");
            modelBuilder.Entity<AdditionalDoc>()
                .HasOne(a => a.Request)
                .WithMany(a => a.AdditionalDocs);
            modelBuilder.Entity<AdditionalDoc>()
                .HasOne(a => a.OfficeOfOriginCountry);
        }
    }
}