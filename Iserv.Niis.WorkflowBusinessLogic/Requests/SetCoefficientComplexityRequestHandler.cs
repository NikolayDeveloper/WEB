using Iserv.Niis.Common.Codes;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.WorkflowBusinessLogic.Requests
{
    public class SetCoefficientComplexityRequestHandler : BaseHandler
    {
        private static readonly double TimeSpentExecuteRequestStageI_03_2_3AstanaInHours = 45.115;
        private static readonly double TimeSpentExecuteRequestStageI_03_2_3AlmataInHours = 26.402;
        private static readonly double TimeSpentExecuteRequestStageI_03_2_3_0InHours = 18.713;
        private static readonly double TimeSpentExecuteRequestStageUM_03_1InHours = 7.580;

        private static readonly double PercentForIndependentItem = 0.8;

        public int Execute(int requestId)
        {

            var request = Executor.GetQuery<GetRequestByIdQuery>().Process(r => r.Execute(requestId));

            if (request == null)
            {
                return requestId;
            }

            var isRequestUpdated = false;
            if (request.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.I_03_2_3 && request.CurrentWorkflow.CurrentUser.Department.Division.Code == DicDivisionCodes.RGP_NIIS)
            {
                request.CoefficientComplexity = CalculateCoefficient(TimeSpentExecuteRequestStageI_03_2_3AstanaInHours, request.CountIndependentItems);
                isRequestUpdated = true;
            }
            if (request.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.I_03_2_3 && request.CurrentWorkflow.CurrentUser.Department.Division.Code == DicDivisionCodes.Filial_ALM_RGP_NIIS)
            {
                request.CoefficientComplexity = CalculateCoefficient(TimeSpentExecuteRequestStageI_03_2_3AlmataInHours, request.CountIndependentItems);
                isRequestUpdated = true;
            }
            if (request.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.I_03_2_3_0)
            {
                request.CoefficientComplexity = CalculateCoefficient(TimeSpentExecuteRequestStageI_03_2_3_0InHours, request.CountIndependentItems);
                isRequestUpdated = true;
            }
            if (request.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.UM_03_1)
            {
                request.CoefficientComplexity = CalculateCoefficient(TimeSpentExecuteRequestStageUM_03_1InHours, request.CountIndependentItems);
                isRequestUpdated = true;
            }

            if (isRequestUpdated)
            {
                Executor.GetCommand<UpdateRequestCommand>().Process(c => c.Execute(request));
            }
            return requestId;
        }

        private double CalculateCoefficient(double timeSpentExecuteRequest, int? countIndepentItems)
        {
            if (countIndepentItems.HasValue)
            {
                return timeSpentExecuteRequest + ((countIndepentItems.Value - 1) * (timeSpentExecuteRequest * PercentForIndependentItem));
            }
            else
            {
                return timeSpentExecuteRequest;
            }
        }
    }
}
