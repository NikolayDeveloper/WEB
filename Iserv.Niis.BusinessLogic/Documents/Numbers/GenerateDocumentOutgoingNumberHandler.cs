using System;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.BusinessLogic.Contracts;
using Iserv.Niis.BusinessLogic.ProtectionDocs;
using Iserv.Niis.BusinessLogic.Requests;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.WorkflowBusinessLogic.SystemCounter;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Documents.Numbers
{
    public class GenerateDocumentOutgoingNumberHandler : BaseHandler
    {
        public async Task Execute(Document document)
        {
            if(!string.IsNullOrEmpty(document.OutgoingNumber))
                return;
            var request =
                (await Executor.GetQuery<GetRequestsByDocumentIdQuery>().Process(q => q.ExecuteAsync(document.Id)))
                .FirstOrDefault();
            var protectionDoc =
            (await Executor.GetQuery<GetProtectionDocsByDocumentIdQuery>()
                .Process(q => q.ExecuteAsync(document.Id))).FirstOrDefault();
            var contract =
                (await Executor.GetQuery<GetContractsByDocumentIdQuery>().Process(q => q.ExecuteAsync(document.Id)))
                .FirstOrDefault();

            if (protectionDoc != null)
            {
                document.OutgoingNumber = GenerateOutgoingNumberForMainFilial();
                await Executor.GetCommand<UpdateDocumentCommand>().Process(r => r.Execute(document)); 
                return;
            }

            if (contract != null)
            {
                document.OutgoingNumber = GenerateOutgoingNumberForMainFilial();
                await Executor.GetCommand<UpdateDocumentCommand>().Process(r => r.Execute(document)); 
                return;
            }

            if (request?.Division == null)
            {
                document.OutgoingNumber = GenerateOutgoingNumberForMainFilial();
                await Executor.GetCommand<UpdateDocumentCommand>().Process(r => r.Execute(document)); 
                return;
            }

            var divisionCode = request.Division.Code;

            var count = Executor.GetHandler<GetNextCountHandler>().Process(h => h.Execute(NumberGenerator.DocumentOutgoingNumberFilialCodePrefix + divisionCode));

            document.OutgoingNumber = divisionCode.Equals("000001")
                ? GenerateOutgoingNumberForMainFilial()
                : $"{count:D5}-{request.Division.IncomingNumberCode}";

            await Executor.GetCommand<UpdateDocumentCommand>().Process(r => r.Execute(document));            
        }

        private string GenerateOutgoingNumberForMainFilial()
        {
            var count = Executor.GetHandler<GetNextCountHandler>().Process(h => h.Execute(NumberGenerator.DocumentOutgoingNumberCodePrefix));
            return $"{DateTime.Now.Year}-{count:D5}";
        }
    }
}
