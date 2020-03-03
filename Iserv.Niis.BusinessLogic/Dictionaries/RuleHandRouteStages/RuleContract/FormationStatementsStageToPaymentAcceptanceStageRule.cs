using Iserv.Niis.BusinessLogic.ContractCustomers;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Dictionaries.RuleHandRouteStages.RuleContract
{
    /// <summary>
    ///     Требования для возможности перехода с этапа 'Формирование данных заявления (DK02.1)' на этап 'Зачтение оплаты за экспертизу
    ///     договора (DK02.1.0)'
    /// </summary>
    public class FormationStatementsStageToPaymentAcceptanceStageRule : BaseHandler
    {
        public bool Execute(Contract contract)
        {
            var isContractCustomersHasAllCustomerRole = Executor
                .GetHandler<IsContractCustomersHasAllCustomerRoleWithCodesHandler>()
                .Process(h => h.Execute(contract.Id, new[]
                {
                    DicCustomerRole.Codes.Storona1,
                    DicCustomerRole.Codes.Storona2,
                    DicCustomerRole.Codes.Correspondence,
                    DicCustomerRole.Codes.Confidant,
                    DicCustomerRole.Codes.PatentAttorney
                }));
            if (isContractCustomersHasAllCustomerRole == false)
            {
                return false;
            }

            return true;
        }
    }
}