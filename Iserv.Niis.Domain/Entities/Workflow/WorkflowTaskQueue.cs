using System;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.Domain.Entities.Workflow
{
    public class WorkflowTaskQueue : Entity<int>, IHaveConcurrencyToken
    {
        public DateTimeOffset ResolveDate { get; set; }
        public int? RequestId { get; set; }
        public Request.Request Request { get; set; }
        public int? ContractId { get; set; }
        public Contract.Contract Contract { get; set; }
        public int? ProtectionDocId { get; set; }
        public ProtectionDoc.ProtectionDoc ProtectionDoc { get; set; }


        public int ConditionStageId { get; set; }
        public DicRouteStage ConditionStage { get; set; }
        public int? ResultStageId { get; set; }
        public DicRouteStage ResultStage { get; set; }


        public bool IsRequestEvent => RequestId.HasValue;
        public bool IsContractEvent => ContractId.HasValue;
        public bool IsProtectionDocEvent => ProtectionDocId.HasValue;

        public string RequestEventKey => $"RequestEventKey_{Id}";
        public string ContractEventKey => $"ContractEventKey{Id}";
        public string ProtectionDocEventKey => $"ProtectionDocEventKey{Id}";

        public string WorkflowEventKey
        {
            get
            {
                if (IsRequestEvent)
                {
                    return RequestEventKey;
                }

                if (IsContractEvent)
                {
                    return ContractEventKey;
                }

                if(IsProtectionDocEvent)
                {
                    return ProtectionDocEventKey;
                }

                return string.Empty;
            }
        }

        public bool? IsExecuted { get; set; }
    }
}