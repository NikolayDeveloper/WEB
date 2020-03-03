using System;
using Iserv.Niis.WorkflowBusinessLogic.SystemCounter;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.WorkflowBusinessLogic.Document
{
    public class GenerateRegisterDocumentNumberHandler: BaseHandler
    {
        public void ExecuteAsync(int documentId)
        {
            var document = Executor.GetQuery<GetDocumentByIdQuery>().Process(q => q.Execute(documentId));
            if (!string.IsNullOrEmpty(document.DocumentNum))
                return;

            var code = document.Type.Code + DateTime.Now.Year;
            var count = Executor.GetHandler<GetNextCountHandler>().Process(h => h.Execute(code));
            document.DocumentNum = count.ToString();

            Executor.GetCommand<UpdateDocumentCommand>().Process(c => c.Execute(document));
        }
    }
}
