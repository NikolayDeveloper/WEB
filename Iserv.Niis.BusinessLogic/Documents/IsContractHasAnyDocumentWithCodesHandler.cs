using System.Linq;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Documents
{
    public class IsContractHasAnyDocumentWithCodesHandler : BaseHandler
    {
        public bool Execute(int contractId, string[] typeCodes)
        {
            return Executor.GetQuery<GetDocumentsByContractIdAndTypeCodesQuery>()
                .Process(c => c.Execute(contractId, typeCodes)).Any();
        }
    }
}