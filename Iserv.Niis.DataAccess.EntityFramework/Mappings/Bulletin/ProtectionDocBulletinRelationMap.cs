using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Bulletin
{
    public class ProtectionDocBulletinRelationMap: IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProtectionDocBulletinRelation>()
                .ToTable("ProtectionDoc_Bulletin")
                .HasKey(x => new { x.Id });
            modelBuilder.Entity<ProtectionDocBulletinRelation>()
                .HasOne(x => x.Bulletin)
                .WithMany(m => m.ProtectionDocs)
                .HasForeignKey(x => x.BulletinId)
                .IsRequired();
            modelBuilder.Entity<ProtectionDocBulletinRelation>()
                .HasOne(x => x.ProtectionDoc)
                .WithMany(m => m.Bulletins)
                .HasForeignKey(x => x.ProtectionDocId)
                .IsRequired();
        }
    }
}
