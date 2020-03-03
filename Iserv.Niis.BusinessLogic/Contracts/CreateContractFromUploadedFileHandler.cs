using Iserv.Niis.BusinessLogic.Common;
using Iserv.Niis.BusinessLogic.Dictionaries.DicDepartment;
using Iserv.Niis.BusinessLogic.Dictionaries.DicDivision;
using Iserv.Niis.BusinessLogic.Dictionaries.DicReceiveTypes;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Model.Models;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Contracts
{
    public class CreateContractFromUploadedFileHandler : BaseHandler
    {
        public Contract Execute(IntellectualPropertyScannerDto dto)
        {
            var newContract = new Contract
            {
                ProtectionDocTypeId = dto.ProtectionDocTypeId,
                DepartmentId = Executor.GetQuery<GetDicDepartmentByCodeQuery>().Process(q => q.Execute(DicDepartmentCodes.ExpertiseDepartment)).Id,
                DivisionId = Executor.GetQuery<GetDicDivisionByCodeQuery>().Process(q => q.Execute(DicDivision.Niis)).Id,
                ReceiveTypeId = Executor.GetQuery<GetReceiveTypeByCodeQuery>()
                    .Process(q => q.Execute(DicReceiveType.Codes.Courier)).Id
            };

            Executor.GetHandler<GenerateBarcodeHandler>().Process(h => h.Execute(newContract));
            
            return newContract;
        }
    }
}