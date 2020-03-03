using System;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Helpers;

namespace Iserv.Niis.Domain.Entities.Notification
{
    public class NotificationTaskQueue : Entity<int>
    {
        #region Conditions



        public DateTimeOffset ResolveDate { get; set; }


        public int? ConditionStageId { get; set; }
        public DicRouteStage ConditionStage { get; set; }
        public int? DicCustomerId { get; set; }
        public DicCustomer DicCustomer { get; set; }


        public int? RequestId { get; set; }
        public Request.Request Request { get; set; }
        public int? ContractId { get; set; }
        public Contract.Contract Contract { get; set; }
        public int? DocumentId { get; set; }
        public Document.Document Document { get; set; }
        public int? ProtectionDocId { get; set; }
        public ProtectionDoc.ProtectionDoc ProtectionDoc { get; set; }

        #endregion

        public bool IsDocument => DocumentId.HasValue;
        public bool IsRequest => RequestId.HasValue;
        public bool IsContract => ContractId.HasValue;
        public bool IsProtectionDoс => ProtectionDocId.HasValue;
        
        public string DocumentEventKey => $"DocumentEventKey_{Id}";
        public string RequestEventKey => $"RequestEventKey_{Id}";
        public string ContractEventKey => $"ContractEventKey{Id}";
        public string ProtectionDocEventKey => $"ProtectionDocEventKey{Id}";

        public string WorkflowEventKey
        {
            get
            {
                if (IsRequest)
                {
                    return RequestEventKey;
                }

                if (IsContract)
                {
                    return ContractEventKey;
                }

                if (IsProtectionDoс)
                {
                    return ProtectionDocEventKey;
                }

                if (IsDocument)
                {
                    return DocumentEventKey;
                }

                return string.Empty;
            }
        }

        public bool? IsExecuted { get; set; }


        #region Result

        public string Subject { get; set; }
        public string Message { get; set; }
        public byte[] Attachment { get; set; }
        public bool IsSms { get; set; }

        #endregion
    }
}