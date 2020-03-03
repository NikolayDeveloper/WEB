using System;

namespace Iserv.Niis.Domain.Intergrations.RequisitionSend
{
    public class DocInfo
    {
        /// <remarks/>
        public string DocumentID { get; set; }

        /// <remarks/>
        public string InNumber { get; set; }

        /// <remarks/>
        public DateTime DateNIIP { get; set; }

        /// <remarks/>
        public string RegNumber { get; set; }

        /// <remarks/>
        public DateTime ApplicationDate { get; set; }
    }
}