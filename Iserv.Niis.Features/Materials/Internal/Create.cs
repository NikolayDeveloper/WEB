using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.Model.Models.Material.Internal;
using Iserv.Niis.Model.Models.Material.Outgoing;
using Iserv.Niis.Workflow.Abstract;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Iserv.Niis.Features.Materials.Internal
{
    public class Create
    {
        public class Command : IRequest<MaterialInternalDetailDto>
        {
            public Command(MaterialInternalDetailDto materialDetailDto, int userId, Owner.Type ownerType)
            {
                MaterialDetailDto = materialDetailDto;
                UserId = userId;
                OwnerType = ownerType;
            }

            public MaterialInternalDetailDto MaterialDetailDto { get; }
            public int UserId { get; }
            public Owner.Type OwnerType { get; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
        }

        public class CommandHandler : IAsyncRequestHandler<Command, MaterialInternalDetailDto>
        {
            private readonly IDocumentApplier<Domain.Entities.Request.Request> _requestDocumentApplier;
            private readonly IDocumentApplier<Domain.Entities.Contract.Contract> _contractDocumentApplier;
            private readonly IWorkflowApplier<Document> _documentWorkflowApplier;
            private readonly NiisWebContext _context;
            private readonly INumberGenerator _numberGenerator;
            private readonly IMapper _mapper;
            private readonly IDocumentGeneratorFactory _templateGeneratorFactory;

            public CommandHandler(
                NiisWebContext context,
                IDocumentApplier<Domain.Entities.Request.Request> requestDocumentApplier,
                IDocumentApplier<Domain.Entities.Contract.Contract> contractDocumentApplier,
                INumberGenerator numberGenerator,
                IWorkflowApplier<Document> documentWorkflowApplier,
                IMapper mapper,
                IDocumentGeneratorFactory templateGeneratorFactory)
            {
                _context = context;
                _requestDocumentApplier = requestDocumentApplier;
                _contractDocumentApplier = contractDocumentApplier;
                _documentWorkflowApplier = documentWorkflowApplier;
                _numberGenerator = numberGenerator;
                _mapper = mapper;
                _templateGeneratorFactory = templateGeneratorFactory;
            }

            public async Task<MaterialInternalDetailDto> Handle(Command message)
            {
                var document = _mapper.Map<Document>(message.MaterialDetailDto);
                document.DocumentType = DocumentType.Internal;
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    _numberGenerator.GenerateBarcode(document);

                    await _context.AddAsync(document);
                    await _context.SaveChangesAsync();

                    foreach (var materialOwnerDto in message.MaterialDetailDto.Owners)
                    {
                        switch (materialOwnerDto.OwnerType)
                        {
                            case Owner.Type.Request:
                            {
                                var request = await _context.Requests.SingleAsync(r => r.Id == materialOwnerDto.OwnerId);
                                request.Documents.Add(new RequestDocument { Document = document });
                                break;
                            }
                            case Owner.Type.Contract:
                                var contract = await _context.Contracts.SingleAsync(c => c.Id == materialOwnerDto.OwnerId);
                                contract.Documents.Add(new ContractDocument { Document = document });
                                break;
                            case Owner.Type.ProtectionDoc:
                                var protectionDoc =
                                    await _context.ProtectionDocs.SingleAsync(pd => pd.Id == materialOwnerDto.OwnerId);
                                protectionDoc.Documents.Add(new ProtectionDocDocument { Document = document });
                                break;
                        }
                    }

                    await _documentWorkflowApplier.ApplyInitialAsync(document, message.UserId);

                    var documentGenerator = _templateGeneratorFactory.Create(message.MaterialDetailDto.UserInput.Code);
                    var generatedFile = documentGenerator?.Process(new Dictionary<string, object>
                    {
                        {"UserId", message.UserId},
                        {"RequestId", message.MaterialDetailDto.UserInput.OwnerId},
                        {"DocumentId", document.Id},
                        {"UserInputFields", message.MaterialDetailDto.UserInput.Fields},
                        { "SelectedRequestIds", message.MaterialDetailDto.UserInput.SelectedRequestIds },
                        {"PageCount", message.MaterialDetailDto.PageCount}
                    });

                    message.MaterialDetailDto.UserInput.DocumentId = document.Id;
                    message.MaterialDetailDto.UserInput.PageCount = generatedFile?.PageCount;

                    var input = new DocumentUserInput
                    {
                        DocumentId = document.Id,
                        UserInput = JsonConvert.SerializeObject(message.MaterialDetailDto.UserInput)
                    };
                    _context.DocumentUserInputs.Add(input);
                    await _context.SaveChangesAsync();

                    await _requestDocumentApplier.ApplyAsync(document.Id);
                    await _contractDocumentApplier.ApplyAsync(document.Id);

                    var documentWithIncludes = await _context.Documents
                        .Include(d => d.Addressee)
                        .Include(d => d.Workflows).ThenInclude(w => w.FromStage)
                        .Include(d => d.Workflows).ThenInclude(w => w.CurrentStage)
                        .Include(d => d.Workflows).ThenInclude(w => w.FromUser)
                        .Include(d => d.Workflows).ThenInclude(w => w.CurrentUser)
                        .Include(d => d.Workflows).ThenInclude(w => w.Route)
                        .Include(d => d.MainAttachment)
                        .Include(d => d.Requests)
                        .Include(d => d.Contracts)
                        .SingleOrDefaultAsync(r => r.Id == document.Id);

                    transaction.Commit();
                    var result = _mapper.Map<MaterialInternalDetailDto>(documentWithIncludes);
                    var requestsOwnerDtos = _mapper.Map<RequestDocument[], MaterialOwnerDto[]>(documentWithIncludes.Requests.ToArray());
                    var contractOwnerDtos = _mapper.Map<ContractDocument[], MaterialOwnerDto[]>(documentWithIncludes.Contracts.ToArray());
                    var protectionDocOwnerDtos = _mapper.Map<ProtectionDocDocument[], MaterialOwnerDto[]>(documentWithIncludes.ProtectionDocs.ToArray());
                    result.Owners = requestsOwnerDtos.Concat(contractOwnerDtos.Concat(protectionDocOwnerDtos)).ToArray();
                    return result;
                }
            }
        }
    }
}
