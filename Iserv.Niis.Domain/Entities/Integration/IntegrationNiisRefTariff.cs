using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.Domain.Entities.Integration
{
    public class IntegrationNiisRefTariff
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Text { get; set; }
        public decimal? ValueFull { get; set; }
        public decimal? ValueJur { get; set; }
        public decimal? ValueBiz { get; set; }
        public decimal? ValueFiz { get; set; }
        public decimal? ValueFizBenefit { get; set; }
    }
}
