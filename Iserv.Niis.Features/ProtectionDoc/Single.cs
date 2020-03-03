using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Model.Models.ProtectionDoc;
using Iserv.Niis.Model.Models.Request;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.ProtectionDoc
{
    public class Single
    {
        public class Query : IRequest<ProtectionDocDetailsDto>
        {
            public Query(int protectionDocId, int userId)
            {
                ProtectionDocId = protectionDocId;
                UserId = userId;
            }

            public int ProtectionDocId { get; }
            public int UserId { get; }
        }

        public class QueryValidator : AbstractValidator<Query> { }

        public class QueryHandler : IAsyncRequestHandler<Query, ProtectionDocDetailsDto>
        {
            private readonly NiisWebContext _context;
            private readonly IMapper _mapper;

            public QueryHandler(IMapper mapper, NiisWebContext context)
            {
                _mapper = mapper;
                _context = context;
            }

            async Task<ProtectionDocDetailsDto> IAsyncRequestHandler<Query, ProtectionDocDetailsDto>.Handle(Query message)
            {
                var protectionDoc = await _context.ProtectionDocs
                    .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
                    .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.FromStage)
                    .Include(r => r.Type)
                    .Include(r => r.SubType)
                    .SingleOrDefaultAsync(pd => pd.Id == message.ProtectionDocId);
                if (protectionDoc == null)
                {
                    throw new DataNotFoundException(nameof(Domain.Entities.ProtectionDoc.ProtectionDoc),
                        DataNotFoundException.OperationType.Read, message.ProtectionDocId.ToString());
                }

                return _mapper.Map<Domain.Entities.ProtectionDoc.ProtectionDoc, ProtectionDocDetailsDto>(protectionDoc,
                    opt => opt.Items["ProtectionDocCustomers"] = protectionDoc.ProtectionDocCustomers);
            }
        }
    }
}
