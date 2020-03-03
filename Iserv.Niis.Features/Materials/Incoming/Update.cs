using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.Model.Models.Material.Incoming;
using Iserv.Niis.Workflow.Abstract;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Materials.Incoming
{
    public class Update
    {
        public class Command : IRequest<MaterialIncomingDetailDto>
        {
            public Command(int documentId, MaterialIncomingDetailDto materialDetailDto, Owner.Type ownerType)
            {
                DocumentId = documentId;
                MaterialDetailDto = materialDetailDto;
                OwnerType = ownerType;
            }

            public int DocumentId { get; }
            public Owner.Type OwnerType { get; }
            public MaterialIncomingDetailDto MaterialDetailDto { get; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
        }

        public class CommandHandler : IAsyncRequestHandler<Command, MaterialIncomingDetailDto>
        {
            private readonly NiisWebContext _context;
            private readonly IMapper _mapper;
            private readonly IDocumentApplier<Domain.Entities.Request.Request> _requestDocumentApplier;
            private readonly IDocumentApplier<Domain.Entities.Contract.Contract> _contractDocumentApplier;

            public CommandHandler(
                NiisWebContext context, 
                IMapper mapper, 
                IDocumentApplier<Domain.Entities.Request.Request> requestDocumentApplier,
                IDocumentApplier<Domain.Entities.Contract.Contract> contractDocumentApplier
                )
            {
                _context = context;
                _mapper = mapper;
                _requestDocumentApplier = requestDocumentApplier;
                _contractDocumentApplier = contractDocumentApplier;
            }

            public async Task<MaterialIncomingDetailDto> Handle(Command message)
            {
                var documentId = message.DocumentId;
                var material = message.MaterialDetailDto;

                var document = await _context.Documents.SingleOrDefaultAsync(r => r.Id == documentId);
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

                var documentWithIncludes = await _context.Documents
                    .Include(r => r.Addressee)
                    .Include(r => r.Workflows).ThenInclude(r => r.FromStage)
                    .Include(r => r.Workflows).ThenInclude(r => r.CurrentStage)
                    .Include(r => r.Workflows).ThenInclude(r => r.FromUser)
                    .Include(r => r.Workflows).ThenInclude(r => r.CurrentUser)
                    .Include(r => r.Workflows).ThenInclude(r => r.Route)
                    .Include(r => r.MainAttachment)
                    .Include(r => r.Requests)
                    .Include(r => r.Contracts)
                    .Include(r => r.ProtectionDocs)
                    .SingleOrDefaultAsync(r => r.Id == documentId);

                var result = _mapper.Map<Document, MaterialIncomingDetailDto>(documentWithIncludes);
                var requestsOwnerDtos = _mapper.Map<RequestDocument[], MaterialOwnerDto[]>(documentWithIncludes.Requests.ToArray());
                var contractOwnerDtos = _mapper.Map<ContractDocument[], MaterialOwnerDto[]>(documentWithIncludes.Contracts.ToArray());
                var protectionDocOwnerDtos = _mapper.Map<ProtectionDocDocument[], MaterialOwnerDto[]>(documentWithIncludes.ProtectionDocs.ToArray());
                result.Owners = requestsOwnerDtos.Concat(contractOwnerDtos.Concat(protectionDocOwnerDtos)).ToArray();
                return result;
            }
        }
    }
}
