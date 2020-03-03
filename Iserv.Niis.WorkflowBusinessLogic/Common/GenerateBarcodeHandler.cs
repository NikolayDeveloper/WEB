using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.WorkflowBusinessLogic.SystemCounter;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.WorkflowBusinessLogic.Common
{
    public class GenerateBarcodeHandler: BaseHandler
    {
        public void Execute(IHaveBarcode haveBarcode)
        {
            var count = Executor.GetHandler<GetNextCountHandler>().Process(h => h.Execute(NumberGenerator.Barcode));
            haveBarcode.Barcode = count;
        }
    }
}
