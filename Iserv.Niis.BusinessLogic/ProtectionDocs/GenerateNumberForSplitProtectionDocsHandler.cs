using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.WorkflowBusinessLogic.SystemCounter;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.ProtectionDocs
{
    public class GenerateNumberForSplitProtectionDocsHandler: BaseHandler
    {
        public async Task ExecuteAsync(ProtectionDoc oldProtectionDoc, ProtectionDoc newProtectionDoc)
        {
            if (newProtectionDoc == null || oldProtectionDoc == null)
            {
                throw new ArgumentNullException(nameof(newProtectionDoc));
            }

            var count = Executor.GetHandler<GetNextCountHandler>().Process(h => h.Execute(oldProtectionDoc.GosNumber));
            newProtectionDoc.GosNumber = $"{oldProtectionDoc.GosNumber}{Convert.ToChar(count + 64)}";
            // newProtectionDoc.GosDate = DateTimeOffset.Now;
        }
    }
}
