using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Features.Helpers;
using Iserv.Niis.FileStorage.Abstract;
using Iserv.Niis.Model.Constans;
using Iserv.Niis.Model.Models.Material;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DocumentType = Iserv.Niis.Domain.Enums.DocumentType;

namespace Iserv.Niis.Features.Materials
{
    public class ReplaceAttachment
    {
        public class Command : IRequest<MaterialDetailDto>
        {
            public Command(MaterialDetailDto materialIncomingDataDto, int userId)
            {
                MaterialIncomingDataDto = materialIncomingDataDto;
                UserId = userId;
            }

            public MaterialDetailDto MaterialIncomingDataDto { get; }
            public int UserId { get; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
        }

        public class CommandHandler : IAsyncRequestHandler<Command, MaterialDetailDto>
        {
            private static readonly string[] StageInitialCodes = { "TM01.1", "TMI01.1", "NMPT01.1", "B01.1", "B01.1_IN", "U01.1", "PO01.1", "AP01", "SA01.1" };
            private readonly IAttachmentHelper _attachmentHelper;
            private readonly NiisWebContext _context;
            private readonly IFileStorage _fileStorage;
            private readonly string _tempFolderPath;
            private readonly IMapper _mapper;
            private string _bucketName;

            public CommandHandler(NiisWebContext context, IFileStorage fileStorage, IAttachmentHelper attachmentHelper,
                IMapper mapper)
            {
                _context = context;
                _fileStorage = fileStorage;
                _attachmentHelper = attachmentHelper;
                _mapper = mapper;
                _tempFolderPath = Path.GetTempPath();
            }

            public async Task<MaterialDetailDto> Handle(Command message)
            {
                using (var fileStream = File.OpenRead(Path.Combine(_tempFolderPath,
                    message.MaterialIncomingDataDto.Attachment.TempName)))
                {
                    var document = await _context.Documents
                        .Include(d => d.MainAttachment)
                        .SingleAsync(d => d.Id == message.MaterialIncomingDataDto.Id);

                    switch (document.DocumentType)
                    {
                        case DocumentType.Incoming:
                            _bucketName = $"document-{document.Id}-incoming";
                            break;
                        case DocumentType.Outgoing:
                            _bucketName = $"document-{document.Id}-outgoing";
                            break;
                        case DocumentType.Internal:
                            _bucketName = $"document-{document.Id}-internal";
                            break;
                        default:
                            throw new Exception("Type is undefined");
                    }

                    Attachment newAttachment;

                    if (message.MaterialIncomingDataDto.Attachment.ContentType.Equals(ContentType.Pdf))
                    {
                        newAttachment = _attachmentHelper.NewPdfObject(message.MaterialIncomingDataDto.Attachment,
                            message.UserId, document.Id, _bucketName, fileStream, true);
                    }
                    else
                    {
                        newAttachment = _attachmentHelper.NewFileObject(message.MaterialIncomingDataDto.Attachment,
                            message.UserId, document.Id, _bucketName, fileStream, true);
                    }



                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            await _context.AddAsync(newAttachment);

                            var oldAttachment = document.MainAttachment;

                            document.MainAttachment = newAttachment;
                            if (newAttachment.PageCount.HasValue && message.MaterialIncomingDataDto.Owners.Any(o => o.OwnerType == Owner.Type.Request))
                            {
                                AddPageCountToRequest(message.MaterialIncomingDataDto.Owners.Where(o => o.OwnerType == Owner.Type.Request).Select(o => o.OwnerId).ToArray()[0], newAttachment.PageCount.Value, oldAttachment?.PageCount);
                            }
                            await _context.SaveChangesAsync();
                            await _fileStorage.AddAsync(document.MainAttachment.BucketName,
                                document.MainAttachment.ValidName, fileStream, document.MainAttachment.ContentType);

                            transaction.Commit();

                            if (oldAttachment != null)
                            {
                                await _fileStorage.Remove(oldAttachment.BucketName, oldAttachment.ValidName);
                                _context.Attachments.Remove(oldAttachment);
                            }
                        }
                        catch (Exception e)
                        {
                            throw;
                        }
                    }

                    return _mapper.Map<Document, MaterialDetailDto>(document);
                }

            }

            private void AddPageCountToRequest(int requestId, int pageCount, int? oldPageCount)
            {
                var request = _context.Requests
                    .Include(r => r.CurrentWorkflow)
                    .SingleOrDefault(r => r.Id == requestId);
                if (request == null)
                {
                    return;
                }

                var currentStageCode = _context.DicRouteStages.Single(s => s.Id == request.CurrentWorkflow.CurrentStageId).Code;
                if (!StageInitialCodes.Contains(currentStageCode))
                {
                    return;
                }

                if (request.PageCount.HasValue)
                {
                    request.PageCount = oldPageCount.HasValue
                        ? request.PageCount + pageCount - oldPageCount.Value
                        : request.PageCount + pageCount;
                }
                else
                {
                    request.PageCount = pageCount;
                }
            }
        }
    }
}
