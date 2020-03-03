using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationContract.Models
{
    [Serializable]
    [XmlType(Namespace = "http://niis-efilling-service.kz")]
    public class ContractApplicationType
    {
        public int ContractApplicationId { get; set; }
        public string NameEn { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
    }
}
