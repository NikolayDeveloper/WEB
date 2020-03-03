using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.Model.Models.Material.Outgoing;
using Iserv.Niis.Workflow.Abstract;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Iserv.Niis.Features.Materials.Outgoing
{
    public class Update
    {
        public class Command : IRequest<MaterialOutgoingDetailDto>
        {
            public Command(int documentId, int userId, MaterialOutgoingDetailDto materialDetailDto, Owner.Type ownerType)
            {
                DocumentId = documentId;
                UserId = userId;
                MaterialDetailDto = materialDetailDto;
                OwnerType = ownerType;
            }

            public int DocumentId { get; }
            public int UserId { get; }
            public Owner.Type OwnerType { get; }
            public MaterialOutgoingDetailDto MaterialDetailDto { get; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
        }

        public class CommandHandler : IAsyncRequestHandler<Command, MaterialOutgoingDetailDto>
        {
            private readonly NiisWebContext _context;
            private readonly IMapper _mapper;
            private readonly IDocumentGeneratorFactory _templateGeneratorFactory;
            private IDocumentApplier<Domain.Entities.Request.Request> _requestDocumentApplier;
            private readonly IDocumentApplier<Domain.Entities.Contract.Contract> _contractDocumentApplier;

            public CommandHandler(
                NiisWebContext context, 
                IMapper mapper, 
                IDocumentGeneratorFactory templateGeneratorFactory, 
                IDocumentApplier<Domain.Entities.Request.Request> requestDocumentApplier,
                IDocumentApplier<Domain.Entities.Contract.Contract> contractDocumentApplier
                )
            {
                _context = context;
                _mapper = mapper;
                _templateGeneratorFactory = templateGeneratorFactory;
                _requestDocumentApplier = requestDocumentApplier;
                _contractDocumentApplier = contractDocumentApplier;
            }

            public async Task<MaterialOutgoingDetailDto> Handle(Command message)
            {
                var documentId = message.DocumentId;
                var material = message.MaterialDetailDto;

                var document = await _context.Documents
                    .Include(d=> d.Type)
                    .SingleOrDefaultAsync(r => r.Id == documentId);
                if (document == null)
                {
                    throw new DataNotFoundException(nameof(Document),
                        DataNotFoundException.OperationType.Update, documentId);
                }

                _mapper.Map(material, document);


                await _requestDocumentApplier.ApplyAsync(documentId);
                await _contractDocumentApplier.ApplyAsync(documentId);
                await _context.SaveChangesAsync();

                var requestLinksToRemove = _context.RequestsDocuments
                    .Where(rd => rd.DocumentId == documentId
                                 && !material.Owners.Where(o => o.OwnerType == Owner.Type.Request)
                                     .Select(o => o.OwnerId).Contains(rd.RequestId));

                var requestLinksToAdd = _context.Requests
                    .Where(r => material.Owners.Where(o => o.OwnerType == Owner.Type.Request)
                        .Select(o => o.OwnerId).Contains(r.Id))
                    .Where(r => r.Documents.All(rd => rd.DocumentId != documentId))
                    .Select(r => new RequestDocument { RequestId = r.Id, DocumentId = documentId })
                    .ToList();

                _context.RequestsDocuments.RemoveRange((requestLinksToRemove));
                await _context.RequestsDocuments.AddRangeAsync(requestLinksToAdd);
                await _context.SaveChangesAsync();

                var contractLinksToRemove = _context.ContractsDocuments
                    .Where(rd => rd.DocumentId == documentId
                                 && !material.Owners.Where(o => o.OwnerType == Owner.Type.Contract)
                                     .Select(o => o.OwnerId).Contains(rd.ContractId));

                var contractLinksToAdd = _context.Contracts
                    .Where(c => material.Owners.Where(o => o.OwnerType == Owner.Type.Contract)
                        .Select(o => o.OwnerId).Contains(c.Id))
                    .Where(c => c.Documents.All(cd => cd.DocumentId != documentId))
                    .Select(c => new ContractDocument { ContractId = c.Id, DocumentId = documentId })
                    .ToList();

                _context.ContractsDocuments.RemoveRange(contractLinksToRemove);
                await _context.ContractsDocuments.AddRangeAsync(contractLinksToAdd);
                await _context.SaveChangesAsync();

                var protectionDocsLinksToRemove = _context.ProtectionDocDocuments
                    .Where(pd => pd.DocumentId == documentId
                                 && !material.Owners.Where(o => o.OwnerType == Owner.Type.ProtectionDoc)
                                     .Select(o => o.OwnerId).Contains(pd.ProtectionDocId));

                var protectionDocsLinksToAdd = _context.ProtectionDocs
                    .Where(pd => material.Owners.Where(o => o.OwnerType == Owner.Type.ProtectionDoc)
                        .Select(o => o.OwnerId).Contains(pd.Id))
                    .Where(c => c.Documents.All(pd => pd.DocumentId != documentId))
                    .Select(c => new ProtectionDocDocument { ProtectionDocId = c.Id, DocumentId = documentId })
                    .ToList();

                _context.ProtectionDocDocuments.RemoveRange(protectionDocsLinksToRemove);
                await _context.ProtectionDocDocuments.AddRangeAsync(protectionDocsLinksToAdd);
                await _context.SaveChangesAsync();

                var input = await _context.DocumentUserInputs
                    .SingleOrDefaultAsync(i => i.DocumentId == documentId);
                if (input == null)
                {
                    input = new DocumentUserInput
                    {
                        DocumentId = document.Id,
                        UserInput = JsonConvert.SerializeObject(message.MaterialDetailDto.UserInput)
                    };
                    _context.DocumentUserInputs.Add(input);
                }
                else
                {
                    input.UserInput = JsonConvert.SerializeObject(message.MaterialDetailDto.UserInput);
                }

                try
                {
                    await _context.SaveChangesAsync();

                    var documentWithIncludes = await _context.Documents
                        .Include(r => r.Addressee)
                        .Include(r => r.Workflows).ThenInclude(r => r.FromStage)
                        .Include(r => r.Workflows).ThenInclude(r => r.CurrentStage)
                        .Include(r => r.Workflows).ThenInclude(r => r.FromUser)
                        .Include(r => r.Workflows).ThenInclude(r => r.CurrentUser)
                        .Include(r => r.Workflows).ThenInclude(r => r.Route)
                        .Include(r => r.Workflows).ThenInclude(r => r.DocumentUserSignature)
                        .Include(r => r.MainAttachment)
                        .Include(r => r.Requests)
                        .Include(d => d.Contracts)
                        .Include(d => d.ProtectionDocs)
                        .SingleOrDefaultAsync(r => r.Id == documentId);

                    var result = _mapper.Map<Document, MaterialOutgoingDetailDto>(documentWithIncludes);
                    var requestsOwnerDtos = _mapper.Map<RequestDocument[], MaterialOwnerDto[]>(documentWithIncludes.Requests.ToArray());
                    var contractOwnerDtos = _mapper.Map<ContractDocument[], MaterialOwnerDto[]>(documentWithIncludes.Contracts.ToArray());
                    var protectionDocOwnerDtos = _mapper.Map<ProtectionDocDocument[], MaterialOwnerDto[]>(documentWithIncludes.ProtectionDocs.ToArray());
                    result.Owners = requestsOwnerDtos.Concat(contractOwnerDtos.Concat(protectionDocOwnerDtos)).ToArray();

                    result.UserInput = JsonConvert.DeserializeObject<UserInputDto>(input.UserInput);
                    if (document.MainAttachmentId == null)
                    {
                        var documentGenerator = _templateGeneratorFactory.Create(result.UserInput.Code);
                        var generatedFile = documentGenerator.Process(new Dictionary<string, object>
                        {
                            {"UserId", message.UserId},
                            {"RequestId", result.UserInput.OwnerId},
                            {"DocumentId", result.UserInput.DocumentId},
                            {"UserInputFields", result.UserInput.Fields},
                            { "SelectedRequestIds", result.UserInput.SelectedRequestIds },
                            {"PageCount", result.PageCount}
                        });
                        result.PageCount = generatedFile.PageCount;
                    }
                    else
                    {
                        result.PageCount = document.MainAttachment.PageCount;
                    }

                    return result;
                }
                catch (Exception e)
                {
                    throw new DatabaseException(e.InnerException?.Message ?? e.Message);
                }
            }
        }
    }
}
