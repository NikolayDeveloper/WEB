using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.FileStorage.Abstract;
using Iserv.Niis.Model.Constans;
using Iserv.Niis.Model.Models.Material;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Iserv.Niis.Features.Materials
{
    public class GetDocument
    {
        public class Command : IRequest<(byte[] file, string contentType)>
        {
            public Command(int id, bool wasScanned, int userId)
            {
                Id = id;
                WasScanned = wasScanned;
                UserId = userId;
            }

            public int Id { get; }
            public bool WasScanned { get; }
            public int UserId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
        }

        public class CommandHandler : IAsyncRequestHandler<Command, (byte[] file, string contentType)>
        {
            private readonly IDocumentGeneratorFactory _templateGeneratorFactory;
            private readonly IFileStorage _fileStorage;
            private readonly NiisWebContext _context;

            public CommandHandler(IDocumentGeneratorFactory templateGeneratorFactory, IFileStorage fileStorage,  NiisWebContext context)
            {
                _templateGeneratorFactory = templateGeneratorFactory;
                _fileStorage = fileStorage;
                _context = context;
            }

            public async Task<(byte[] file, string contentType)> Handle(Command message)
            {
                var document = await _context.Documents
                    .Include(x => x.MainAttachment)
                    .SingleOrDefaultAsync(x => x.Id == message.Id);
                byte[] result;

                if (document == null)
                    throw new DataNotFoundException(nameof(Domain.Entities.Document.Document),
                        DataNotFoundException.OperationType.Read, message.Id);

                var input = await _context.DocumentUserInputs
                    .SingleOrDefaultAsync(i => i.DocumentId == document.Id);

                if (input != null && !message.WasScanned)
                {
                    var dto = JsonConvert.DeserializeObject<UserInputDto>(input.UserInput);

                    var documentGenerator = _templateGeneratorFactory.Create(dto.Code);
                    var generatedFile = documentGenerator.Process(new Dictionary<string, object>
                    {
                        {"UserId", message.UserId},
                        {"RequestId", dto.OwnerId},
                        {"DocumentId", dto.DocumentId},
                        {"UserInputFields", dto.Fields},
                        { "SelectedRequestIds", dto.SelectedRequestIds },
                        {"PageCount", dto.PageCount}
                    });
                    return (file: generatedFile.File, contentType: ContentType.Pdf);
                }

                result = await _fileStorage.GetAsync(document.MainAttachment.BucketName,
                    document.MainAttachment.OriginalName);
                return (file:result, contentType: document.MainAttachment.ContentType);
            }
        }
    }
}
