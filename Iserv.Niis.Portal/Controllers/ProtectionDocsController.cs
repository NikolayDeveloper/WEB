using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Model.Models.ProtectionDoc;
using Iserv.Niis.Portal.Infrastructure.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Single = Iserv.Niis.Features.ProtectionDoc.Single;
using Iserv.Niis.Features.ProtectionDoc;

namespace Iserv.Niis.Portal.Controllers
{
    [Route("api/ProtectionDocs")]
    public class ProtectionDocsController : Controller
    {
        private readonly IMediator _mediator;

        public ProtectionDocsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new Single.Query(id, User.Identity.GetUserId()));

            return Ok(result);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProtectionDocDetailsDto detailDto)
        {
            var result = await _mediator.Send(new Create.Command(detailDto, User.Identity.GetUserId()));

            return Ok(result);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProtectionDocDetailsDto detailDto)
        {
            var result = await _mediator.Send(new Update.Command(id, detailDto));

            return Ok(result);
        }

        [HttpPost("generateGosNumbers")]
        public async Task<IActionResult> GenerateGosNumbers([FromBody] int[] ids)
        {
            await _mediator.Send(new GenerateGosNumber.Command(ids));

            return NoContent();
        }

        [HttpPost("workflow/{userId}")]
        public async Task<IActionResult> WorkflowCreateMultiple([FromBody] int[] ids, int userId)
        {
            await _mediator.Send(new WorkflowCreate.Command(ids, userId));

            return NoContent();
        }
    }
}