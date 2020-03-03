using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.Model.Models.System
{
    public class TableManagementDto
    {
        public int userId { get; set; }
        public string tableName { get; set; }
        public string[] columns { get; set; }
    }
}
