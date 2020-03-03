using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.ProtectionDocs
{
    public class GenerateProtectionDocBarcodeHandler: BaseHandler
    {
        public ProtectionDoc Execute(ProtectionDoc protectionDoc)
        {
            //TODO! Сгенерировать штрихкод

            return protectionDoc;
        }
    }
}
