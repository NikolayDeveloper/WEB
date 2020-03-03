using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Contract;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Contract
{
    public class ContractDocumentMap: IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContractDocument>()
                .ToTable("ContractsDocuments");
            modelBuilder.Entity<ContractDocument>()
                .HasOne(x => x.Document)
                .WithMany(y => y.Contracts)
                .HasForeignKey(y => y.DocumentId)
                .IsRequired();
            modelBuilder.Entity<ContractDocument>()
                .HasOne(x => x.Contract)
                .WithMany(y => y.Documents)
                .HasForeignKey(y => y.ContractId)
                .IsRequired();
        }
    }
}
