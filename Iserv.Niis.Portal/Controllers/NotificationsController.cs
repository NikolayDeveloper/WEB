using System.Threading.Tasks;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Portal.Infrastructure.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Notifications = Iserv.Niis.Features.Notification;

namespace Iserv.Niis.Portal.Controllers
{
    [Route("api/notifications")]
    public class NotificationsController : Controller
    {
        private readonly IMediator _mediator;

        public NotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{ownerType}/{ownerId}/{documentId}")]
        public async Task<IActionResult> List(Owner.Type ownerType, int ownerId, int documentId)
        {
            var result = await _mediator.Send(new Notifications.List.Query(ownerType, ownerId, documentId));
            return Ok(result);
        }

        // TODO: Убрать второй запрос
        [HttpGet("send/{ownerType}/{ownerId}/{documentId}")]
        public async Task<IActionResult> Resend(Owner.Type ownerType, int ownerId, int documentId)
        {
            await _mediator.Send(new Notifications.Email.Query(ownerType, ownerId, documentId, User.Identity.GetUserId()));
            var result = await _mediator.Send(new Notifications.List.Query(ownerType, ownerId, documentId));
            return Ok(result);
        }
    }
}
