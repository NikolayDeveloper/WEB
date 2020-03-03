using System.Threading.Tasks;
using Iserv.Niis.BusinessLogic.Common;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Documents.Numbers
{
    public class GenerateDocumentBarcodeHandler: BaseHandler
    {
        public async Task ExecuteAsync(int documentId)
        {
            var document = await Executor.GetQuery<GetDocumentByIdQuery>().Process(q => q.ExecuteAsync(documentId));
            Executor.GetHandler<GenerateBarcodeHandler>().Process(h => h.Execute(document));
            await Executor.GetCommand<UpdateDocumentCommand>().Process(c => c.Execute(document));
        }
    }
}
