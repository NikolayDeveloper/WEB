using Iserv.Niis.Business.Implementations;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicDepartments;
using Iserv.Niis.WorkflowBusinessLogic.SystemCounter;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.WorkflowBusinessLogic.Contracts
{
    public class GenerateContractNumberHandler : BaseHandler
    {
        public void Execute(Contract contract)
        {
            if (string.IsNullOrEmpty(contract.ContractNum) == false)
            {
                return;
            }

            var count = Executor.GetHandler<GetNextCountHandler>()
                .Process(h => h.Execute(Business.Implementations.NumberGenerator.ContractCode));

            contract.ContractNum = $"{DateTime.Now.Year}-{count}";

            if (contract.DepartmentId.HasValue)
            {
                var department = Executor.GetQuery<GetDicDepartmentByIdQuery>()
                    .Process(q => q.Execute(contract.DepartmentId.Value));
                if (department.Code == DicDepartmentCodes.F_01)
                {
                    contract.ContractNum = $"{count}-ALM";
                }

                Executor.GetCommand<UpdateContractCommand>().Process(c => c.Execute(contract));
            }
        }
    }
}
