using System;
using Iserv.Niis.BusinessLogic.Dictionaries.DicDivision;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Exceptions;
using Iserv.Niis.WorkflowBusinessLogic.SystemCounter;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Requests
{
    public class GenerateRequestIncomingNumberHandler : BaseHandler
    {
        public object Execute(Request request)
        {
            int count;
            var division = Executor.GetQuery<GetDicDivisionByIdQuery>()
                .Process(q => q.Execute(request.DivisionId ?? 0));
            if (division == null)
            {
                throw new DataNotFoundException(nameof(DicDivision), DataNotFoundException.OperationType.Read, request.DivisionId ?? 0);
            }
            if (division.Code == DicDivisionCodes.RGP_NIIS)
            {
                count = Executor.GetHandler<GetNextCountHandler>()
                    .Process(q => q.Execute(NumberGenerator.DocumentIncomingNumberCodePrefix + division.Code));
                request.IncomingNumber = $"{DateTime.Now.Year}-{count:D5}";
            }
            else
            {
                count = Executor.GetHandler<GetNextCountHandler>()
                    .Process(q => q.Execute(NumberGenerator.DocumentIncomingNumberFilialCodePrefix + division.Code));
                request.IncomingNumber = $"{count:D5}-{division.IncomingNumberCode}";
            }

            return null;
        }

        public object Execute(int requestId)
        {
            var request = Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.ExecuteAsync(requestId)).Result;
            return Execute(request);
        }
    }
}
