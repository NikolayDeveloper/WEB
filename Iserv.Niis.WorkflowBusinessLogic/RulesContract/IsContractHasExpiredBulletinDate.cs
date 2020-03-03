using System;
using Iserv.Niis.WorkflowBusinessLogic.Contracts;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowContracts;
using NetCoreRules;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesContract
{
    public class IsContractHasExpiredBulletinDate : BaseRule<ContractWorkFlowRequest>
    {
        public bool Eval()
        {
            var contract = Executor.GetQuery<GetContractByIdQuery>()
                .Process(q => q.Execute(WorkflowRequest.ContractId));
            if (contract.BulletinDate.HasValue == false)
            {
                return false;
            }

            var isBulletinDateExpired = contract.BulletinDate.Value.AddDays(1) <= DateTimeOffset.Now;
            return isBulletinDateExpired;
        }
    }
}