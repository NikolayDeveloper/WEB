using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.Domain.Entities.Payment
{
    public class RequiredPayment: Entity<int>
    {
        public int StageId { get; set; }
        public DicRouteStage Stage { get; set; }
        public int TariffId { get; set; }
        public DicTariff Tariff { get; set; }
    }
}
