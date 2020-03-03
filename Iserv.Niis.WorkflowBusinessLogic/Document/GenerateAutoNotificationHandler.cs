using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.WorkflowBusinessLogic.Requests;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.WorkflowBusinessLogic.Document
{
    public class GenerateAutoNotificationHandler: BaseHandler
    {
        public bool Execute(int requestId)
        {
            var rules = Executor.GetQuery<GetNotificationAutoGenerationRulesQuery>().Process(q => q.Execute());
            var request = Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.Execute(requestId));

            var documentCodes = rules.Where(r => r.StageId == request.CurrentWorkflow.CurrentStageId).Select(r => r.NotificationType.Code);

            foreach (var documentCode in documentCodes)
            {
                if (!string.IsNullOrEmpty(documentCode))
                {
                    var userInputDto = new UserInputDto
                    {
                        Code = documentCode,
                        Fields = new List<KeyValuePair<string, string>>(),
                        OwnerId = requestId,
                        OwnerType = Owner.Type.Request
                    };
                    Executor.GetHandler<CreateDocumentHandler>().Process(h =>
                        h.Execute(requestId, Owner.Type.Request, documentCode, DocumentType.Outgoing, userInputDto));
                }
            }

            return false;
        }
    }
}
