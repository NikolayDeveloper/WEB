using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.Domain.Entities.Integration
{
    public class IntegrationLogRefList
    {
        public int Id { get; set; }
        public string RefName { get; set; }
        public string SqLquery { get; set; }
        public bool IsEnabled { get; set; }
        public string Note { get; set; }
    }
}
