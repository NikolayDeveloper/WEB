using System.Linq;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Documents
{
    public class IsContractHasAllDocumentWithCodesHandler : BaseHandler
    {
        public bool Execute(int contractId, string[] typeCodes)
        {
            var existTypeCodes = Executor.GetQuery<GetDocumentsByContractIdAndTypeCodesQuery>()
                .Process(c => c.Execute(contractId, typeCodes))
                .Select(d => d.Type.Code)
                .ToList();

            return typeCodes.All(t => existTypeCodes.Contains(t));
        }
    }
}