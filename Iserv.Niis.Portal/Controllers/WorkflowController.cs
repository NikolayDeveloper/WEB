using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Features.Workflow;
using Iserv.Niis.Model.Models.Request;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iserv.Niis.Portal.Controllers
{
    [Produces("application/json")]
    [Route("api/workflow")]
    public class WorkflowController : Controller
    {
        private readonly IMediator _mediator;

        public WorkflowController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{ownerId}/{ownerType}")]
        public async Task<IActionResult> GetByParent(int ownerId, Owner.Type ownerType)
        {
            var result = await _mediator.Send(new List.Query(ownerId, ownerType));

            return Ok(result);
        }
        
        // POST: api/Workflow
        [HttpPost("{ownerType}")]
        public async Task<IActionResult> Post([FromBody] WorkflowDto workflowDto, Owner.Type ownerType)
        {
            var result = await _mediator.Send(new Create.Command(workflowDto, ownerType));

            return Ok(result);
        }        

        [HttpGet("stageUsers/{stageId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStageUsers(GetStageUserOptions.Query query)
        {
            var stageUserOptions = await _mediator.Send(query);

            return Ok(stageUserOptions);
        }
    }
}