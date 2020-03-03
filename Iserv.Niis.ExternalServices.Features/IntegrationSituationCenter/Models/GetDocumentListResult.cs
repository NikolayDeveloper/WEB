using System;

namespace Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Models
{
    [Serializable]
    [System.Xml.Serialization.XmlType(Namespace = "http://SituationCenter.niis.kz")]
    public class GetDocumentListResult : SystemInfoMessage
    {
        public Document[] List { get; set; }
    }


    [Serializable]
    [System.Xml.Serialization.XmlType(Namespace = "http://SituationCenter.niis.kz")]
    public class Document
    {
        public int UID { get; set; }
        public int? PatentUID { get; set; }
        public string DocTypeName { get; set; }
        public int DocTypeID { get; set; }
        public string DocNum { get; set; }
        public DateTime? DocDate { get; set; }
        public string Status { get; set; }
        public string User { get; set; }
//        public int? RefID { get; set; }

        public DateTime? PlanProvDate { get; set; }
        public string ResultStatus { get; set; }
    }

    [Serializable]
    [System.Xml.Serialization.XmlType(Namespace = "http://SituationCenter.niis.kz")]
    public class GetDocumentListTestResult : SystemInfoMessage
    {
        public DocumentTest[] List { get; set; }
    }


    [Serializable]
    [System.Xml.Serialization.XmlType(Namespace = "http://SituationCenter.niis.kz")]
    public class DocumentTest: Document
    {
        public DocumentTest(){}

        public DocumentTest(Document doc) {
            UID = doc.UID;
            PatentUID = doc.PatentUID;
            DocTypeName = doc.DocTypeName;
            DocTypeID = doc.DocTypeID;
            DocNum = doc.DocNum;
            DocDate = doc.DocDate;
            Status = doc.Status;
            User = doc.User;
        }
    }

}
//http://192.168.43.8/SituationCenter/WebServiceIntelSituationCenter.asmx