using Iserv.Niis.BusinessLogic.Bulletin;
using Iserv.Niis.Domain.Entities.Bulletin;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Infrastructure.Pagination;
using Iserv.Niis.Model.Models.Bulletin;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Iserv.Niis.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Bulletin")]
    public class BulletinController : BaseNiisApiController
    {
        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var bulletins = await Executor.GetQuery<GetBulletinsQuery>().Process(q => q.ExecuteAsync());
            var result = bulletins.FirstOrDefault(d => d.Id == id);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var bulletins = await Executor.GetQuery<GetBulletinsQuery>().Process(q => q.ExecuteAsync());
            var result = Mapper.Map<List<BulletinDto>>(bulletins);

            return Ok(result.OrderByDescending(b => b.Number));
        }

        [HttpGet("earliest")]
        public async Task<IActionResult> GetEarliestBulletin()
        {
            var bulletins = await Executor.GetQuery<GetBulletinsQuery>().Process(q => q.ExecuteAsync());
            var earliestBulletin = bulletins.OrderBy(b => b.PublishDate)
                .FirstOrDefault(b => b.PublishDate > DateTimeOffset.Now);

            if (earliestBulletin == null)
            {
                var latestBulletin = bulletins.OrderBy(b => b.PublishDate)
                    .LastOrDefault(b => b.PublishDate <= DateTimeOffset.Now);
                var newPublishDate = latestBulletin?.PublishDate?.AddDays(7) ?? DateTimeOffset.Now.AddDays(7);
                earliestBulletin = new Bulletin
                {
                    PublishDate = newPublishDate,
                };
                Executor.GetHandler<GenerateBulletinNumberHandler>()
                    .Process<string>(h => h.Execute(earliestBulletin));
                var newbulletinId = await Executor.GetCommand<CreateBulletinCommand>()
                    .Process(c => c.ExecuteAsync(earliestBulletin));
                earliestBulletin = await Executor.GetQuery<GetBulletinByIdQuery>()
                    .Process(q => q.ExecuteAsync(newbulletinId));
            }

            var result = Mapper.Map<BulletinDto>(earliestBulletin);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BulletinDto bulletinDto)
        {
            var bulletin = Mapper.Map<Bulletin>(bulletinDto);
            Executor.GetHandler<GenerateBulletinNumberHandler>()
                .Process<string>(h => h.Execute(bulletin));
            var bulletinId = await Executor.GetCommand<CreateBulletinCommand>().Process(c => c.ExecuteAsync(bulletin));

            return Ok(bulletinId);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] BulletinDto bulletinDto)
        {
            var bulletin = Mapper.Map<Bulletin>(bulletinDto);
            await Executor.GetCommand<UpdateBulletinCommand>().Process(c => c.ExecuteAsync(bulletin));

            return NoContent();
        }

        [HttpPost("attach")]
        public async Task<IActionResult> AttachProtectionDocToBulletin([FromBody] BulletinAttachmentDto bulletinAttachmentDto)
        {
            var relation = new ProtectionDocBulletinRelation
            {
                BulletinId = bulletinAttachmentDto.BulletinId,
                ProtectionDocId = bulletinAttachmentDto.ProtectionDocId
            };
            await Executor.GetCommand<CreateProtectionDocBulletinRelationCommand>()
                .Process(c => c.ExecuteAsync(relation));
            return NoContent();
        }

        #region Bulletin Sections

        [HttpGet("sections")]
        public IActionResult GetBulletinSections()
        {
            var sectionDtos = Executor.GetQuery<GetPagedBulletinSectionsQuery>()
                .Process(q => q.Execute(Request));

            return sectionDtos.AsOkObjectResult(Response);
        }

        [HttpPost("sections/create")]
        public async Task<ActionResult> CreateBulletinSection([FromBody] CreateBulletinSectionRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                throw new ValidationException("Model is not valid.");
            }

            await Executor.GetCommand<CreateBulletinSectionCommand>()
                .Process(c => c.Execute(requestDto));

            return Ok();
        }

        [HttpPost("sections/{bulletinSectionId}/edit")]
        public async Task<ActionResult> EditBulletinSection(int bulletinSectionId, [FromBody] EditBulletinSectionRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                throw new ValidationException("Model is not valid.");
            }

            await Executor.GetCommand<EditBulletinSectionCommand>()
                .Process(c => c.Execute(bulletinSectionId, requestDto));

            return Ok();
        }

        #endregion Bulletin Sections
    }
}