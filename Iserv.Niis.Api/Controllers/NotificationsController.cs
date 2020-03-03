using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.BusinessLogic.Documents;
using Iserv.Niis.BusinessLogic.DocumentUserInput;
using Iserv.Niis.BusinessLogic.Notifications;
using Iserv.Niis.DI;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.Notifications.Implementations;
using Microsoft.AspNetCore.Mvc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using Newtonsoft.Json;

namespace Iserv.Niis.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Notifications")]
    public class NotificationsController : Controller
    {
        private readonly IExecutor _executor;
        private readonly IDocumentGeneratorFactory _templateGeneratorFactory;

        public NotificationsController(IExecutor executor,
            IDocumentGeneratorFactory templateGeneratorFactory)
        {
            _executor = executor;
            _templateGeneratorFactory = templateGeneratorFactory;
        }

        [HttpGet("{ownerType}/{ownerId}")]
        public async Task<IActionResult> List(Owner.Type ownerType, int ownerId)
        {
            var result = await _executor.GetQuery<GetNotificationsQuery>().Process(q => q.ExecuteAsync(ownerType, ownerId));
            return Ok(result);
        }

        [HttpGet("send/{ownerType}/{ownerId}/{documentId}")]
        public async Task<IActionResult> Resend(Owner.Type ownerType, int ownerId, int documentId)
        {
            var document = await _executor.GetQuery<GetDocumentByIdQuery>().Process(q => q.ExecuteAsync(documentId));
            if (document != null)
                new NotificationTaskQueueRegister(_templateGeneratorFactory).AddNotificationsByOwnerType(documentId, Owner.Type.Material);

            var result = await _executor.GetQuery<GetNotificationsQuery>().Process(q => q.ExecuteAsync(ownerType, documentId));

            return Ok(result);
        }
    }
}
