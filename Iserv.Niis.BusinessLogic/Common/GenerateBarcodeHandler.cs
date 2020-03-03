using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.WorkflowBusinessLogic.SystemCounter;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Common
{
    public class GenerateBarcodeHandler: BaseHandler
    {
        private static readonly object LockObject = new object();

        public object Execute(IHaveBarcode haveBarcode)
        {
            lock (LockObject)
            {
                var count = Executor.GetHandler<GetNextCountHandler>().Process(h => h.Execute(NumberGenerator.Barcode));
                haveBarcode.Barcode = count;
            }

            return null;
        }
    }
}
