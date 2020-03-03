using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Iserv.Niis.Domain.Enums;

namespace Iserv.Niis.Domain.Entities.Integration
{
    public class IntegrationRomarinLog
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public string MessageTemplate { get; set; }

        [StringLength(128)]
        public string Level { get; set; }

        public DateTimeOffset TimeStamp { get; set; }

        public string Exception { get; set; }

        [Column(TypeName = "xml")]
        public string Properties { get; set; }

        [NotMapped]
        public string File
        {
            get
            {
                XmlNodeList xnList = GetProperty("File");
                if (xnList == null || xnList.Count == 0) return string.Empty;

                return xnList[0].InnerText;
            }
        }

        [NotMapped]
        public IntegrationLogType? Type
        {
            get
            {
                XmlNodeList xnList = GetProperty("Type");
                if (xnList == null || xnList.Count == 0 || !Enum.TryParse(xnList[0].InnerText, out IntegrationLogType t)) return null;

                return t;
            }
        }

        [NotMapped]
        public string Status
        {
            get
            {
                XmlNodeList xnList = GetProperty("Status");
                if (xnList == null || xnList.Count == 0) return string.Empty;

                return xnList[0].InnerText;
            }
        }

        [NotMapped]
        public string OldValue
        {
            get
            {
                XmlNodeList xnList = GetProperty("OldValue"); 
                if (xnList == null || xnList.Count == 0) return string.Empty;

                return xnList[0].InnerText;
            }
        }

        [NotMapped]
        public string NewValue
        {
            get
            {
                XmlNodeList xnList = GetProperty("NewValue");
                if (xnList == null || xnList.Count == 0) return string.Empty;

                return xnList[0].InnerText;
            }
        }

        public XmlNodeList GetProperty(string key)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(Properties);

            return xml.SelectNodes($"/properties/property[@key='{key}']");
        }
    }

    // структура Properties
    public class PropertiesStructure
    {
        public string File { get; set; }
        public IntegrationLogType Type { get; set; }
        public string Status { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}