using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Features.Materials;
using Iserv.Niis.Features.Materials.Outgoing;
using Iserv.Niis.Model.Constans;
using Incoming = Iserv.Niis.Features.Materials.Incoming;
using Iserv.Niis.Model.Models.Material.Outgoing;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.Model.Models.Material.Incoming;
using Iserv.Niis.Model.Models.Material.Internal;
using Iserv.Niis.Portal.Infrastructure.Extensions;
using Iserv.Niis.Portal.Infrastructure.Extensions.Filter;
using Iserv.Niis.Portal.Infrastructure.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iserv.Niis.Portal.Controllers
{
    [Route("api/materials")]
    public class MaterialsController : Controller
    {
        private readonly IMediator _mediator;

        public MaterialsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("listByOwner/{ownerType}/{ownerId}")]
        public async Task<IActionResult> ByOwner(int ownerId, Owner.Type ownerType)
        {
            var result = await _mediator.Send(new Features.Materials.ListByOwner.Query(ownerId, ownerType));

            return Ok(result);
        }

        [HttpGet("incoming/{id}")]
        public async Task<IActionResult> Singleincomig(int id)
        {
            var result = await _mediator.Send(new Incoming.Single.Query(id));

            return Ok(result);
        }

        [HttpPut("incoming/{ownerType}/{id}")]
        public async Task<IActionResult> Put(int id, Owner.Type ownerType, [FromBody] MaterialIncomingDetailDto detailDto)
        {
            var result = await _mediator.Send(new Incoming.Update.Command(id, detailDto, ownerType));

            return Ok(result);
        }

        [HttpPost("incoming/{ownerType}")]
        public async Task<IActionResult> PostSingle(Owner.Type ownerType, [FromBody] MaterialIncomingDetailDto detail)
        {
            var result = await _mediator.Send(new Incoming.Create.Command(detail, User.Identity.GetUserId(), ownerType));

            return Ok(result);
        }

        [HttpGet("outgoing/{id}")]
        public async Task<IActionResult> GetOutgoing(int id)
        {
            var result = await _mediator.Send(new Features.Materials.Outgoing.Single.Query(id, User.Identity.GetUserId()));

            return Ok(result);
        }

        [HttpPost("outgoing/{ownerType}")]
        public async Task<IActionResult> PostOutgoing(Owner.Type ownerType, [FromBody] MaterialOutgoingDetailDto detail)
        {
            var result = await _mediator.Send(new Create.Command(detail, User.Identity.GetUserId(), ownerType));

            return Ok(result);
        }

        [HttpPut("outgoing/{ownerType}/{id}")]
        public async Task<IActionResult> PutOutgoing(int id, Owner.Type ownerType, [FromBody] MaterialOutgoingDetailDto detail)
        {
            var result = await _mediator.Send(new Update.Command(id, User.Identity.GetUserId(), detail, ownerType));

            return Ok(result);
        }

        [HttpGet("internal/{id}")]
        public async Task<IActionResult> GetInternal(int id)
        {
            var result = await _mediator.Send(new Features.Materials.Internal.Single.Query(id, User.Identity.GetUserId()));

            return Ok(result);
        }

        [HttpPost("internal/{ownerType}")]
        public async Task<IActionResult> PostInternal(Owner.Type ownerType, [FromBody] MaterialInternalDetailDto detail)
        {
            var result = await _mediator.Send(new Features.Materials.Internal.Create.Command(detail, User.Identity.GetUserId(), ownerType));

            return Ok(result);
        }

        [HttpPut("internal/{ownerType}/{id}")]
        public async Task<IActionResult> PutInternal(int id, Owner.Type ownerType, [FromBody] MaterialInternalDetailDto detail)
        {
            var result = await _mediator.Send(new Features.Materials.Internal.Update.Command(id, detail, ownerType));

            return Ok(result);
        }

        /// <summary>
        ///     Загрузка временных файлов в системную папку Temp
        /// </summary>
        /// <returns></returns>
        [Route("upload")]
        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            var response = new List<TempFileItemDto>();
            var files = Request.Form?.Files;

            if (files == null || !files.All(x =>
                    ContentType.Pdf.Equals(x.ContentType)
                    || ContentType.Docx.Equals(x.ContentType)
                    || ContentType.Doc.Equals(x.ContentType)
                    || ContentType.Jpg.Equals(x.ContentType)
                    || ContentType.Png.Equals(x.ContentType)
                ))
            {
                return BadRequest();
            }

            try
            {
                foreach (var file in files)
                {
                    var tempFilePath = Path.GetTempFileName();
                    if (file.Length <= 0)
                    {
                        continue;
                    }
                    using (var stream = new FileStream(tempFilePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    response.Add(new TempFileItemDto(file.FileName, Path.GetFileName(tempFilePath)));
                }
            }
            catch (Exception e)
            {
                foreach (var tempFileItemDto in response)
                {
                    System.IO.File.Delete(Path.Combine(Path.GetTempPath(), tempFileItemDto.TempName));
                }
                throw;
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new Delete.Command(id));

            return NoContent();
        }

        [HttpPost("workflows")]
        public async Task<IActionResult> Post([FromBody] MaterialWorkflowDto workflowDto)
        {
            var result = await _mediator.Send(new WorkflowCreate.Command(workflowDto, User.Identity.GetUserId()));

            return Ok(result);
        }

        [Route("replace")]
        [HttpPost]
        public async Task<IActionResult> Replace([FromBody] MaterialDetailDto data)
        {
            var result = await _mediator.Send(new ReplaceAttachment.Command(data, User.Identity.GetUserId()));

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var documents = await _mediator.Send(new List.Query());

            return documents
                .Filter(Request.Query)
                .Sort(Request.Query)
                .ToPagedList(Request.GetPaginationParams())
                .AsOkObjectResult(Response);
        }

        [HttpPost("sign")]
        public async Task<IActionResult> SignDocument([FromBody] DocumentUserSignatureDto documentUserSignatureDto)
        {
            var userId = User.Identity.GetUserId();
            documentUserSignatureDto.UserId = userId;
            var result =  await _mediator.Send(new SignDocument.Command(documentUserSignatureDto));
            return Ok(result);
        }

        [HttpGet("generateOutgoingNumber/{documentId}")]
        public async Task<IActionResult> GenerateOutgoingNumber(int documentId)
        {
            var result = await _mediator.Send(new GenerateOutgoingNumber.Command(documentId, User.Identity.GetUserId()));

            return Ok(new { result.OutgoingNumber, result.SendingDate });
        }
    }
}