using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Features.Helpers;
using Iserv.Niis.Features.Materials;
using Iserv.Niis.Features.Materials.Compare;
using Iserv.Niis.FileStorage.Abstract;
using Iserv.Niis.Model.Models;
using Iserv.Niis.Portal.Infrastructure.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Portal.Controllers
{
    [Produces("application/json")]
    [Route("api/Documents")]
    public class DocumentsController : Controller
    {
        private readonly NiisWebContext _context;
        private readonly IFileStorage _fileStorage;
        private readonly IMediator _mediator;

        private string _bucketName;

        public DocumentsController(NiisWebContext context,
            IDocumentGeneratorFactory templateGeneratorFactory,
            ITemplateUserInputChecker templateUserInputChecker, IFileStorage fileStorage,
            IAttachmentHelper attachmentHelper,
            IMediator mediator)
        {
            _context = context;
            _fileStorage = fileStorage;
            _mediator = mediator;
        }

        //TODO: check access
        [HttpGet("{id}/{wasScanned}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id, bool wasScanned)
        {
            var result = await _mediator.Send(new GetDocument.Command(id, wasScanned, User.Identity.GetUserId()));

            return File(result.file, result.contentType);
        }

        [HttpGet("availableTypes/{id}")]
        public async Task<IActionResult> AvailableTypes(int id)
        {
            var result = _mediator.Send(new AvailableTypesList.Query());

            return Ok(result);
        }

        [HttpGet("getUserInputFields/{code}")]
        public async Task<IActionResult> GetUserInputFields(string code)
        {
            var result = await _mediator.Send(new UserInputFieldsList.Query(code));

            return Ok(result);
        }

        [HttpGet("getDocumetsInfoForCompare/{requestId}")]
        public async Task<IActionResult> GetDocumetsInfo(int requestId)
        {
            var result = await _mediator.Send(new DocumentsInfoForCompare.Query(requestId));
            return Ok(result);
        }

        [HttpGet("makeDocumentFinished/{documentId}")]
        public async Task<IActionResult> MakeDocumentFinished(int documentId)
        {
            await _mediator.Send(new DocumentFinished.Command(documentId));
            return NoContent();
        }

        [HttpGet("types/bycode/{code}")]
        public async Task<IActionResult> GetDictionariesByClassificationCode(string code)
        {
            var result = _context.DicDocumentTypes
                .Where(d => d.Classification.Code.StartsWith(code)
                && !d.Classification.Code.Equals("01.01.01")
                && !d.Classification.Code.Equals("01.01.02"));
            return Ok(result.ProjectTo<SelectOptionDto>());
        }
    }
}