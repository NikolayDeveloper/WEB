using Microsoft.EntityFrameworkCore;
using NIIS.DBConverter.Entities.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.OldNiis.DataAccess.EntityFramework
{
    public class OldNiisFileContext : DbContext
    {
        public OldNiisFileContext(DbContextOptions options) : base(options) { }
        public virtual DbSet<DdDocumentData> DdDocumentDatas { get; set; }
    }
}
