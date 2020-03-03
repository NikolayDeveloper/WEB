using Iserv.Niis.BusinessLogic.Documents;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;
using System.Linq;
using Iserv.Niis.Common.Codes;

namespace Iserv.Niis.BusinessLogic.Dictionaries.RuleHandRouteStages.RuleContract
{
    public class ControlHeadStageToTransferPrepareStageRule : BaseHandler
    {
        public bool Execute(Contract contract)
        {
            var documents = Executor.GetQuery<GetDocumentsByContractIdAndTypeCodesQuery>()
                .Process(q => q.Execute(contract.Id, 
                    new[] { DicDocumentTypeCodes.ConclusionAboutRegistrationOfContract }));


            return documents.Any(d => d.CurrentWorkflows.Any(cwf => cwf.CurrentStage.Code == RouteStageCodes.DocumentOutgoing_03_1));
        }
    }
}
