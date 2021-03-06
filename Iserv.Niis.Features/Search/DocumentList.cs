﻿using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Model.Models.Search;
using MediatR;

namespace Iserv.Niis.Features.Search
{
    public class DocumentList
    {
        public class Query : IRequest<IQueryable<DocumentSearchDto>>
        {
        }

        public class QueryValidator : AbstractValidator<Query>
        {
        }

        public class QueryHandler : IAsyncRequestHandler<Query, IQueryable<DocumentSearchDto>>
        {
            private readonly NiisWebContext _context;
            private readonly IMapper _mapper;

            public QueryHandler(NiisWebContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public Task<IQueryable<DocumentSearchDto>> Handle(Query message)
            {
                return Task.FromResult(_mapper.Map<IQueryable<DocumentSearchDto>>(_context.Documents));
            }
        }
    }
}