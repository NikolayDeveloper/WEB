﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models.Material.Incoming;
using Iserv.Niis.Workflow.Abstract;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Materials.Incoming
{
    public class Create
    {
        public class Command : IRequest<int>
        {
            public Command(MaterialIncomingDetailDto materialDetailDto, int userId, Owner.Type ownerType)
            {
                MaterialDetailDto = materialDetailDto;
                UserId = userId;
                OwnerType = ownerType;
            }

            public MaterialIncomingDetailDto MaterialDetailDto { get; }
            public int UserId { get; }
            public Owner.Type OwnerType { get; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
        }

        public class CommandHandler : IAsyncRequestHandler<Command, int>
        {
            private readonly IDocumentApplier<Domain.Entities.Request.Request> _requestDocumentApplier;
            private readonly IDocumentApplier<Domain.Entities.Contract.Contract> _contractDocumentApplier;
            private readonly IWorkflowApplier<Document> _documentWorkflowApplier;
            private readonly NiisWebContext _context;
            private readonly INumberGenerator _numberGenerator;
            private readonly IMapper _mapper;

            public CommandHandler(
                NiisWebContext context,
                IDocumentApplier<Domain.Entities.Request.Request> requestDocumentApplier,
                IDocumentApplier<Domain.Entities.Contract.Contract> contractDocumentApplier,
                INumberGenerator numberGenerator,
                IWorkflowApplier<Document> documentWorkflowApplier,
                IMapper mapper)
            {
                _context = context;
                _requestDocumentApplier = requestDocumentApplier;
                _contractDocumentApplier = contractDocumentApplier;
                _documentWorkflowApplier = documentWorkflowApplier;
                _numberGenerator = numberGenerator;
                _mapper = mapper;
            }

            public async Task<int> Handle(Command message)
            {
                var document = _mapper.Map<Document>(message.MaterialDetailDto);
                document.DocumentType = DocumentType.Incoming;
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    _numberGenerator.GenerateBarcode(document);
                    _numberGenerator.GenerateIncomingNum(document);

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
                                contract.Documents.Add(new ContractDocument {Document = document});
                                break;
                            case Owner.Type.ProtectionDoc:
                                var protectionDoc =
                                    await _context.ProtectionDocs.SingleAsync(pd => pd.Id == materialOwnerDto.OwnerId);
                                protectionDoc.Documents.Add(new ProtectionDocDocument{Document = document});
                                break;
                        }
                    }

                    await _documentWorkflowApplier.ApplyInitialAsync(document, message.UserId);
                    await _context.SaveChangesAsync();
                    await _requestDocumentApplier.ApplyAsync(document.Id);
                    await _contractDocumentApplier.ApplyAsync(document.Id);
                    await _context.SaveChangesAsync();

                    transaction.Commit();
                }
                return document.Id;
            }
        }
    }
}
