using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.Model.Models.Other
{
    public class RouteStageOrderDto
    {
        public int Id { get; set; }
        public int CurrentStageId { get; set; }
        public int NextStageId { get; set; }
        public bool IsAutomatic { get; set; }
        public bool IsParallel { get; set; }
        public bool IsReturn { get; set; }
    }
}
