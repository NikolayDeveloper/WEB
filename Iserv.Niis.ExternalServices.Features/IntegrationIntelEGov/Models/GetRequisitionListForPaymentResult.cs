using System;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models
{
    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public class GetRequisitionListForPaymentResult : SystemInfoMessage
    {
        public RequisitionInfo[] RequisitionList { get; set; }
    }
}