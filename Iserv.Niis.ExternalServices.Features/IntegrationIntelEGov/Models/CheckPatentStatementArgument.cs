using System;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models
{
    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public class CheckPatentStatementArgument : SystemInfoMessage
    {
        public string Identifier { get; set; }

        /// <remarks />
        public string GosNumber { get; set; }
    }
}