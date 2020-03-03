using System;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models
{
    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public class GetRequisitionListByMessageTypeResult : SystemInfoMessage
    {
        public RequisitionInfo[] RequisitionList { get; set; }
    }


    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public class RequisitionInfo
    {
        public int DocumentID { get; set; }
        public DateTime? DocumentDate { get; set; }
        public RefKey DocumentStatus { get; set; }
        public DateTime? StatusDate { get; set; }
    }
}