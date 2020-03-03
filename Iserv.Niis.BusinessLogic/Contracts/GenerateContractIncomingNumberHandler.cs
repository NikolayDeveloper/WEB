using Iserv.Niis.BusinessLogic.Dictionaries.DicDivision;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.WorkflowBusinessLogic.SystemCounter;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;
using System;
using Iserv.Niis.Exceptions;

namespace Iserv.Niis.BusinessLogic.Contracts
{
    public class GenerateContractIncomingNumberHandler : BaseHandler
    {
        public void Execute(Contract contract)
        {
            int count;
            var division = Executor.GetQuery<GetDicDivisionByIdQuery>()
                .Process(q => q.Execute(contract.DivisionId ?? 0));
            if (division == null)
            {
                throw new DataNotFoundException(nameof(DicDivision), DataNotFoundException.OperationType.Read, contract.DivisionId ?? 0);
            }
            if (division.Code == DicDivisionCodes.RGP_NIIS)
            {
                count = Executor.GetHandler<GetNextCountHandler>()
                    .Process(q => q.Execute(NumberGenerator.DocumentIncomingNumberCodePrefix + division.Code));
                contract.IncomingNumber = $"{DateTime.Now.Year}-{count:D5}";
            }
            else if (division.Code == DicDivisionCodes.Filial_ALM_RGP_NIIS)
            {
                count = Executor.GetHandler<GetNextCountHandler>()
                    .Process(q => q.Execute(NumberGenerator.DocumentIncomingNumberCodePrefix + division.Code));
                contract.IncomingNumber = $"{count:D5}-ALM";
            }
            else
            {
                count = Executor.GetHandler<GetNextCountHandler>()
                    .Process(q => q.Execute(NumberGenerator.DocumentIncomingNumberFilialCodePrefix + division.Code));
                contract.IncomingNumber = $"{count:D5}-{division.IncomingNumberCode}";
            }
        }
    }
}
