using System;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models
{
    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public class MessageSendArgument : SystemInfoMessage
    {
        public string OwnerDocumentUId { get; set; }

        public RefKey DocumentType { get; set; }
        public File File { get; set; }
        public string Note { get; set; }

        public RefKey CustomerType { get; set; }
        public string CustomerName { get; set; }
        public string Login { get; set; }
        public string Xin { get; set; }

        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }

        public File PaymentFile { get; set; }
        public string PaymentNote { get; set; }

        public EGovPay Pay { get; set; }
    }
}