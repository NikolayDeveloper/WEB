using System;

namespace Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Models
{
    [Serializable]
    [System.Xml.Serialization.XmlType(Namespace = "http://SituationCenter.niis.kz")]
    public class GetBtBasePatentListResult : SystemInfoMessage
    {
        public BtBasePatent[] List { get; set; }
    }


    [Serializable]
    [System.Xml.Serialization.XmlType(Namespace = "http://SituationCenter.niis.kz")]
    public class BtBasePatent
    {
        public int UID { get; set; }
        public string TypeName { get; set; }
        public int TypeID { get; set; }
        public string ReqNumber { get; set; }
        public DateTime? ReqDate { get; set; }
        public string GosNumber { get; set; }
        public DateTime? PublishDate { get; set; }
        public string RefType { get; set; }
    }


}
