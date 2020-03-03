using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.BusinessLogic.Documents;
using Iserv.Niis.BusinessLogic.Requests;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Workflows.Requests
{
    public class SetRequestChangeScenarioFlagHandler: BaseHandler
    {
        public async Task ExecuteAsync(int documentId)
        {
            var document = await Executor.GetQuery<GetDocumentByIdQuery>().Process(q => q.ExecuteAsync(documentId));
            if (document.Type.Code == DicDocumentTypeCodes.PetitionForChanging)
            {
                foreach (var documentRequest in document.Requests)
                {
                    var request = await Executor.GetQuery<GetRequestByIdQuery>()
                        .Process(q => q.ExecuteAsync(documentRequest.RequestId));
                    request.IsOnChangeScenario = true;
                    await Executor.GetCommand<UpdateRequestCommand>().Process(c => c.ExecuteAsync(request));
                }
            }
        }
    }
}
