using System;
using System.Threading.Tasks;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Features;
using Iserv.Niis.Features.Customer;
using Iserv.Niis.Model.Models.Subject;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Iserv.Niis.Portal.Controllers
{
    [Route("api/Subject")]
    public class SubjectController : Controller
    {
        private readonly IMediator _mediator;

        public SubjectController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{ownerId}/{ownerType}")]
        public async Task<IActionResult> GetByParent(int ownerId, Owner.Type ownerType)
        {
            var result = await _mediator.Send(new List.Query(ownerId, ownerType));

            return Ok(result);
        }

        [HttpGet("byXinAndName/{query}")]
        public async Task<IActionResult> GetByXinAndName(string query)
        {
            var split = query.Split(';');

            var result = await _mediator.Send(new ListByXinAndName.Query(split[0], split[1], split[2], split[3]));

            return Ok(result);
        }

        // GET: api/Subject/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            throw new NotImplementedException();
        }
        
        // POST: api/Subject
        [HttpPost("{ownerType}")]
        public async Task<IActionResult> AttachCustomer([FromBody] SubjectDto subjectDto, Owner.Type ownerType)
        {
            var result = await _mediator.Send(new AttachToOwner.Command(subjectDto, ownerType));

            return Ok(result);
        }

        [HttpPost("create/{ownerType}")]
        public async Task<IActionResult> CreateCustomer([FromBody] SubjectDto subjectDto, Owner.Type ownerType)
        {
            var result = await _mediator.Send(new Create.Command(subjectDto));

            if (ownerType != Owner.Type.None)
            {
                result.OwnerId = subjectDto.OwnerId;
                result.Id = subjectDto.Id;
                result.RoleId = subjectDto.RoleId;
                result = await _mediator.Send(new AttachToOwner.Command(result, ownerType));
            }

            return Ok(result);
        }

        // PUT: api/Subject/5
        [HttpPut("{ownerType}/{id}")]
        public async Task<IActionResult> Put(int id, Owner.Type ownerType, [FromBody] SubjectDto subjectDto)
        {
            var result = await _mediator.Send(new Update.Command(id, subjectDto, ownerType));

            return Ok(result);
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{ownerType}/{id}")]
        public async Task<IActionResult> Delete(Owner.Type ownerType, int id)
        {
            await _mediator.Send(new Delete.Command(id, ownerType));

            return NoContent();
        }
    }
}
