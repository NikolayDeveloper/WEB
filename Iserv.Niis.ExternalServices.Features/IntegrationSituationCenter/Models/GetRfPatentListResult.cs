using System;

namespace Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Models
{
    [Serializable]
    [System.Xml.Serialization.XmlType(Namespace = "http://SituationCenter.niis.kz")]
    public class GetRfPatentListResult : SystemInfoMessage
    {
        public RfPatent[] List { get; set; }
    }




    [Serializable]
    [System.Xml.Serialization.XmlType(Namespace = "http://SituationCenter.niis.kz")]
    public class RfPatent
    {
        public int UID { get; set; }
        public int ParentPatentUID { get; set; }
        public int ChildPatentUID { get; set; }
    }

}
