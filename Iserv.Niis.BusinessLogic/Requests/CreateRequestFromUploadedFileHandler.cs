using Iserv.Niis.BusinessLogic.Common;
using Iserv.Niis.BusinessLogic.Dictionaries.DicReceiveTypes;
using Iserv.Niis.BusinessLogic.Security;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Model.Models;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Requests
{
    public class CreateRequestFromUploadedFileHandler : BaseHandler
    {
        public Request Execute(IntellectualPropertyScannerDto dto)
        {
            var newRequest = new Request{ ProtectionDocTypeId = dto.ProtectionDocTypeId };
            Executor.GetHandler<GenerateBarcodeHandler>().Process(h => h.Execute(newRequest));
            newRequest.ReceiveTypeId = Executor.GetQuery<GetReceiveTypeByCodeQuery>()
                .Process(q => q.Execute(DicReceiveType.Codes.Courier)).Id;
            var user = Executor.GetQuery<GetUserByIdQuery>()
                .Process(q => q.Execute(NiisAmbientContext.Current.User.Identity.UserId));
            newRequest.DepartmentId = user.DepartmentId;
            newRequest.DivisionId = user.Department.DivisionId;

            return newRequest;
        }
    }
}