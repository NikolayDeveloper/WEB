using System;
using System.Xml.Serialization;
using System.Web;

namespace Iserv.Niis.ExternalServices.Features.IntegrationContract.Models
{
    [Serializable]
    [XmlType(Namespace = "http://niis-efilling-service.kz")]
    public class File
    {
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = HttpUtility.HtmlDecode(value);
            }
        }

        public int? PageCount { get; set; }

        public int? CopyCount { get; set; }

        public byte[] Content { get; set; }
    }
}
