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
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Iserv.Niis.Features.Materials.Outgoing
{
    public class Single
    {
        public class Query : IRequest<MaterialOutgoingDetailDto>
        {
            public Query(int documentId, int userId)
            {
                DocumentId = documentId;
                UserId = userId;
            }

            public int DocumentId { get; }
            public int UserId { get; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
        }

        public class QueryHandler : IAsyncRequestHandler<Query, MaterialOutgoingDetailDto>
        {
            private readonly NiisWebContext _context;
            private readonly IMapper _mapper;
            private readonly IDocumentGeneratorFactory _templateGeneratorFactory;

            public QueryHandler(IMapper mapper, IDocumentGeneratorFactory templateGeneratorFactory, NiisWebContext context)
            {
                _mapper = mapper;
                _context = context;
                _templateGeneratorFactory = templateGeneratorFactory;
            }

            async Task<MaterialOutgoingDetailDto> IAsyncRequestHandler<Query, MaterialOutgoingDetailDto>.Handle(
                Query message)
            {
                var documentId = message.DocumentId;

                var documentWithIncludes = await _context.Documents
                    .Include(d => d.Addressee)
                    .Include(d => d.Workflows).ThenInclude(w => w.FromStage)
                    .Include(d => d.Workflows).ThenInclude(w => w.CurrentStage)
                    .Include(d => d.Workflows).ThenInclude(w => w.FromUser)
                    .Include(d => d.Workflows).ThenInclude(w => w.CurrentUser)
                    .Include(d => d.Workflows).ThenInclude(w => w.Route)
                    .Include(d => d.Workflows).ThenInclude(w => w.DocumentUserSignature)
                    .Include(d => d.MainAttachment)
                    .Include(d => d.Requests)
                    .Include(d => d.Contracts)
                    .Include(d => d.ProtectionDocs)
                    .SingleOrDefaultAsync(r => r.Id == documentId);

                if (documentWithIncludes == null)
                    throw new DataNotFoundException(nameof(Document),
                        DataNotFoundException.OperationType.Read, documentId);

                var result = _mapper.Map<Document, MaterialOutgoingDetailDto>(documentWithIncludes);
                var requestsOwnerDtos = _mapper.Map<RequestDocument[], MaterialOwnerDto[]>(documentWithIncludes.Requests.ToArray());
                var contractOwnerDtos = _mapper.Map<ContractDocument[], MaterialOwnerDto[]>(documentWithIncludes.Contracts.ToArray());
                var protectionDocOwnerDtos = _mapper.Map<ProtectionDocDocument[], MaterialOwnerDto[]>(documentWithIncludes.ProtectionDocs.ToArray());
                result.Owners = requestsOwnerDtos.Concat(contractOwnerDtos.Concat(protectionDocOwnerDtos)).ToArray();

                var input = await _context.DocumentUserInputs
                    .SingleOrDefaultAsync(i => i.DocumentId == documentId);
                if (input != null)
                {
                    result.UserInput = JsonConvert.DeserializeObject<UserInputDto>(input.UserInput);
                    if (documentWithIncludes.MainAttachmentId == null)
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
                        result.PageCount = documentWithIncludes.MainAttachment.PageCount;
                    }
                }

                return result;
            }
        }
    }
}
