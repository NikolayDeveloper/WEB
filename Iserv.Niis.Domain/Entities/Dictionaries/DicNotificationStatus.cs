using System.Collections.Generic;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.Request;

namespace Iserv.Niis.Domain.Entities.Dictionaries
{
    public class DicNotificationStatus : DictionaryEntity<int>
    { 
        public ICollection<DocumentNotificationStatus> Documents { get; set; }
        public ICollection<RequestNotificationStatus> Requests { get; set; }
        public ICollection<ContractNotificationStatus> Contracts { get; set; }

        public DicNotificationStatus()
        {
            Documents = new HashSet<DocumentNotificationStatus>();
            Requests = new HashSet<RequestNotificationStatus>();
            Contracts = new HashSet<ContractNotificationStatus>();
        }

        public static class Codes
        {
            public const string PhoneNotFound = "PNF";
            public const string SmsSend = "SS";
            public const string SmsSendFail = "SSF";
            public const string EmailNotFound = "ENF";
            public const string EmailSend = "ES";
            public const string EmailSendFail = "ESF";
            public const string EmailCorrespondenceNotFound = "ECNF";
            public const string SmsCorrespondenceNotFound = "SCNF";
        }
    }
}
