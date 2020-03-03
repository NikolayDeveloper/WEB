using Iserv.Niis.BusinessLogic.Documents;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Dictionaries.RuleHandRouteStages.RuleContract
{
    /// <summary>
    ///     Требования для возможности перехода с этапа 'Экспертиза по существу  (DK02.4)' на этап 'Контроль начальника
    ///     управления (DK02.5.1)'
    /// </summary>
    public class ExpertiseEssenceStageToSupervisionHeadDepartmentStageRule : BaseHandler
    {
        public bool Execute(Contract contract)
        {
            var isContactHasConclusionAboutRegistrationRefusalOfContractDocument = Executor.GetHandler<IsContractHasAnyDocumentWithCodesHandler>()
                .Process(q => q.Execute(contract.Id, new[]
                {
                    DicDocumentTypeCodes.ConclusionAboutRegistrationRefusalOfContract
                }));

            var isContactHasConclusionAboutRegistrationOfContractDocument = Executor.GetHandler<IsContractHasAnyDocumentWithCodesHandler>()
                .Process(q => q.Execute(contract.Id, new[]
                {
                    DicDocumentTypeCodes.ConclusionAboutRegistrationOfContract,
                }));
            if (isContactHasConclusionAboutRegistrationRefusalOfContractDocument)
            {
                return true;
            }

            if (isContactHasConclusionAboutRegistrationOfContractDocument &&
                string.IsNullOrEmpty(contract.GosNumber) == false)
            {
                return true;
            }
            return false;
        }
    }
}