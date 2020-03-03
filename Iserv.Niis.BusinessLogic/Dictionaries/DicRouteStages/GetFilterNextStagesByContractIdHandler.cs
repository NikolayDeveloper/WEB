using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.BusinessLogic.Contracts;
using Iserv.Niis.BusinessLogic.Dictionaries.RuleHandRouteStages.RuleContract;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Exceptions;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicRouteStages
{
    public class GetFilterNextStagesByContractIdHandler : BaseHandler
    {
        public async Task<List<DicRouteStage>> ExecuteAsync(int contractId)
        {
            var contract = await Executor.GetQuery<GetContractByIdQuery>().Process(q => q.ExecuteAsync(contractId));
            if (contract == null)
            {
                throw new DataNotFoundException(nameof(Contract), DataNotFoundException.OperationType.Read, contractId);
            }

            if (contract.CurrentWorkflow.CurrentStageId.HasValue == false)
            {
                throw new ArgumentNullException(nameof(contract.CurrentWorkflow.CurrentStageId));
            }

            var nextStages = await Executor.GetQuery<GetAllRouteStageByRouteQuery>()
                .Process(q => q.ExecuteAsync(contract.CurrentWorkflow.RouteId ?? default(int)));

            //var nextStages = await Executor.GetQuery<GetNotAutomaticNextStagesByCurrentStageIdQuery>()
            //    .Process(q => q.ExecuteAsync(contract.CurrentWorkflow.CurrentStageId.Value));

            //FilterNextStages(contract, nextStages);

            return nextStages;
        }

        private void FilterNextStages(Contract contract, List<DicRouteStage> nextStages)
        {
            switch (contract.CurrentWorkflow.CurrentStage.Code)
            {
                case RouteStageCodes.DK01_1:
                    var isPossiblyGotoHeadDepartmentStage = Executor
                        .GetHandler<CreateContractStageToControlHeadDepartmentStageRule>()
                        .Process(h => h.Execute(contract));
                    if (isPossiblyGotoHeadDepartmentStage == false)
                    {
                        nextStages.RemoveAll(s => s.Code == RouteStageCodes.DK01_2);
                    }
                    break;

                case RouteStageCodes.DK02_1:
                    var isPossiblyGotoPaymentAcceptanceStage = Executor
                        .GetHandler<FormationStatementsStageToPaymentAcceptanceStageRule>()
                        .Process(h => h.Execute(contract));
                    if (isPossiblyGotoPaymentAcceptanceStage == false)
                    {
                        nextStages.RemoveAll(s => s.Code == RouteStageCodes.DK02_1_1);
                    }
                    break;

                case RouteStageCodes.DK02_4:
                    var isPossiblyGotoSupervisionHeadDepartment = Executor
                        .GetHandler<ExpertiseEssenceStageToSupervisionHeadDepartmentStageRule>()
                        .Process(h => h.Execute(contract));
                    if (isPossiblyGotoSupervisionHeadDepartment == false)
                    {
                        nextStages.RemoveAll(s => s.Code == RouteStageCodes.DK02_5_1);
                    }
                    break;
                case RouteStageCodes.DK02_5_1:
                    var isPossiblyToTransfer = Executor.GetHandler<ControlHeadStageToTransferPrepareStageRule>().Process(h => h.Execute(contract));

                    if (!isPossiblyToTransfer)
                        nextStages.RemoveAll(s => s.Code == RouteStageCodes.DK02_7);

                    break;
            }
        }
    }
}