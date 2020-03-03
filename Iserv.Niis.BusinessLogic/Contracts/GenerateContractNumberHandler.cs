using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.WorkflowBusinessLogic.SystemCounter;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Contracts
{
    public class GenerateContractNumberHandler : BaseHandler
    {
        public async Task ExecuteAsync(Contract contract)
        {
            if (string.IsNullOrEmpty(contract.ContractNum) == false)
            {
                return;
            }

            var count = Executor.GetHandler<GetNextCountHandler>()
                .Process(h => h.Execute(NumberGenerator.ContractCode));

            var objectTypeCode = "";
            if (contract.ProtectionDocs.Count > 0)
                objectTypeCode = contract.ProtectionDocs.First().ProtectionDoc.Type.Code + "-";

            var contractType = "";
            if (contract.TypeId != null)
                contractType = contract.Type.Code + "-";

            var contractCategory = "";
            if (contract.CategoryId != null)
                contractCategory = contract.Category?.Code;

            contract.ContractNum = $"{objectTypeCode}{DateTime.Now.Year}-{count}/{contractType}";
            
            Executor.GetCommand<UpdateContractCommand>().Process(c => c.Execute(contract));
        }
    }
}