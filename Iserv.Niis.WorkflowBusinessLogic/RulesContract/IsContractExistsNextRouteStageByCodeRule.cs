using System.Linq;
using Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicRouteStage;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowContracts;
using NetCoreRules;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesContract
{
    public class IsContractExistsNextRouteStageByCodeRule : BaseRule<ContractWorkFlowRequest>
    {
        public bool Eval(string currentStageCode, string nextStageCode)
        {
            if (string.IsNullOrEmpty(nextStageCode) == true)
            {
                return false;
            }

            if (string.IsNullOrEmpty(currentStageCode) == true && string.IsNullOrEmpty(nextStageCode) == true)
            {
                return false;
            }

            if (string.IsNullOrEmpty(currentStageCode) == true && string.IsNullOrEmpty(nextStageCode) == false)
            {
                var isNextStageHasAny = Executor.GetQuery<GetNextDicRouteStageOrdersByCodeQuery>().Process(r => r.Execute(nextStageCode)).Any();

                return isNextStageHasAny;
            }

            return true;

            //var stages = Executor.GetQuery<GetNextDicRouteStageOrdersByCodeQuery>().Process(r => r.Execute(currentStageCode));

            //if (stages.Any(r => r.NextStage != null && r.NextStage.Code == nextStageCode))
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }
    }
}
