using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace Iserv.Niis.Domain.Intergrations.RequisitionSend.RequisitionFiles
{
    public class RequisitionFile
    {
        /// <remarks/>
        public string Name { get; set; }

        /// <remarks/>
        public string Content { get; set; }

        /// <remarks/>
        public RequisitionFileShepFile ShepFile { get; set; }
    }
}