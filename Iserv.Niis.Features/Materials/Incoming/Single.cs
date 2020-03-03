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
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Materials.Incoming
{
    public class Single
    {
        public class Query : IRequest<MaterialIncomingDetailDto>
        {
            public Query(int documentId)
            {
                DocumentId = documentId;
            }

            public int DocumentId { get; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
        }

        public class QueryHandler : IAsyncRequestHandler<Query, MaterialIncomingDetailDto>
        {
            private readonly NiisWebContext _context;
            private readonly IMapper _mapper;

            public QueryHandler(IMapper mapper, NiisWebContext context)
            {
                _mapper = mapper;
                _context = context;
            }

            async Task<MaterialIncomingDetailDto> IAsyncRequestHandler<Query, MaterialIncomingDetailDto>.Handle(
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
                    .Include(d => d.MainAttachment)
                    .Include(d => d.Requests)
                    .Include(d => d.Contracts)
                    .Include(d => d.ProtectionDocs)
                    .SingleOrDefaultAsync(r => r.Id == documentId);

                if (documentWithIncludes == null)
                    throw new DataNotFoundException(nameof(Document), DataNotFoundException.OperationType.Read,
                        documentId);

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
