using System;
using System.Threading.Tasks;
using Iserv.Niis.WorkflowBusinessLogic.SystemCounter;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Documents.Numbers
{
    public class GenerateRegisterDocumentNumberHandler: BaseHandler
    {
        public async Task ExecuteAsync(int documentId)
        {
            var document = await Executor.GetQuery<GetDocumentByIdQuery>().Process(q => q.ExecuteAsync(documentId));
            if (!string.IsNullOrEmpty(document.DocumentNum))
                return;

            var code = document.Type.Code + DateTime.Now.Year;
            var count = Executor.GetHandler<GetNextCountHandler>().Process(h => h.Execute(code));
            document.DocumentNum = count.ToString();

            await Executor.GetCommand<UpdateDocumentCommand>().Process(c => c.Execute(document));
        }
    }
}
